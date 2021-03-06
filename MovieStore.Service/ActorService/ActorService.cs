using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieStore.DTO.ActorDTO;
using MovieStore.Entity;
using MovieStore.Service.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.Service.ActorService
{
    public class ActorService : IActorService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly CustomMemoryCache _memoryCache;

        public ActorService(MovieStoreDbContext context, IMapper mapper, CustomMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<GetActorsResponse> GetActorsAsync(GetActorsRequest request)
        {
            var currentStartRow = (request.PageNumber - 1) * request.PageSize;
            var response = new GetActorsResponse
            {
                NextPage = $"api/Actors?PageNumber={request.PageNumber + 1}&PageSize={request.PageSize}",
                TotalActors = await _context.Actors.CountAsync(),
            };

            if (!_memoryCache.Cache.TryGetValue("AllActors", out List<Actor> allActors))
            {
                allActors = await _context.Actors.ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12))
                    .SetPriority(CacheItemPriority.High)
                    .SetSize(1)
                    .RegisterPostEvictionCallback(AllActorsCallback, _memoryCache);

                _memoryCache.Cache.Set("AllActors", allActors, cacheEntryOptions);
            }

            var actors = allActors
                .Skip(currentStartRow)
                .Take(request.PageSize)
                .Select(m => new ActorResponse
                {
                    FullName = $"{m.FirstName} {m.LastName}",
                    Born = $"{m.DateOfBirth:MMMM} {m.DateOfBirth:dd}, {m.PlaceOfBirth}"
                }).ToList();

            response.Actors = actors;

            return response;
        }
        private static void AllActorsCallback(
            object cacheKey, object cacheValue, EvictionReason evictionReason, object state)
        {
            var memoryCache = (CustomMemoryCache)state;

            memoryCache.Cache.Set(
                "AllActorsCallbackMessage",
                $"Entry {cacheKey} was evicted: {evictionReason}.", new MemoryCacheEntryOptions { Size = 1 });  //SizeLimit verdiğim için, her Set ederken size belirtmek zorundayım.
        }



        public async Task<GetActorDetailResponse> GetActorDetailAsync(int id)
        {
            try
            {
                var actor = await _context.Actors.SingleOrDefaultAsync(x => x.ActorId == id);
                if (actor is null)
                    return new GetActorDetailResponse { ErrorMessage = "You have entered an invalid Id." };

                GetActorDetailResponse response = _mapper.Map<GetActorDetailResponse>(actor);

                var moviesIds = await _context.MovieActors
                        .Where(x => x.ActorId == actor.ActorId)
                        .Select(x => x.MovieId).ToListAsync();

                var moviesNames = await _context.Movies.Where(x => moviesIds.Contains(x.MovieId)).Select(x => x.Name).ToListAsync();

                var awardsIds = await _context.ActorAwardWinners
                        .Where(x => x.ActorId == actor.ActorId)
                        .Select(x => x.AwardId).ToListAsync();

                var awardsNames = await _context.Awards.Where(x => awardsIds.Contains(x.AwardId)).Select(x => x.Name).ToListAsync();

                response.Awards = awardsNames;
                response.Movies = moviesNames;

                return response;
            }
            catch (Exception ex)
            {
                return new GetActorDetailResponse { ErrorMessage = ex.Message };
            }
        }

        public async Task<CreateActorResponse> CreateActorAsync(CreateActorRequest request)
        {

            //Transaction Begin
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Actor actor = _mapper.Map<Actor>(request);
                await _context.Actors.AddAsync(actor);
                await _context.SaveChangesAsync();

                foreach (var movieId in request.MoviesIds)
                {
                    var movie = await _context.Movies.SingleOrDefaultAsync(x => x.MovieId == movieId);
                    if (movie is not null)
                    {
                        MovieActor movieActor = new()
                        {
                            MovieId = movieId,
                            ActorId = actor.ActorId,
                        };

                        await _context.MovieActors.AddAsync(movieActor);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateActorResponse
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
                        ActorAwardWinner actorAwardWinner = new()
                        {
                            AwardId = awardId,
                            ActorId = actor.ActorId
                        };

                        await _context.ActorAwardWinners.AddAsync(actorAwardWinner);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Rollback();     //Transaction Failure

                        return new CreateActorResponse
                        {
                            IsSuccess = false,
                            Message = "You entered a non-existent AwardId."
                        };
                    }
                }

                transaction.Commit();       //Transaction Success
                _memoryCache.Cache.Remove("AllActors");


                return new CreateActorResponse
                {
                    IsSuccess = true,
                    Message = "create successful"
                };

            }
            catch (Exception ex)
            {
                transaction.Rollback();     //Transaction Failure

                return new CreateActorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DeleteActorResponse> DeleteActorAsync(int actorId)
        {
            try
            {
                var actor = await _context.Actors.SingleOrDefaultAsync(x => x.ActorId == actorId);

                if (actor is null)
                {
                    return new DeleteActorResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();

                return new DeleteActorResponse
                {
                    IsSuccess = true,
                    Message = "delete successful"
                };
            }
            catch (Exception ex)
            {
                return new DeleteActorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }


        }

        public async Task<UpdateActorResponse> UpdateActorAsync(int actorId, UpdateActorRequest request)
        {
            try
            {
                var actor = await _context.Actors.SingleOrDefaultAsync(x => x.ActorId == actorId);

                if (actor is null)
                {
                    return new UpdateActorResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                actor.FirstName = request.FirstName != actor.FirstName ? request.FirstName : actor.FirstName;
                actor.LastName = request.LastName != actor.LastName ? request.LastName : actor.LastName;
                actor.Biography = request.Biography != actor.Biography ? request.Biography : actor.Biography;

                await _context.SaveChangesAsync();

                return new UpdateActorResponse
                {
                    IsSuccess = true,
                    Message = "update successful"
                };
            }
            catch (Exception ex)
            {
                return new UpdateActorResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }


        }
    }
}
