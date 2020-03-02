using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;

namespace Scheduledo.Api.Controllers
{
    [Route("api/training-results")]
    [ApiController]
    [Authorize]
    public class TrainingResultController : BaseController
    {
        private readonly ITrainingResultService _trainingResultService;
        private readonly ITokenService _tokenService;
        private readonly ITrainingService _trainingService;

        public TrainingResultController(ITrainingResultService trainingResultService, ITokenService tokenService,
            ITrainingService trainingService)
        {
            _trainingResultService = trainingResultService;
            _tokenService = tokenService;
            _trainingService = trainingService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTrainingsResults([FromHeader]string Authorization, int idTraining)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(idTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingResultService.GetTrainingsResults(idTraining);
            return GetResult(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTrainingResult([FromHeader]string Authorization, [FromBody]TrainingResultInput trainingResultInput)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingResultInput.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingResultService.AddTrainingResult(trainingResultInput);
            return GetResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditTrainingResult([FromHeader]string Authorization, [FromBody]TrainingResult trainingResult)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingResult.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingResultService.EditTrainingResult(trainingResult);
            return GetResult(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTrainingResult([FromHeader]string Authorization, [FromBody]TrainingResult trainingResult)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingResult.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingResultService.DeleteTrainingResult(trainingResult);
            return GetResult(result);
        }
    }
}