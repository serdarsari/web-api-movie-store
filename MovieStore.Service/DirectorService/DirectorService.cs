using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieStore.DTO.DirectorDTO;
using MovieStore.Entity;
using MovieStore.Service.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.Service.DirectorService
{
    public class DirectorService : IDirectorService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly CustomMemoryCache _memoryCache;

        public DirectorService(MovieStoreDbContext context, IMapper mapper, CustomMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<GetDirectorsResponse> GetDirectorsAsync(GetDirectorsRequest request)
        {
            var currentStartRow = (request.PageNumber - 1) * request.PageSize;
            var response = new GetDirectorsResponse
            {
                NextPage = $"api/Directors?PageNumber={request.PageNumber + 1}&PageSize={request.PageSize}",
                TotalDirectors = await _context.Directors.CountAsync(),
            };

            if (!_memoryCache.Cache.TryGetValue("AllDirectors", out List<Director> allDirectors))
            {
                allDirectors = await _context.Directors.ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12))
                    .SetPriority(CacheItemPriority.High)
                    .SetSize(1)
                    .RegisterPostEvictionCallback(AllDirectorsCallback, _memoryCache);

                _memoryCache.Cache.Set("AllDirectors", allDirectors, cacheEntryOptions);
            }

            var directors = allDirectors
                .Skip(currentStartRow)
                .Take(request.PageSize)
                .Select(m => new DirectorResponse
                {
                    FullName = $"{m.FirstName} {m.LastName}",
                    Born = $"{m.DateOfBirth:MMMM} {m.DateOfBirth:dd}, {m.PlaceOfBirth}"
                }).ToList();

            response.Directors = directors;

            return response;
        }
        private static void AllDirectorsCallback(
            object cacheKey, object cacheValue, EvictionReason evictionReason, object state)
        {
            var memoryCache = (CustomMemoryCache)state;

            memoryCache.Cache.Set(
                "AllDirectorsCallbackMessage",
                $"Entry {cacheKey} was evicted: {evictionReason}.", new MemoryCacheEntryOptions { Size = 1 });
        }

        public async Task<GetDirectorDetailResponse> GetDirectorDetailAsync(int id)
        {
            try
            {
                var director = await _context.Directors.SingleOrDefaultAsync(x => x.DirectorId == id);
                if (director is null)
                    return new GetDirectorDetailResponse { ErrorMessage = "You have entered an invalid Id." };

                GetDirectorDetailResponse response = _mapper.Map<GetDirectorDetailResponse>(director);

                var moviesIds = await _context.MovieDirectors
                        .Where(x => x.DirectorId == director.DirectorId)
                        .Select(x => x.MovieId).ToListAsync();

                var moviesNames = await _context.Movies.Where(x => moviesIds.Contains(x.MovieId)).Select(x => x.Name).ToListAsync();

                var awardsIds = await _context.DirectorAwardWinners
                        .Where(x => x.DirectorId == director.DirectorId)
                        .Select(x => x.AwardId).ToListAsync();

                var awardsNames = await _context.Awards.Where(x => awardsIds.Contains(x.AwardId)).Select(x => x.Name).ToListAsync();

                response.Awards = awardsNames;
                response.Movies = moviesNames;

                return response;
            }
            catch (Exception ex)
            {
                return new GetDirectorDetailResponse { ErrorMessage = ex.Message };
            }
        }

        public async Task<CreateDirectorResponse> CreateDirectorAsync(CreateDirectorRequest request)
        {

            //Transaction Begin
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Director director = _mapper.Map<Director>(request);
                await _context.Directors.AddAsync(director);
                await _context.SaveChangesAsync();

                foreach (var movieId in request.MoviesIds)
                {
                    var movie = await _context.Movies.SingleOrDefaultAsync(x => x.MovieId == movieId);
                    if (movie is not null)
                    {
                        MovieDirector movieDirector = new()
                        {
                            MovieId = movieId,
                            DirectorId = director.DirectorId,
                        };

                        await _context.MovieDirectors.AddAsync(movieDirector);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateDirectorResponse
                        {
                            IsSuccess = false,
                            Message = "You entered a non-existent MovieId."
                        };
                    }
                }


                foreach (var awardId in request.AwardsIds)
                {
                    var award = await _context.Awards.SingleOrDefaultAsync(x => x.AwardId == awardId);
                    if (award is not null)
                    {
                        DirectorAwardWinner directorAwardWinner = new()
                        {
                            AwardId = awardId,
                            DirectorId = director.DirectorId
                        };

                        await _context.DirectorAwardWinners.AddAsync(directorAwardWinner);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateDirectorResponse
                        {
                            IsSuccess = false,
                            Message = "You entered a non-existent AwardId."
                        };
                    }
                }

                transaction.Commit();       //Transaction Success
                _memoryCache.Cache.Remove("AllDirectors");

                return new CreateDirectorResponse
                {
                    IsSuccess = true,
                    Message = "create successful"
                };

            }
            catch (Exception ex)
            {
                transaction.Rollback();     //Transaction Failure

                return new CreateDirectorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DeleteDirectorResponse> DeleteDirectorAsync(int directorId)
        {
            try
            {
                var director = await _context.Directors.SingleOrDefaultAsync(x => x.DirectorId == directorId);

                if (director is null)
                {
                    return new DeleteDirectorResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();

                return new DeleteDirectorResponse
                {
                    IsSuccess = true,
                    Message = "delete successful"
                };
            }
            catch (Exception ex)
            {
                return new DeleteDirectorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }


        }

        public async Task<UpdateDirectorResponse> UpdateDirectorAsync(int directorId, UpdateDirectorRequest request)
        {
            try
            {
                var director = await _context.Directors.SingleOrDefaultAsync(x => x.DirectorId == directorId);

                if (director is null)
                {
                    return new UpdateDirectorResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                director.FirstName = request.FirstName != default ? request.FirstName : director.FirstName;
                director.LastName = request.LastName != default ? request.LastName : director.LastName;
                director.Biography = request.Biography != default ? request.Biography : director.Biography;

                await _context.SaveChangesAsync();

                return new UpdateDirectorResponse
                {
                    IsSuccess = true,
                    Message = "update successful"
                };
            }
            catch (Exception ex)
            {
                return new UpdateDirectorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
