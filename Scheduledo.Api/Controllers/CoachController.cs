﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;

namespace Scheduledo.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/coaches")]
    [ApiController]
    //[Authorize]
    public class CoachController : BaseController
    {
        private readonly ICoachService _coachService;
        private readonly ITokenService _tokenService;

        public CoachController(ICoachService coachService, ITokenService tokenService)
        {
            _coachService = coachService;
            _tokenService = tokenService;
        }

        [HttpGet("allCoaches")]
        public async Task<Result<ICollection<UpdateCoachInput>>> GetAllCoaches()
        {
            var result = new Result<ICollection<UpdateCoachInput>>();
            result = await _coachService.GetAllCoaches();
            return result;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCoach([FromHeader] string Authorization)
        {
            var coachId = await _tokenService.GetRequesterCoachId(Authorization);
            if(coachId.Data == null)
            {
                return BadRequest();
            }
            else
            {
                string Id = coachId.Data;
                var result = await _coachService.GetCoach(Id);
                return GetResult(result);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCoach([FromHeader] string Authorization, [FromBody] UpdateCoachInput coach)
        {
            var coachId = await _tokenService.GetRequesterCoachId(Authorization);
            string Id = coachId.Data;
            var result = await _coachService.UpdateCoach(Id, coach);
            return GetResult(result);

        }

        [HttpGet("myClients")]
        [Authorize]
        public async Task<Result<ICollection<UpdateClientInput>>> GetAllClients([FromHeader] string Authorization)
        {
            var coachId = await _tokenService.GetRequesterCoachId(Authorization);
            var result = new Result<ICollection<UpdateClientInput>>();
            string Id = coachId.Data;
            result = await _coachService.GetAllClients(Id);
            return result;
        }


        //[HttpPost]
        //public async Task<IActionResult> RegisterCoach([FromBody] Coach coach)
        //{
        //    if(!String.IsNullOrEmpty(coach.Name))
        //    {
        //        return BadRequest("Empty Name!");
        //    }
        //    if(String.IsNullOrEmpty(coach.Surname))
        //    {
        //        return BadRequest("Empty Surname!");
        //    }
        //    await _coachService.RegisterCoach(coach);

        //    return Ok();
        //}
    }
}