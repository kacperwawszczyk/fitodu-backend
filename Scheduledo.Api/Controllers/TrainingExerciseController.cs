﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Service.Models.TrainingExercise;

namespace Scheduledo.Api.Controllers
{
    [Route("api")]
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

        /// <summary>
        /// Used to get a list of exercises in the given training
        /// </summary>
        /// <param name="idTraining"></param>
        /// <returns></returns>
        [HttpGet("training-exercises")] 
        [Authorize]
        [ProducesResponseType(typeof(ICollection<TrainingExercise>), 200)]
        public async Task<IActionResult> GetTrainingsExercises(int idTraining)
        {
            var result = await _trainingExerciseService.GetTrainingsExercises(idTraining, CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }


        /// <summary>
        ///  Used to add a new exercise to the training
        /// </summary>
        /// <param name="trainingExerciseInput"></param>
        /// <returns></returns>
        [HttpPost("training-exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> AddTrainingExercise([FromBody]TrainingExerciseInput trainingExerciseInput)
        {
            var result = await _trainingExerciseService.AddTrainingExercise(CurrentUser.Id, trainingExerciseInput);
            return GetResult(result);
        }


        /// <summary>
        /// Used to modify an exisitng exercise in the training
        /// </summary>
        /// <param name="trainingExercise"></param>
        /// <returns></returns>
        [HttpPut("training-exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditTrainingExercise([FromBody]TrainingExercise trainingExercise)
        {

            var result = await _trainingExerciseService.EditTrainingExercise(CurrentUser.Id, trainingExercise);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an exercise from the training
        /// </summary>
        /// <param name="trainingExerciseId"></param>
        /// <returns></returns>
        [HttpDelete("training-exercises")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteTrainingExercise(int trainingExerciseId)
        {
            var result = await _trainingExerciseService.DeleteTrainingExercise(CurrentUser.Id, trainingExerciseId);
            return GetResult(result);
        }

    }
}