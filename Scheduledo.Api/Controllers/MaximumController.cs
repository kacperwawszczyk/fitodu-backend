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
    [Route("api")]
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

        /// <summary>
        /// Used by Coach to get a list of all Maximums of all Exercises of selected Client
        /// </summary>
        /// <param name="IdClient"> string type </param>
        /// <returns> Returns ICollection of Maximum </returns>
        [HttpGet("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<Maximum>), 200)]
        public async Task<IActionResult> GetAllMaximums(string IdClient)
        {
            var result = await _maximumService.GetAllMaximums(CurrentUser.Id, IdClient);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to get a Maximum with given IdClient and IdExercise
        /// </summary>
        /// <param name="IdClient"> string type </param>
        /// <param name="IdExercise"> string type </param>
        /// <returns> Returns Maximum </returns>
        [HttpGet("maximums/{IdExercise}")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(Maximum), 200)]
        public async Task<IActionResult> GetClientMaximum(string IdClient, int IdExercise)
        {
            var result = await _maximumService.GetClientMaximum(CurrentUser.Id, IdClient, IdExercise);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to create a new Maximum
        /// </summary>
        /// <param name="max"> CreateMaximumInput type </param>
        /// <returns></returns>
        [HttpPost("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateMaximum([FromBody]CreateMaximumInput max)
        {
            var result = await _maximumService.CreateMaximum(CurrentUser.Id, max);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to modify an existing Maximum
        /// </summary>
        /// <param name="max"> Maximum type </param>
        /// <returns></returns>
        [HttpPut("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateMaximum([FromBody]Maximum max)
        {
            var result = await _maximumService.UpdateMaximum(CurrentUser.Id, max);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to delete an existing Maximum
        /// </summary>
        /// <param name="max"> Maximum type </param>
        /// <returns></returns>
        [HttpDelete("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteMaximum([FromBody]Maximum max)
        {
            var result = await _maximumService.DeleteMaximum(CurrentUser.Id, max);
            return GetResult(result);
        }

    }
}