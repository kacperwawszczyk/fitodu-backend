using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitodu.Api.Controllers
{
    [ApiController]
    public class UserFeedbackController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IUserFeedbackService _userFeedbackService;

        public UserFeedbackController(IUserService userService, IUserFeedbackService userFeedbackService)
        {
            _userFeedbackService = userFeedbackService;
            _userService = userService;
        }

        [HttpPost("user-feedback")]
        [Authorize]
        //[ProducesResponseType(typeof())]
        public async Task<IActionResult> AddUserFeedback([FromBody] UserFeedbackInput input)
        {
            var result = await _userFeedbackService.AddUserFeedback(CurrentUser.Id, CurrentUser.Role, input);
            return GetResult(result);
        }
    }
}
