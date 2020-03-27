using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Service.Models;
using Fitodu.Service.Models.WeekPlan;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class WeekPlanController : BaseController
    {
        private readonly IWeekPlanService _weekPlanService;

        public WeekPlanController(IWeekPlanService weekPlanService)
        {
            _weekPlanService = weekPlanService;
        }


        /// <summary>
        /// Used to get all week plans of a coach. If requester is a coach, endpoint  will return all week plans of this coach, if requester is a client
        /// endpoint will return all week plans of his coach instead.
        /// </summary>
        /// <returns></returns>
        [HttpGet("week-plans")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<WeekPlanOutput>), 200)]
        public async Task<IActionResult> GetWeekPlans()
        {
            var result = await _weekPlanService.GetWeekPlans(CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }

        // TODO: Testowa metoda (do wykorzystania lub usunięcia)
        /// <summary>
        /// Week plans paging test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("week-plans-short")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<WeekPlanListOutput>), 200)]
        public async Task<IActionResult> GetWeekPlansShort([FromQuery] WeekPlanListInput model)
        {
            var result = await _weekPlanService.GetWeekPlansShort(CurrentUser.Id, CurrentUser.Role, model);
            return GetResult(result);
        }

        // TODO: Testowa metoda (do wykorzystania lub usunięcia)
        /// <summary>
        /// Creates a weekPlan object based on all the workouts coach has planned for a specified week. TEST METHOD
        /// </summary>
        /// <param name="id">weekPlanId</param>
        /// <returns></returns>
        [HttpGet("week-plans/{id}/booked-hours")]
        [Authorize]
        [ProducesResponseType(typeof(WeekPlanOutput), 200)]
        public async Task<IActionResult> GetWeekPlansBookedHours(int id)
        {
            var result = await _weekPlanService.GetWeekPlansBookedHours(CurrentUser.Id, CurrentUser.Role, id);
            return GetResult(result);
        }


        /// <summary>
        /// Used to create a new week plan with related day plans and workout times. There can not be two different week plans starting at the same day. All week plans have to start on Monday.
        /// </summary>
        /// <param name="weekPlanInput"></param>
        /// <returns></returns>
        [HttpPost("week-plans")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateWeekPlan([FromBody] WeekPlanInput weekPlanInput)
        {
            var result = await _weekPlanService.CreateWeekPlan(CurrentUser.Id, weekPlanInput);
            return GetResult(result);
        }

        /// <summary>
        /// Used to modify and existing week plan for a requesting coach (sending a week plan with an empty day plan collection removes 
        /// related day plans; sending a week plan with a day plan colletion containing objects that do not exist in the database
        /// will create those objects; sending a week plan with a day plan colletion not containing objects that exist in the database
        /// will remove those objects. There can not be two different week plans starting at the same day. All week plans have to start on Monday.
        /// </summary>
        /// <param name="weekPlan"></param>
        /// <returns></returns>
        [HttpPut("week-plans")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> EditWeekPlan([FromBody]UpdateWeekPlanInput weekPlan)
        {
            var result = await _weekPlanService.EditWeekPlan(CurrentUser.Id, weekPlan);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing week plan with related day plans and workout times
        /// </summary>
        /// <param name="id">Id of the weekplan you wish to delete</param>
        /// <returns></returns>
        [HttpDelete("week-plans/{id}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteWeekPlan(int id)
        {
            var result = await _weekPlanService.DeleteWeekPlan(CurrentUser.Id, id);
            return GetResult(result);
        }

    }
}