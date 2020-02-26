using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models.TrainingExercise;

namespace Scheduledo.Api.Controllers
{
    [Route("api/TrainingExercises")]
    [ApiController]
    [Authorize]

    public class TrainingExerciseController : BaseController
    {
        private readonly ITrainingExerciseService _trainingExerciseService;
        private readonly ITokenService _tokenService;
        private readonly ITrainingService _trainingService;
        public TrainingExerciseController(ITrainingExerciseService trainingExerciseService, ITokenService tokenService,
            ITrainingService trainingService)
        {
            _trainingExerciseService = trainingExerciseService;
            _tokenService = tokenService;
            _trainingService = trainingService;
        }


        [HttpGet("allTrainingsExercises")] //all exercises in a training
        [Authorize]
        public async Task<IActionResult> GetTrainingsExercises([FromHeader]string Authorization, int idTraining)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(idTraining);

            //check if this training is related to requesting coach
            if(coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingExerciseService.GetTrainingsExercises(idTraining);
            return GetResult(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTrainingExercise([FromHeader]string Authorization, [FromBody]TrainingExerciseInput trainingExerciseInput)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExerciseInput.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingExerciseService.AddTrainingExercise(trainingExerciseInput);
            return GetResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditTrainingExercise([FromHeader]string Authorization, [FromBody]TrainingExercise trainingExercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExercise.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingExerciseService.EditTrainingExercise(trainingExercise);
            return GetResult(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTrainingExercise([FromHeader]string Authorization, [FromBody]TrainingExercise trainingExercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExercise.IdTraining);

            //check if this training is related to requesting coach
            if (coachIdResult == null || trainingsCoachResult == null || coachIdResult.Data != trainingsCoachResult.Data)
            {
                return BadRequest();
            }

            var result = await _trainingExerciseService.DeleteTrainingExercise(trainingExercise);
            return GetResult(result);
        }

    }
}