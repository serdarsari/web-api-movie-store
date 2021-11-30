using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MovieStore.API.Validations.ActorValidations;
using MovieStore.API.Validations.AwardValidations;
using MovieStore.API.Validations.DirectorValidations;
using MovieStore.API.Validations.MovieValidations;
using MovieStore.DTO.ActorDTO;
using MovieStore.DTO.AwardDTO;
using MovieStore.DTO.DirectorDTO;
using MovieStore.DTO.MovieDTO;
using MovieStore.Entity;
using MovieStore.Service.ActorService;
using MovieStore.Service.AwardService;
using MovieStore.Service.Caching;
using MovieStore.Service.DirectorService;
using MovieStore.Service.MovieService;
using System.Reflection;

namespace MovieStore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MovieStoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MovieStoreDbConnStr"), b => b.MigrationsAssembly("MovieStore.API"));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IActorService, ActorService>();
            services.AddTransient<IDirectorService, DirectorService>();
            services.AddTransient<IAwardService, AwardService>();

            services.AddTransient<IValidator<CreateMovieRequest>, CreateMovieRequestValidator>();   //Fluent Validation
            services.AddTransient<IValidator<UpdateMovieRequest>, UpdateMovieRequestValidator>();
            services.AddTransient<IValidator<GetMoviesRequest>, GetMoviesRequestValidator>();

            services.AddTransient<IValidator<CreateActorRequest>, CreateActorRequestValidator>();
            services.AddTransient<IValidator<UpdateActorRequest>, UpdateActorRequestValidator>();
            services.AddTransient<IValidator<GetActorsRequest>, GetActorsRequestValidator>();

            services.AddTransient<IValidator<CreateDirectorRequest>, CreateDirectorRequestValidator>();
            services.AddTransient<IValidator<UpdateDirectorRequest>, UpdateDirectorRequestValidator>();
            services.AddTransient<IValidator<GetDirectorsRequest>, GetDirectorsRequestValidator>();

            services.AddTransient<IValidator<CreateAwardRequest>, CreateAwardRequestValidator>();
            services.AddTransient<IValidator<UpdateAwardRequest>, UpdateAwardRequestValidator>();
            services.AddTransient<IValidator<GetAwardsRequest>, GetAwardsRequestValidator>();

            services.AddMemoryCache();
            services.AddSingleton<CustomMemoryCache>();


            services.AddControllers().AddFluentValidation(i => i.DisableDataAnnotationsValidation = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieStore.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieStore.API v1"));
            }

            app.UseResponseCaching();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
