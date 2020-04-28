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
        /// Used to display awaiting trainings. If type equals "sent" it displays sent training requests, else if type equals "received" it displays received training request, if left empty or something else is type it displays both recieved and sent training requests.
        /// </summary>
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
        /// Used to create a new workout request by a coach or a client. 
        /// Creating a requests sends a mail notification to the requests' receiver and lowers client's available trainings value by one.
        /// </summary>
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
        /// Used to delete, accept or reject awaiting training.
        /// Deleting can only be used if someone wants to withdrew his workout requests before it gets acctepted or rejected. All actions send a mail notification.
        /// Deleting or rejecting an awaiting training increases client's available trainings value by one.
        /// </summary>
        /// <param name="accept">bool - true = accept, false = reject, null = delete</param>
        /// <param name="id">Id of the awaiting training you wish to delete/accept/reject</param>
        /// <returns> -1 if no training was created, otherwise returns the id of the created trainin</returns>
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