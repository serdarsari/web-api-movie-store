using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.DTO.AwardDTO;
using MovieStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.AwardService
{
    public class AwardService : IAwardService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public AwardService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAwardsResponse> GetAwardsAsync(GetAwardsRequest request)
        {
            var currentStartRow = (request.PageNumber - 1) * request.PageSize;
            var response = new GetAwardsResponse
            {
                NextPage = $"api/Awards?PageNumber={request.PageNumber + 1}&PageSize={request.PageSize}",
                TotalAwards = await _context.Awards.CountAsync(),
            };

            var awards = await _context.Awards
                .Skip(currentStartRow)
                .Take(request.PageSize)
                .Select(m => new AwardResponse
                {
                    Name = m.Name,
                    Country = m.Country
                })
                .ToListAsync();
            response.Awards = awards;

            return response;
        }
    
        public async Task<CreateAwardResponse> CreateAwardAsync(CreateAwardRequest request)
        {
            try
            {
                var result = await _context.Awards.SingleOrDefaultAsync(x => x.Name == request.Name && x.Country == request.Country);
                if (result is not null)
                {
                    return new CreateAwardResponse
                    {
                        IsSuccess = false,
                        Message = "This award already exists."
                    };
                }

                Award award = _mapper.Map<Award>(request);

                await _context.Awards.AddAsync(award);
                await _context.SaveChangesAsync();

                return new CreateAwardResponse
                {
                    IsSuccess = true,
                    Message = "create successful"
                };
            }
            catch (Exception ex)
            {
                return new CreateAwardResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            
        }
    
        public async Task<DeleteAwardResponse> DeleteAwardAsync(int id)
        {
            try
            {
                var award = await _context.Awards.SingleOrDefaultAsync(x => x.AwardId == id);

                if (award is null)
                {
                    return new DeleteAwardResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                _context.Awards.Remove(award);
                await _context.SaveChangesAsync();

                return new DeleteAwardResponse
                {
                    IsSuccess = true,
                    Message = "delete successful"
                };
            }
            catch (Exception ex)
            {
                return new DeleteAwardResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            
        }
   
        public async Task<UpdateAwardResponse> UpdateAwardAsync(int awardId, UpdateAwardRequest request)
        {
            try
            {
                var award = await _context.Awards.SingleOrDefaultAsync(x => x.AwardId == awardId);
                if (award is null)
                {
                    return new UpdateAwardResponse
                    {
                        IsSuccess = false,
                        Message = "You have entered an invalid Id."
                    };
                }

                award.Name = request.Name != default ? request.Name : award.Name;
                award.Country = request.Country != default ? request.Country : award.Country;

                await _context.SaveChangesAsync();

                return new UpdateAwardResponse
                {
                    IsSuccess = true,
                    Message = "update successful"
                };
            }
            catch (Exception ex)
            {
                return new UpdateAwardResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            
        }
    }
}
