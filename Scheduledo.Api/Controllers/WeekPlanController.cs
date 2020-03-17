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

        /// <summary>
        /// Used to create a new week plan with related day plans and workout times.
        /// </summary>
        /// <param name="weekPlanInput"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateWeekPlan([FromBody] WeekPlanInput weekPlanInput)
        {
            weekPlanInput.IdCoach = CurrentUser.Id;
            var result = await _weekPlanService.CreateWeekPlan(weekPlanInput);
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
        public async Task<IActionResult> EditWeekPlan([FromBody] WeekPlan weekPlan)
        {
            weekPlan.IdCoach = CurrentUser.Id;
            var result = await _weekPlanService.EditWeekPlan(weekPlan);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing week plan with related day plans and workout times
        /// </summary>
        /// <param name="weekPlan"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteWeekPlan([FromBody] WeekPlan weekPlan)
        {
            weekPlan.IdCoach = CurrentUser.Id;
            var result = await _weekPlanService.DeleteWeekPlan(weekPlan);
            return GetResult(result);
        }

    }
}