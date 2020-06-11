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
using Fitodu.Service.Models;

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
        [ProducesResponseType(typeof(TrainingOutput), 200)]
        public async Task<IActionResult> GetTraining(string id)
        {
            var result = await _trainingService.GetTraining(CurrentUser.Id, CurrentUser.Role, int.Parse(id));
            return GetResult(result);
        }

        /// <summary>
        /// Returns training with a StartDate greater than given.
        /// </summary>
        /// <param name="from">StartDate</param>
        /// <param name="idClient">if not specified method will return trainings with all coach's clients</param>
        /// <returns>List of trainings</returns>
        [HttpGet("trainings")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<TrainingListOutput>), 200)]
        public async Task<IActionResult> GetFutureTrainings([FromQuery]string from, [FromQuery] string idClient)
        {
            var result = await _trainingService.GetTrainings(CurrentUser.Id, CurrentUser.Role, from, idClient);
            return GetResult(result);
        }

        /// <summary>
        /// Creates a new training for a requesting coach. 
        /// </summary>
        /// <remarks>
        /// Can only be used to create a training with a client that does not have an account. Automatically sets seconds property of StartDate and EndDate to 0.
        /// </remarks>
        /// <param name="trainingInput"></param>
        /// <returns>ID of created training</returns>
        [HttpPost("trainings")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddTraining([FromBody]TrainingInput trainingInput)
        {
            var result = await _trainingService.AddTraining(CurrentUser.Id, trainingInput);
            return GetResult(result);
        }


        /// <summary>
        /// Modifies an existing training for a requesting coach. 
        /// </summary>
        /// <remarks> Automatically sets seconds property of StartDate and EndDate to 0.
        /// </remarks>
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
        /// Deletes an existing training for a requesting coach. 
        /// </summary>
        /// <remarks>
        /// Also deletes all TrainingExercises related to that training.
        /// </remarks>
        /// <param name="id">Id of the training you wish to delete</param>
        /// <param name="time_zone_offset"> Timezone offset (in minutes) </param>
        /// <returns></returns>
        [HttpDelete("trainings/{id}/{time_zone_offset}")]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteTraining(int id, int time_zone_offset)
        {
            var result = await _trainingService.DeleteTraining(CurrentUser.Id, CurrentUser.Role, id, time_zone_offset);
            return GetResult(result);
        }
    }
}
