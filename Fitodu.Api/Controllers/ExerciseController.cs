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
        /// Returns an exercise with given id.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
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
        /// Returns a list of all (archived and not-archvied) exercises.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <returns></returns>
        [HttpGet("exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetAllExercises([FromQuery] string name) //all exercises of a coach
        {
            var result = await _exerciseService.GetAllExercises(CurrentUser.Id, name);
            return GetResult(result);
        }

        /// <summary>
        /// Returns a list of archived exercises.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <returns></returns>
        [HttpGet("exercises/archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetArchivedExercises([FromQuery] string name) //all archived exercises of a coach
        {
            var result = await _exerciseService.GetArchivedExercises(CurrentUser.Id, name);
            return GetResult(result);
        }

        /// <summary>
        /// Returns a list of not-archived exercises.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <returns></returns>
        [HttpGet("exercises/not-archived")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ExerciseOutput>), 200)]
        public async Task<IActionResult> GetNotArchivedExercises([FromQuery] string name) //all  not archived exercises of a coach
        {
            var result = await _exerciseService.GetNotArchivedExercises(CurrentUser.Id, name);
            return GetResult(result);
        }


        /// <summary>
        /// Creates a new exercise for a requesting coach.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="exercise"></param>
        /// <returns></returns>
        /// <response code="200"> Returns id of newly created exercise. </response>
        [HttpPost("exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateExercise([FromBody]ExerciseInput exercise)
        {
            var result = await _exerciseService.CreateExercise(CurrentUser.Id, exercise);
            return GetResult(result);
        }


        /// <summary>
        /// Modifies an existing exercise.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
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
        /// Deletes an existing exerciseh.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
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