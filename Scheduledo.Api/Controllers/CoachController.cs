using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;

namespace Scheduledo.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/coaches")]
    [ApiController]
    //[Authorize]
    public class CoachController : BaseController
    {
        private readonly ICoachService _coachService;

        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        [HttpGet("all")]
        public async Task<Result<List<Coach>>> GetAllCoaches()
        {
            var result = await _coachService.GetAllCoaches();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<Result<Coach>> GetCoach(string id)
        {
            var result = await _coachService.GetCoach(id);
            return result;
        }

        //[HttpPut("modify")]
        //public async Task<Result> ModifyCoach([FromBody] Coach coach)
        //{
        //    var updatedCoach = 
        //    var result = await _coachService.ModifyCoach();
        //}
        

        //[HttpPost]
        //public async Task<IActionResult> RegisterCoach([FromBody] Coach coach)
        //{
        //    if(!String.IsNullOrEmpty(coach.Name))
        //    {
        //        return BadRequest("Empty Name!");
        //    }
        //    if(String.IsNullOrEmpty(coach.Surname))
        //    {
        //        return BadRequest("Empty Surname!");
        //    }
        //    await _coachService.RegisterCoach(coach);
            
        //    return Ok();
        //}
    }
}