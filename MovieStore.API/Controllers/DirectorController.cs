using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.DTO.DirectorDTO;
using MovieStore.Service.DirectorService;
using System.Threading.Tasks;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors([FromQuery] GetDirectorsRequest request)
        {
            var directors = await _directorService.GetDirectorsAsync(request);
            return Ok(directors);
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorDetail(int id)
        {
            var directorDetail = await _directorService.GetDirectorDetailAsync(id);

            if (!string.IsNullOrWhiteSpace(directorDetail.ErrorMessage))
                return BadRequest(directorDetail.ErrorMessage);

            return Ok(directorDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector([FromBody] CreateDirectorRequest request)
        {
            var createDirectorResult = await _directorService.CreateDirectorAsync(request);

            if (!createDirectorResult.IsSuccess)
                return BadRequest(createDirectorResult.Message);

            return Ok(createDirectorResult.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var deleteDirectorResult = await _directorService.DeleteDirectorAsync(id);

            if (!deleteDirectorResult.IsSuccess)
                return BadRequest(deleteDirectorResult.Message);

            return Ok(deleteDirectorResult.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDirector(int id, UpdateDirectorRequest request)
        {
            var updateDirectorResult = await _directorService.UpdateDirectorAsync(id, request);

            if (!updateDirectorResult.IsSuccess)
                return BadRequest(updateDirectorResult.Message);

            return Ok(updateDirectorResult.Message);
        }
    }
}
