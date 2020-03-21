using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.WeekPlan;

namespace Scheduledo.Api.Controllers
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
        [ProducesResponseType(typeof(ICollection<WeekPlan>), 200)]
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

        /// <summary>
        /// Used to create a new week plan with related day plans and workout times.
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
        /// will remove those objects
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
        /// <param name="weekPlanId"></param>
        /// <returns></returns>
        [HttpDelete("week-plans/{weekPlanId}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteWeekPlan(int weekPlanId)
        {
            var result = await _weekPlanService.DeleteWeekPlan(CurrentUser.Id, weekPlanId);
            return GetResult(result);
        }

    }
}