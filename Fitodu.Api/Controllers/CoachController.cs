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

        /// <summary>
        /// Used by a coach to set a new value for client's available trainings.
        /// </summary>
        /// <param name="id">Id of the client you wish to change the value for</param>
        /// <param name="value">new value that will be set, it has be greater than or equal to 0</param>
        /// <returns></returns>
        [HttpPut("coaches/clients/{id}/trainings-available/{value}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> SetClientsTrainingsAvailable(string id, int value)
        {
            var result = await _coachService.SetClientsTrainingsAvailable(CurrentUser.Id, CurrentUser.Role, id, value);
            return GetResult(result);
        }

        //[HttpPut("coaches/update-avatar")]
        //[AuthorizePolicy(UserRole.Client)]
        //public async Task<IActionResult> UpdateAvatar(IFormFile file)
        //{
        //    var result = await _coachService.UpdateAvatar(CurrentUser.Id, CurrentUser.Role, file);
        //    return GetResult(result);
        //}
    }
}