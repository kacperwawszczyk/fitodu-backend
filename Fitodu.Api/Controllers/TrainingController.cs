using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Service.Models.Training;
using Fitodu.Service.Models.TrainingExercise;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
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
        // <summary>
        // Used to get a list of trainings for a requesting coach/client
        // </summary>
        // <returns></returns>
        //[HttpGet("trainings")]
        //[Authorize]
        //[ProducesResponseType(typeof(ICollection<Training>), 200)]
        //public async Task<IActionResult> GetTrainings()
        //{
        //    var result = await _trainingService.GetTrainings(CurrentUser.Id, CurrentUser.Role);
        //    return GetResult(result);
        //}

        /// <summary>
        /// Used to get a single training (with given id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("trainings/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Training), 200)]
        public async Task<IActionResult> GetTraining(string id)
        {
            var result = await _trainingService.GetTraining(CurrentUser.Id, CurrentUser.Role, int.Parse(id));
            return GetResult(result);
        }

        /// <summary>
        /// Used to get training with a StartDate greater than given.
        /// </summary>
        /// <param name="from">StartDate</param>
        /// <param name="idClient">if not specified method will return trainings with all coach's clients</param>
        /// <returns></returns>
        [HttpGet("trainings")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<Training>), 200)]
        public async Task<IActionResult> GetFutureTrainings([FromQuery]string from, [FromQuery] string idClient)
        {
            var result = await _trainingService.GetTrainings(CurrentUser.Id, CurrentUser.Role, from, idClient);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new training for a requesting coach (can only be used to create a training with a client that does not have an account). Automatically sets seconds property of StartDate and EndDate to 0.
        /// </summary>
        /// <param name="trainingInput"></param>
        /// <returns></returns>
        [HttpPost("trainings")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddTraining([FromBody]TrainingInput trainingInput)
        {
            var result = await _trainingService.AddTraining(CurrentUser.Id, trainingInput);
            return GetResult(result);
        }


        /// <summary>
        /// Used to modify an existing training for a requesting coach.  Automatically sets seconds property of StartDate and EndDate to 0.
        /// </summary>
        /// <param name="editTrainingInput"></param>
        /// <returns></returns>
        [HttpPut("trainings")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditTraining([FromBody]UpdateTrainingInput editTrainingInput)
        {
            var result = await _trainingService.EditTraining(CurrentUser.Id, editTrainingInput);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing training for a requesting coach. Also deletes all TrainingExercises related to that training.
        /// </summary>
        /// <param name="id">Id of the training you wish to delete</param>
        /// <returns></returns>
        [HttpDelete("trainings")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            var result = await _trainingService.DeleteTraining(CurrentUser.Id, id);
            return GetResult(result);
        }
    }
}
