using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Service.Models;

namespace Fitodu.Api.Controllers
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
        /// Returns a list of all maximums of all exercises of selected client.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="IdClient"> string type </param>
        /// <returns> Returns ICollection of MaximumOutput </returns>
        [HttpGet("maximums/client")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<MaximumOutput>), 200)]
        public async Task<IActionResult> GetAllMaximums(string IdClient)
        {
            var result = await _maximumService.GetAllMaximums(CurrentUser.Id, IdClient);
            return GetResult(result);
        }

        /// <summary>
        /// Returns a maximum of selected client and exercise.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="IdClient"> string type </param>
        /// <param name="IdExercise"> int type </param>
        /// <returns> Returns MaximumOutput </returns>
        [HttpGet("maximums/client-exercise")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(MaximumOutput), 200)]
        public async Task<IActionResult> GetClientMaximum(string IdClient, int IdExercise)
        {
            var result = await _maximumService.GetClientMaximum(CurrentUser.Id, IdClient, IdExercise);
            return GetResult(result);
        }

        /// <summary>
        /// Creates a new maximum.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="max"> MaximumInput type </param>
        /// <returns></returns>
        [HttpPost("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateMaximum([FromBody]MaximumInput max)
        {
            var result = await _maximumService.CreateMaximum(CurrentUser.Id, max);
            return GetResult(result);
        }

        /// <summary>
        /// Modifies an existing maximum.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="max"> MaximumInput type </param>
        /// <returns></returns>
        [HttpPut("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateMaximum([FromBody]MaximumInput max)
        {
            var result = await _maximumService.UpdateMaximum(CurrentUser.Id, max);
            return GetResult(result);
        }

        /// <summary>
        /// Deletes an existing maximum.
        /// </summary>
        /// <remarks> Used by coach. </remarks>
        /// <param name="IdClient"> string type </param>
        /// <param name="IdExercise"> int type </param>
        /// <returns></returns>
        [HttpDelete("maximums")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteMaximum(string IdClient, int IdExercise)
        {
            var result = await _maximumService.DeleteMaximum(CurrentUser.Id, IdClient, IdExercise);
            return GetResult(result);
        }

    }
}