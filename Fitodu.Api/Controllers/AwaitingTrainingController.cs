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
        /// Returns awaiting trainings of a user.
        /// </summary>
        /// <remarks>
        ///  If "sent" is true than returns sent requests. If "received" is true it returns received training request. If both are left empty or something else is type it displays both recieved and sent training requests.
        /// </remarks>
        /// <param name="sent"></param>
        /// <param name="received"></param>
        /// <returns></returns>
        [HttpGet("awaiting-trainings")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<AwaitingTrainingOutput>), 200)]
        public async Task<IActionResult> GetAwaitingTrainings([FromQuery] bool? sent, [FromQuery] bool? received)
        {
            var result = await _awaitingTraningService.GetAwaitingTraining(CurrentUser.Id, CurrentUser.Role, sent, received);
            return GetResult(result);
        }

        /// <summary>
        /// Creates new training request.
        /// </summary>
        /// <remarks>
        /// Creating a requests sends a mail notification to the requests' receiver and lowers client's available trainings value by one.
        /// </remarks>
        /// <param name="awaitingTrainingInput"></param>
        /// <returns></returns>
        [HttpPost("awaiting-trainings")]
        [Authorize]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateAwaitingTraining([FromBody] AwaitingTrainingInput awaitingTrainingInput)
        {
            var result = await _awaitingTraningService.CreateAwaitingTraining(CurrentUser.Id, CurrentUser.Role, awaitingTrainingInput);
            return GetResult(result);
        }

        /// <summary>
        /// Deletes, accepts or rejects awaiting training.
        /// </summary>
        /// <remarks>
        /// Deleting can only be used if someone wants to cancel his request before it gets acctepted or rejected. All actions send a mail notification. Deleting or rejecting an awaiting training increases client's available trainings value by one.
        /// </remarks>
        /// <param name="accept">true = accept, false = reject, null = delete</param>
        /// <param name="id"></param>
        /// <returns> -1 if no training was created, otherwise returns the id of the created training </returns>
        /// <response code="200">Returns -1 if no training was created, otherwise returns the id of the created trainin</response>
        [HttpDelete("awaiting-trainings/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> DeleteAwaitingTraining(int id, [FromQuery] bool? accept)
        {
            var result = await _awaitingTraningService.DeleteAwaitingTraining(CurrentUser.Id, CurrentUser.Role, id, accept);
            return GetResult(result);
        }
    }
}