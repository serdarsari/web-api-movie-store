using MovieStore.DTO.AwardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.AwardService
{
    public interface IAwardService
    {
        Task<GetAwardsResponse> GetAwardsAsync(GetAwardsRequest request);
        Task<CreateAwardResponse> CreateAwardAsync(CreateAwardRequest request);
        Task<DeleteAwardResponse> DeleteAwardAsync(int id);
        Task<UpdateAwardResponse> UpdateAwardAsync(int awardId, UpdateAwardRequest request);
    }
}
