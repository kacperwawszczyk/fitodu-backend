﻿using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fitodu.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        //[HttpGet("users")]
        //[AuthorizePolicy(UserRole.User)]
        //[ProducesResponseType(typeof(ICollection<UserListItemOutput>), 200)]
        //public async Task<IActionResult> GetList()
        //{
        //    var result = await _userService.GetList(CurrentUser.Id);
        //    return GetResult(result);
        //}

        //[HttpGet("users/{id}")]
        //[AuthorizePolicy(UserRole.CompanyAdmin)]
        //[ProducesResponseType(typeof(UserOutput), 200)]
        //public async Task<IActionResult> Get(string id)
        //{
        //    var result = await _userService.Get(id);
        //    return GetResult(result);
        //}

        //[HttpGet("users/me")]
        //[AuthorizePolicy(UserRole.User)]
        //[ProducesResponseType(typeof(UserOutput), 200)]
        //public async Task<IActionResult> GetMe()
        //{
        //    var id = CurrentUser.Id;
        //    var result = await _userService.Get(id);
        //    return GetResult(result);
        //}

        //[HttpGet("users/company")]
        //[AuthorizePolicy(UserRole.User)]
        //[ProducesResponseType(typeof(CompanyOutput), 200)]
        //public async Task<IActionResult> GetCompany()
        //{
        //    var id = CurrentUser.Id;
        //    var result = await _userService.GetCompany(id);
        //    return GetResult(result);
        //}

        

        //[HttpPost("users")]
        //[AuthorizePolicy(UserRole.CompanyAdmin)]
        //[ProducesResponseType(typeof(string), 200)]
        //public async Task<IActionResult> Create([FromBody]CreateUserInput model)
        //{
        //    model.AdminId = CurrentUser.Id;
        //    var result = await _userService.Create(model);
        //    return GetResult(result);
        //}

        //[HttpPut("users")]
        //[AuthorizePolicy(UserRole.CompanyAdmin)]
        //[ProducesResponseType(typeof(string), 200)]
        //public async Task<IActionResult> Update([FromBody]UpdateUserInput model)
        //{
        //    model.AdminId = CurrentUser.Id;
        //    var result = await _userService.Update(model);
        //    return GetResult(result);
        //}

        //[HttpPut("users/me")]
        //[ProducesResponseType(typeof(string), 200)]
        //public async Task<IActionResult> UpdateMe([FromBody]UpdateUserInput model)
        //{
        //    model.Id = CurrentUser.Id;
        //    var result = await _userService.UpdateMe(model);
        //    return GetResult(result);
        //}

        //[HttpPut("users/company")]
        //[AuthorizePolicy(UserRole.User)]
        //[ProducesResponseType(typeof(long), 200)]
        //public async Task<IActionResult> UpdateCompany([FromBody]UpdateCompanyInput model)
        //{
        //    model.UserId = CurrentUser.Id;
        //    var result = await _userService.UpdateCompany(model);
        //    return GetResult(result);
        //}

        //[HttpDelete("users/{id}")]
        //[AuthorizePolicy(UserRole.CompanyAdmin)]
        //[ProducesResponseType(typeof(string), 200)]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var result = await _userService.Delete(CurrentUser.Id, id);
        //    return GetResult(result);
        //}

        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenOutput), 200)]
        public async Task<IActionResult> CreateToken([FromBody]CreateTokenInput model)
        {
            var result = await _userService.CreateToken(model);
            return GetResult(result);
        }

        //[HttpPost("users/token/super")]
        //[AuthorizePolicy(UserRole.SuperAdmin)]
        //[ProducesResponseType(typeof(TokenOutput), 200)]
        //public async Task<IActionResult> CreateSuperToken([FromBody]CreateSuperTokenInput model)
        //{
        //    var result = await _userService.CreateToken(
        //        new CreateTokenInput { Email = model.Email }, false);

        //    return GetResult(result);
        //}

        [HttpPost("token/refresh")]
        [ProducesResponseType(typeof(TokenOutput), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInput model)
        {
            var result = await _userService.RefreshToken(model);
            return GetResult(result);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody]SignOutInput model)
        {
            var result = await _userService.SignOut(model);
            return GetResult(result);
        }

        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordInput model)
        {
            var result = await _userService.ForgotPassword(model.Email);
            return GetResult(result);
        }

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordInput model)
        {
            var result = await _userService.ResetPassword(model);
            return GetResult(result);
        }
    }
}