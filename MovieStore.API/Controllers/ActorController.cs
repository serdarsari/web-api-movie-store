using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.DTO.ActorDTO;
using MovieStore.Service.ActorService;
using System.Threading.Tasks;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors([FromQuery] GetActorsRequest request)
        {
            var actors = await _actorService.GetActorsAsync(request);
            return Ok(actors);
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorDetail(int id)
        {
            var actorDetail = await _actorService.GetActorDetailAsync(id);

            if (!string.IsNullOrWhiteSpace(actorDetail.ErrorMessage))
                return BadRequest(actorDetail.ErrorMessage);

            return Ok(actorDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActor([FromBody] CreateActorRequest request)
        {
            var createActorResult = await _actorService.CreateActorAsync(request);

            if (!createActorResult.IsSuccess)
                return BadRequest(createActorResult.Message);

            return Ok(createActorResult.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var deleteActorResult = await _actorService.DeleteActorAsync(id);

            if (!deleteActorResult.IsSuccess)
                return BadRequest(deleteActorResult.Message);

            return Ok(deleteActorResult.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id, UpdateActorRequest request)
        {
            var updateActorResult = await _actorService.UpdateActorAsync(id, request);

            if (!updateActorResult.IsSuccess)
                return BadRequest(updateActorResult.Message);

            return Ok(updateActorResult.Message);
        }
    }
}
