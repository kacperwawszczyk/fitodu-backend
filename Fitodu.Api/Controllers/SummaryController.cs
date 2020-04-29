using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class SummaryController : BaseController
    {
        private readonly ISummaryService _summaryService;
        private readonly ITokenService _tokenService;

        public SummaryController(ISummaryService summaryService, ITokenService tokenService)
        {
            _summaryService = summaryService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Used by coach to get a list of all summaries of selected client
        /// </summary>
        /// <param name="IdClient"> string type </param>
        /// <returns> Returns ICollection of SummaryOutput </returns>
        [HttpGet("summaries/client")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<SummaryOutput>), 200)]
        public async Task<IActionResult> GetAllSummaries(string IdClient)
        {
            var result = await _summaryService.GetAllSummaries(CurrentUser.Id, CurrentUser.Role, IdClient);
            return GetResult(result);
        }

        /// <summary>
        /// Used by coach to get concrete summary of selected client
        /// </summary>
        /// <param name="IdClient"> string type </param>
        /// <param name="Id"> int type of summary entity key </param>
        /// <returns> Returns SummaryOutput </returns>
        [HttpGet("summaries/client-summary")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(SummaryOutput), 200)]
        public async Task<IActionResult> GetClientSummary(string IdClient, int Id)
        {
            var result = await _summaryService.GetClientSummary(CurrentUser.Id, IdClient, Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used by coach to create a new summary
        /// </summary>
        /// <param name="sum"> SummaryInput type </param>
        /// <returns> returns summary id </returns>
        [HttpPost("summaries")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateSummary([FromBody]SummaryInput sum)
        {
            var result = await _summaryService.CreateSummary(CurrentUser.Id, sum);
            return GetResult(result);
        }

        /// <summary>
        /// Used by coach to modify an existing summary
        /// </summary>
        /// <param name="sum"> UpdateSummaryInput type </param>
        /// <returns></returns>
        [HttpPut("summaries")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateSummary([FromBody]UpdateSummaryInput sum)
        {
            var result = await _summaryService.UpdateSummary(CurrentUser.Id, sum);
            return GetResult(result);
        }

        /// <summary>
        /// Used by coach to delete an existing summary
        /// </summary>
        /// <param name="Id"> int type of summary key </param>
        /// <returns></returns>
        [HttpDelete("summaries")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteSummary(int Id)
        {
            var result = await _summaryService.DeleteSummary(CurrentUser.Id, Id);
            return GetResult(result);
        }
    }
}