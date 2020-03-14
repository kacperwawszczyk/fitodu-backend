using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;

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
        /// Used to get all week plans of a coach with given id
        /// </summary>
        /// <returns></returns>
        [HttpGet("week-plans/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<WeekPlan>), 200)]
        public async Task<IActionResult> GetWeekPlans(string id)
        {
            var result = await _weekPlanService.GetWeekPlans(id, CurrentUser.Id, CurrentUser.Role);
            return GetResult(result);
        }

    }
}