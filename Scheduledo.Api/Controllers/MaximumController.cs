using System;
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
using Scheduledo.Service.Models.Maximum;

namespace Scheduledo.Api.Controllers
{
    [Route("api/maximums")]
    [ApiController]
    public class MaximumController : BaseController
    {
        private readonly IMaximumService _maximumService;
        private readonly ITokenService _tokenService;

        public MaximumController(IMaximumService maximumService, ITokenService tokenService)
        {
            _maximumService = maximumService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllMaximums([FromHeader]string Authorization, string IdClient)
        {
            var coachId = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachId.Data != null)
            {
                var result = await _maximumService.GetAllMaximums(coachId.Data, IdClient);
                return GetResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{IdExercise}")]
        [Authorize]
        public async Task<IActionResult> GetClientMaximum([FromHeader]string Authorization, string IdClient, int IdExercise)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (coachIdResult.Data != null)
            {
                var result = await _maximumService.GetClientMaximum(coachIdResult.Data, IdClient, IdExercise);
                return GetResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateMaximum([FromHeader]string Authorization, [FromBody]CreateMaximumInput max)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (max == null)
            {
                return BadRequest();
            }

            var result = await _maximumService.CreateMaximum(coachIdResult.Data, max);
            return GetResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateMaximum([FromHeader]string Authorization, [FromBody]Maximum max)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (max == null)
            {
                return BadRequest();
            }

            var result = await _maximumService.UpdateMaximum(coachIdResult.Data, max);
            return GetResult(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMaximum([FromHeader]string Authorization, [FromBody]Maximum max)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (max == null)
            {
                return BadRequest();
            }

            var result = await _maximumService.DeleteMaximum(coachIdResult.Data, max);
            return GetResult(result);
        }

    }
}