using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Service.Models.Training;
using Scheduledo.Service.Models.TrainingExercise;

namespace Scheduledo.Api.Controllers
{
    [Route("api/training")]
    [ApiController]
    [Authorize]
    public class TrainingController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly ITrainingService _trainingService;
        private readonly ITrainingExerciseService _trainingExerciseService;

        public TrainingController(ITokenService tokenService, ITrainingService trainingService, ITrainingExerciseService trainingExerciseService)
        {
            _tokenService = tokenService;
            _trainingService = trainingService;
            _trainingExerciseService = trainingExerciseService;
        }


        //TODO: Usunąć jak na pewno nie będzie potrzebne
        //[HttpGet("coach")]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(typeof(ICollection<Training>), 200)]
        //public async Task<IActionResult> GetCoachsTrainings()
        //{
        //    var result = await _trainingService.GetCoachsTrainings(CurrentUser.Id);
        //    return GetResult(result);
        //}
        //[HttpGet("client")]
        //[AuthorizePolicy(UserRole.Client)]
        //[ProducesResponseType(typeof(ICollection<Training>), 200)]
        //public async Task<IActionResult> GetClientsTrainings()
        //{
        //    var result = await _trainingService.GetClientsTrainings(CurrentUser.Id);
        //    return GetResult(result);
        //}


        /// <summary>
        /// Used to get a list of trainings for a requesting coach/client
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<Training>), 200)]
        public async Task<IActionResult> GetTrainings()
        {
            var result = await _trainingService.GetTrainings(CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new training for a requesting coach
        /// </summary>
        /// <param name="trainingInput"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> AddTraining([FromBody]TrainingInput trainingInput)
        {
            trainingInput.IdCoach = CurrentUser.Id;
            var result = await _trainingService.AddTraining(trainingInput);
            return GetResult(result);
        }


        /// <summary>
        /// USed to modify an existing training for a requesting coach
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditTraining([FromBody]Training training)
        {
            training.IdCoach = CurrentUser.Id;
            var result = await _trainingService.EditTraining(training);
            return GetResult(result);
        }


        /// <summary>
        /// USed to delete an existing training for a requesting coach
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteTraining([FromBody]Training training)
        {
            training.IdCoach = CurrentUser.Id;
            var result = await _trainingService.DeleteTraining(training);
            return GetResult(result);
        }
    }
}
