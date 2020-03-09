﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Service.Models;

namespace Scheduledo.Api.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize]
    public class CoachController : BaseController
    {
        private readonly ICoachService _coachService;

        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
        }
        // TODO: Uzupełnić
        [HttpPost("coaches")]
        public async Task<IActionResult> CoachRegister([FromBody]RegisterCoachInput model)
        {
            var result = await _coachService.CoachRegister(model);
            return GetResult(result);
        }

        // TODO: Wykomentować jak będzie niepotrzebne
        /// <summary>
        /// Used by anyone to get list of all coaches
        /// </summary>
        /// <returns> Returns ICollection of CoachOutput containing collecion of information about Coaches </returns>
        [HttpGet("coaches")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<CoachOutput>), 200)]
        public async Task<IActionResult> GetAllCoaches()
        {
            var result = await _coachService.GetAllCoaches();
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to get information about oneself
        /// </summary>
        /// <returns> Returns CoachOutput containing information about Coach </returns>
        [HttpGet("coaches/me")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(CoachOutput), 200)]
        public async Task<IActionResult> GetCoach()
        {
            var result = await _coachService.GetCoach(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to update information about oneself
        /// </summary>
        /// <param name="coach"></param>
        /// <returns> Returns long </returns>
        [HttpPut("coaches/me")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<IActionResult> UpdateCoach([FromBody] UpdateCoachInput coach)
        {
            var result = await _coachService.UpdateCoach(CurrentUser.Id, coach);
            return GetResult(result);

        }

        /// <summary>
        /// Used by Coach to get list of Clients
        /// </summary>
        /// <returns> Returns ICollection of ClientOutput containing information about Clients </returns>
        [HttpGet("coaches/clients")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<ClientOutput>), 200)]
        public async Task<IActionResult> GetAllClients()
        {
            var result = await _coachService.GetAllClients(CurrentUser.Id);
            return GetResult(result);
        }
    }
}