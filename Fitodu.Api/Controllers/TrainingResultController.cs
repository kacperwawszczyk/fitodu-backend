using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class TrainingResultController : BaseController
    {
        private readonly ITrainingResultService _trainingResultService;
        private readonly ITokenService _tokenService;
        

        public TrainingResultController(ITrainingResultService trainingResultService, ITokenService tokenService)
        {
            _trainingResultService = trainingResultService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Used to get a list of exercises' results for a given training.
        /// </summary>
        /// <param name="idTraining"></param>
        /// <returns></returns>
        [HttpGet("training-results")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<TrainingResult>), 200)]
        public async Task<IActionResult> GetTrainingsResults(int idTraining)
        {
            var result = await _trainingResultService.GetTrainingsResults(idTraining, CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }


        /// <summary>
        /// Used to create a new training result.
        /// </summary>
        /// <param name="trainingResultInput"></param>
        /// <returns></returns>
        [HttpPost("training-results")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> AddTrainingResult([FromBody]TrainingResultInput trainingResultInput)
        {
            var result = await _trainingResultService.AddTrainingResult(CurrentUser.Id, trainingResultInput);
            return GetResult(result);
        }


        /// <summary>
        /// Used to modify an existing training result.
        /// </summary>
        /// <param name="trainingResult"></param>
        /// <returns></returns>
        [HttpPut("training-results")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditTrainingResult([FromBody]TrainingResult trainingResult)
        {
            var result = await _trainingResultService.EditTrainingResult(CurrentUser.Id, trainingResult);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing training result.
        /// </summary>
        /// <param name="trainingResultId"></param>
        /// <returns></returns>
        [HttpDelete("training-results")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteTrainingResult(int trainingResultId)
        {
            var result = await _trainingResultService.DeleteTrainingResult(CurrentUser.Id, trainingResultId);
            return GetResult(result);
        }
    }
}