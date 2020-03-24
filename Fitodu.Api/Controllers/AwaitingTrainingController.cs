using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models.AwaitingTraining;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class AwaitingTrainingController : BaseController
    {
        private readonly IAwaitingTrainingService _awaitingTraningService;

        public AwaitingTrainingController(IAwaitingTrainingService awaitingTraningService)
        {
            _awaitingTraningService = awaitingTraningService;
        }

        /// <summary>
        /// Used to get all awaiting trainings of a requestiong coach or a client
        /// </summary>
        /// <returns></returns>
        [HttpGet("awaiting-trainings")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<AwaitingTrainingOutput>), 200)]
        public async Task<IActionResult> GetAwaitingTrainings()
        {
            var result = await _awaitingTraningService.GetAwaitingTraining(CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }

        [HttpPost("awaiting-trainings")]
        [Authorize]
        public async Task<IActionResult> CreateAwaitingTraining([FromBody] AwaitingTrainingInput awaitingTrainingInput)
        {
            var result = await _awaitingTraningService.CreateAwaitingTraining(CurrentUser.Id, CurrentUser.Role, awaitingTrainingInput);
            return GetResult(result);
        }

    }
}