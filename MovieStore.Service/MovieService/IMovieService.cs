using MovieStore.DTO.MovieDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.MovieService
{
    public interface IMovieService
    {
        Task<GetMoviesResponse> GetMoviesAsync(GetMoviesRequest request);
        Task<GetMovieDetailResponse> GetMovieDetailAsync(int id);
        Task<CreateMovieResponse> CreateMovieAsync(CreateMovieRequest request);
        Task<DeleteMovieResponse> DeleteMovieAsync(int id);
        Task<UpdateMovieResponse> UpdateMovieAsync(int id, UpdateMovieRequest request);
    }
}
