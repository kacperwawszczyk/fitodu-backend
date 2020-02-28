using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models.Training;
using Scheduledo.Service.Models.TrainingExercise;

namespace Scheduledo.Api.Controllers
{
    [Route("api/Training")]
    [ApiController]
    [Authorize]
    public class TrainingController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly ITrainingService _trainingService;

        public TrainingController(ITokenService tokenService, ITrainingService trainingService)
        {
            _tokenService = tokenService;
            _trainingService = trainingService;
        }


        [HttpGet("coach")]
        [Authorize]//coach only
        public async Task<IActionResult> GetCoachsTrainings([FromHeader]string Authorization)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult == null)
            {
                return BadRequest();
            }

            string idCoach= coachIdResult.Data;

            var result = await _trainingService.GetCoachsTrainings(idCoach);
            return GetResult(result);
        }


        [HttpGet("client")]
        [Authorize]//client only
        public async Task<IActionResult> GetClientsTrainings([FromHeader]string Authorization)
        {
            var coachIdResult = await _tokenService.GetRequesterClientId(Authorization);
            if (coachIdResult == null)
            {
                return BadRequest();
            }
            string idClient = coachIdResult.Data;

            var result = await _trainingService.GetClientsTrainings(idClient);
            return GetResult(result);
        }

        [HttpPost]
        [Authorize]//coach only
        public async Task<IActionResult> AddTraining([FromHeader]string Authorization, [FromBody]TrainingInput trainingInput)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult == null || coachIdResult.Data != trainingInput.IdCoach)
            {
                return BadRequest();
            }

            var result = await _trainingService.AddTraining(trainingInput);
            return GetResult(result);
        }

        [HttpPut]
        [Authorize]//coach only
        public async Task<IActionResult> EditTraining([FromHeader]string Authorization, [FromBody]Training training)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult == null || coachIdResult.Data != training.IdCoach)
            {
                return BadRequest();
            }

            var result = await _trainingService.EditTraining(training);
            return GetResult(result);
        }

        [HttpDelete]
        [Authorize] //coach only
        public async Task<IActionResult> DeleteTraining([FromHeader]string Authorization, [FromBody]Training training)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult == null || coachIdResult.Data != training.IdCoach)
            {
                return BadRequest();
            }

            var result = await _trainingService.DeleteTraining(training);
            return GetResult(result);
        }
    }
}
