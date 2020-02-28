using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models.Exercise;

namespace Scheduledo.Api.Controllers
{

    [Route("api/exercises")]
    [ApiController]
    [Authorize]
    public class ExerciseController : BaseController
    {
        private readonly IExerciseService _exerciseService;
        private readonly ITokenService _tokenService;

        public ExerciseController(IExerciseService exerciseService, ITokenService tokenService)
        {
            _exerciseService = exerciseService;
            _tokenService = tokenService;
        }


        [HttpGet]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> GetAllExercises([FromHeader]string Authorization) //all exercises of a coach
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult.Data != null)
            {
                string coachId = coachIdResult.Data;
                var result = await _exerciseService.GetAllExercises(coachId);
                return GetResult(result);

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateExercise([FromHeader]string Authorization, [FromBody]ExerciseInput exercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if(exercise == null || exercise.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }

            var result = await _exerciseService.CreateExercise(exercise);
            return GetResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditExercise([FromHeader]string Authorization, [FromBody]Exercise exercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (exercise == null || exercise.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }

            var result = await _exerciseService.EditExercise(exercise);
            return GetResult(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteExercise([FromHeader]string Authorization, [FromBody]Exercise exercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (exercise == null || exercise.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            var result = await _exerciseService.DeleteExercise(exercise);
            return GetResult(result);
        }
    }
}