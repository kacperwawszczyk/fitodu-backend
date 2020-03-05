using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
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


        /// <summary>
        /// Used to get a list of all (archived and not-archvied) exercises of a requesting coach 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<Exercise>), 500)]
        public async Task<IActionResult> GetAllExercises() //all exercises of a coach
        { 
            var result = await _exerciseService.GetAllExercises(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a list of archived exercises of a requesting coach 
        /// </summary>
        /// <returns></returns>
        [HttpGet("archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<Exercise>), 500)]
        public async Task<IActionResult> GetArchivedExercises() //all archived exercises of a coach
        {
            var result = await _exerciseService.GetArchivedExercises(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a list of not-archived exercises of a requesting coach 
        /// </summary>
        /// <returns></returns>
        [HttpGet("not-archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<Exercise>), 500)]
        public async Task<IActionResult> GetNotArchivedExercises() //all  not archived exercises of a coach
        {
            var result = await _exerciseService.GetNotArchivedExercises(CurrentUser.Id);
            return GetResult(result);
        }


        /// <summary>
        /// Used to create a new exercise for a requesting coach (if it doesn't already exist)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateExercise([FromBody]ExerciseInput exercise)
        {
            exercise.IdCoach = CurrentUser.Id;
            var result = await _exerciseService.CreateExercise(exercise);
            return GetResult(result);
        }


        /// <summary>
        /// Used to modify an existing exercise for a requesting coach
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditExercise([FromBody]Exercise exercise)
        {
            exercise.IdCoach = CurrentUser.Id;
            var result = await _exerciseService.EditExercise(exercise);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing exercise for a requesting coach
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteExercise([FromBody]Exercise exercise)
        {
            exercise.IdCoach = CurrentUser.Id;
            var result = await _exerciseService.DeleteExercise(exercise);
            return GetResult(result);
        }
    }
}