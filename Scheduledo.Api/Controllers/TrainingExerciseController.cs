using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Service.Abstract;

namespace Scheduledo.Api.Controllers
{
    [Route("api/TrainingExercises")]
    [ApiController]
    [Authorize]

    public class TrainingExerciseController : BaseController
    {
        private readonly ITrainingExerciseService _trainingExerciseService;
        private readonly ITokenService _tokenService;
        public TrainingExerciseController(ITrainingExerciseService trainingExerciseService, ITokenService tokenService)
        {
            _trainingExerciseService = trainingExerciseService;
            _tokenService = tokenService;
        }


        [HttpGet("allTrainingsExercises")] //all exercises in a training
        [Authorize]
        public async Task<IActionResult> GetTrainingsExercises([FromHeader]string Authorization, int idTraining)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            throw new NotImplementedException();
        }



    }
}