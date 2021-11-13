using MovieStore.DTO.DirectorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.DirectorService
{
    public interface IDirectorService
    {
        Task<GetDirectorsResponse> GetDirectorsAsync(GetDirectorsRequest request);
        Task<GetDirectorDetailResponse> GetDirectorDetailAsync(int id);
        Task<CreateDirectorResponse> CreateDirectorAsync(CreateDirectorRequest request);
        Task<DeleteDirectorResponse> DeleteDirectorAsync(int directorId);
        Task<UpdateDirectorResponse> UpdateDirectorAsync(int directorId, UpdateDirectorRequest request);

    }
}
