using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.DTO.AwardDTO;
using MovieStore.Service.AwardService;
using System.Threading.Tasks;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class AwardController : ControllerBase
    {
        private readonly IAwardService _awardService;

        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAwards([FromQuery] GetAwardsRequest request)
        {
            var awards = await _awardService.GetAwardsAsync(request);

            return Ok(awards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAward([FromBody] CreateAwardRequest request)
        {
            var createAwardResult = await _awardService.CreateAwardAsync(request);
            
            if (!createAwardResult.IsSuccess)
                return BadRequest(createAwardResult.Message);

            return Ok(createAwardResult.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAward(int id)
        {
            var deleteAwardResult = await _awardService.DeleteAwardAsync(id);

            if (!deleteAwardResult.IsSuccess)
                return BadRequest(deleteAwardResult.Message);

            return Ok(deleteAwardResult.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAward(int id, UpdateAwardRequest request)
        {
            var updateAwardResult = await _awardService.UpdateAwardAsync(id, request);

            if (!updateAwardResult.IsSuccess)
                return BadRequest(updateAwardResult.Message);

            return Ok(updateAwardResult.Message);
        }
    }
}
