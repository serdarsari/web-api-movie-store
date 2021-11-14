using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.DTO.MovieDTO;
using MovieStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetMoviesResponse> GetMoviesAsync(GetMoviesRequest request)
        {
            var currentStartRow = (request.PageNumber - 1) * request.PageSize;
            var response = new GetMoviesResponse
            {
                NextPage = $"api/Movies?PageNumber={request.PageNumber + 1}&PageSize={request.PageSize}",
                TotalMovies = await _context.Movies.CountAsync(),
            };

            var movies = await _context.Movies
                .Skip(currentStartRow)
                .Take(request.PageSize)
                .Select(m => new MovieResponse
                {
                    Name = m.Name,
                    ReleaseDate = $"{m.Month}, {m.Year}",
                    Genre = m.Genre,
                    Budget = m.Budget,
                    Rating = m.Rating,
                    Storyline = m.Storyline,
                })
                .ToListAsync();
            response.Movies = movies;

            return response;
        }

        public async Task<GetMovieDetailResponse> GetMovieDetailAsync(int id)
        {
            try
            {
                var movie = await _context.Movies.SingleOrDefaultAsync(x => x.MovieId == id);

                if (movie is null)
                    return new GetMovieDetailResponse { ErrorMessage = "You have entered an invalid Id." };

                GetMovieDetailResponse response = _mapper.Map<GetMovieDetailResponse>(movie);

                var actorsIds = await _context.MovieActors
                    .Where(x => x.MovieId == movie.MovieId)
                    .Select(x => x.ActorId).ToListAsync();

                var actorsNames = await _context.Actors.Where(x => actorsIds.Contains(x.ActorId)).Select(x => $"{x.FirstName} {x.LastName}").ToListAsync();

                var directorsIds = await _context.MovieDirectors
                    .Where(x => x.MovieId == movie.MovieId)
                    .Select(x => x.DirectorId).ToListAsync();

                var directorsNames = await _context.Directors.Where(x => directorsIds.Contains(x.DirectorId)).Select(x => $"{x.FirstName} {x.LastName}").ToListAsync();
                
                response.Actors = actorsNames;
                response.Directors = directorsNames;

                return response;
            }
            catch (Exception ex)
            {
                return new GetMovieDetailResponse { ErrorMessage = ex.Message };
            }
        }

        public async Task<CreateMovieResponse> CreateMovieAsync(CreateMovieRequest request)
        {
            var result = await _context.Movies.SingleOrDefaultAsync(x => x.Name == request.Name);
            if (result is not null)
            {
                return new CreateMovieResponse
                {
                    IsSuccess = false,
                    Message = "This movie already exists."
                };
            }


            //Transaction Begin
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Movie movie = _mapper.Map<Movie>(request);

                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();

                foreach (var actorId in request.ActorsIds)
                {
                    var actor = await _context.Actors.SingleOrDefaultAsync(x => x.ActorId == actorId);
                    if (actor is not null)
                    {
                        MovieActor movieActor = new()
                        {
                            MovieId = movie.MovieId,
                            ActorId = actorId,
                        };

                        await _context.MovieActors.AddAsync(movieActor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateMovieResponse
                        {
                            IsSuccess = false,
                            Message = "You entered a non-existent ActorId."
                        };
                    }
                }

                foreach (var directorId in request.DirectorsIds)
                {
                    var director = await _context.Directors.SingleOrDefaultAsync(x => x.DirectorId == directorId);
                    if (director is not null)
                    {
                        MovieDirector movieDirector = new()
                        {
                            MovieId = movie.MovieId,
                            DirectorId = directorId,
                        };

                        await _context.MovieDirectors.AddAsync(movieDirector);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateMovieResponse
                        {
                            IsSuccess = false,
                            Message = "You entered a non-existent DirectorId."
                        };
                    }
                }

                transaction.Commit();       //Transaction Success

                return new CreateMovieResponse
                {
                    IsSuccess = true,
                    Message = "create successful"
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();     //Transaction Failure

                return new CreateMovieResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DeleteMovieResponse> DeleteMovieAsync(int id)
        {
            try
            {
                var movie = await _context.Movies.SingleOrDefaultAsync(x => x.MovieId == id);

                if (movie is null)
                {
                    return new DeleteMovieResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return new DeleteMovieResponse
                {
                    IsSuccess = true,
                    Message = "delete successful"
                };
            }
            catch (Exception ex)
            {
                return new DeleteMovieResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<UpdateMovieResponse> UpdateMovieAsync(int id, UpdateMovieRequest request)
        {
            try
            {
                var movie = await _context.Movies.SingleOrDefaultAsync(x => x.MovieId == id);

                if (movie is null)
                {
                    return new UpdateMovieResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                movie.Name = request.Name != default ? request.Name : movie.Name;
                movie.Month = request.Month != default ? request.Month : movie.Month;
                movie.Year = request.Year != default ? request.Year : movie.Year;
                movie.Budget = request.Budget != default ? request.Budget : movie.Budget;
                movie.Rating = request.Rating != default ? request.Rating : movie.Rating;
                movie.Storyline = request.Storyline != default ? request.Storyline : movie.Storyline;

                await _context.SaveChangesAsync();

                return new UpdateMovieResponse
                {
                    IsSuccess = true,
                    Message = "update successful"
                };
            }
            catch (Exception ex)
            {
                return new UpdateMovieResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }
    }
}
