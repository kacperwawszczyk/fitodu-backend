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
using Fitodu.Service.Models.Exercise;
using Fitodu.Service.Models;

namespace Fitodu.Api.Controllers
{

    [Route("api")]
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
    /// Used by coach to get exercise by id.
    /// </summary>
    /// <param name="id">Id of the exercise</param>
    /// <returns></returns>
        [HttpGet("exercises/{id}")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetExerciseById(int id) 
        { 
            var result = await _exerciseService.GetExerciseById(CurrentUser.Id, id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a list of all (archived and not-archvied) exercises of a requesting coach .
        /// </summary>
        /// <returns></returns>
        [HttpGet("exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetAllExercises() //all exercises of a coach
        {
            var result = await _exerciseService.GetAllExercises(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a list of archived exercises of a requesting coach.
        /// </summary>
        /// <returns></returns>
        [HttpGet("exercises/archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetArchivedExercises() //all archived exercises of a coach
        {
            var result = await _exerciseService.GetArchivedExercises(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a list of not-archived exercises of a requesting coach.
        /// </summary>
        /// <returns></returns>
        [HttpGet("exercises/not-archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetNotArchivedExercises() //all  not archived exercises of a coach
        {
            var result = await _exerciseService.GetNotArchivedExercises(CurrentUser.Id);
            return GetResult(result);
        }


        /// <summary>
        /// Used to create a new exercise for a requesting coach (if an exercise with given name doesn't already exist).
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        [HttpPost("exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateExercise([FromBody]ExerciseInput exercise)
        {
            var result = await _exerciseService.CreateExercise(CurrentUser.Id, exercise);
            return GetResult(result);
        }


        /// <summary>
        /// Used to modify an existing exercise for a requesting coach.
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns></returns>
        [HttpPut("exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditExercise([FromBody]UpdateExerciseInput exercise)
        {
            var result = await _exerciseService.EditExercise(CurrentUser.Id, exercise);
            return GetResult(result);
        }

        /// <summary>
        /// Used to delete an existing exercise for a requesting coach.
        /// </summary>
        /// <param name="id">Id of the exercise you wish to delete</param>
        /// <returns></returns>
        [HttpDelete("exercises/{id}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var result = await _exerciseService.DeleteExercise(CurrentUser.Id, id);
            return GetResult(result);
        }
    }
}