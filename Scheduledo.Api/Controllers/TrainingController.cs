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


        [HttpGet("coachs-trainings")]
        [Authorize]
        public async Task<IActionResult> GetCoachsTrainings([FromHeader]string Authorization, string idCoach)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (coachIdResult == null || coachIdResult.Data != idCoach)
            {
                return BadRequest();
            }

            var result = await _trainingService.GetCoachsTrainings(idCoach);
            return GetResult(result);
        }


        [HttpGet("clients-trainings")] 
        [Authorize]
        public async Task<IActionResult> GetClientsTrainings([FromHeader]string Authorization, string idClient)
        {
            var coachIdResult = await _tokenService.GetRequesterClientId(Authorization);
            if (coachIdResult == null || coachIdResult.Data != idClient)
            {
                return BadRequest();
            }

            var result = await _trainingService.GetClientsTrainings(idClient);
            return GetResult(result);
        }
    }
}
