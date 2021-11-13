using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.DTO.MovieDTO;
using MovieStore.Service.MovieService;
using System.Threading.Tasks;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] GetMoviesRequest request)
        {
            var movies = await _movieService.GetMoviesAsync(request);
            return Ok(movies);
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieDetail(int id)
        {
            var movieDetail = await _movieService.GetMovieDetailAsync(id);

            if (!string.IsNullOrWhiteSpace(movieDetail.ErrorMessage))
                return BadRequest(movieDetail.ErrorMessage);

            return Ok(movieDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request)
        {
            var createMovieResult = await _movieService.CreateMovieAsync(request);

            if (!createMovieResult.IsSuccess)
                return BadRequest(createMovieResult.Message);
                
            return Ok(createMovieResult.Message);

        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleteMovieResult = await _movieService.DeleteMovieAsync(id);

            if (!deleteMovieResult.IsSuccess)
                return BadRequest(deleteMovieResult.Message);

            return Ok(deleteMovieResult.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieRequest request)
        {
            var updateMovieResult = await _movieService.UpdateMovieAsync(id, request);

            if (!updateMovieResult.IsSuccess)
                return BadRequest(updateMovieResult.Message);

            return Ok(updateMovieResult.Message);
        }
    }
}
