using MovieStore.DTO.ActorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.ActorService
{
    public interface IActorService
    {
        Task<GetActorsResponse> GetActorsAsync(GetActorsRequest request);
        Task<GetActorDetailResponse> GetActorDetailAsync(int id);
        Task<CreateActorResponse> CreateActorAsync(CreateActorRequest request);
        Task<DeleteActorResponse> DeleteActorAsync(int actorId);
        Task<UpdateActorResponse> UpdateActorAsync(int actorId, UpdateActorRequest request);
    }
}
