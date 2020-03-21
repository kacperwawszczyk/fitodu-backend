using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scheduledo.Service.Concrete;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Core.Enums;
using Scheduledo.Service.Models.PrivateNote;

namespace Scheduledo.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class PrivateNoteController : BaseController
    {
        private readonly IPrivateNoteService _privateNoteService;
        private readonly ITokenService _tokenService;

        public PrivateNoteController(IPrivateNoteService privateNoteService, ITokenService tokenService)
        {
            _privateNoteService = privateNoteService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Used to get a list of all private notes of a requsting coach
        /// </summary>
        /// <returns></returns>
        [HttpGet("private-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<PrivateNote>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var result = await _privateNoteService.GetAllNotes(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a single private notes of a requsting coach
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("private-notes/{clientId}")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(PrivateNote), 200)]
        public async Task<IActionResult> GetUsersNote(string clientId)
        {
            var result = await _privateNoteService.GetClientsNote(CurrentUser.Id, clientId);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new private note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost("private-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateNote([FromBody]PrivateNoteInput note)
        {
            var result = await _privateNoteService.CreateNote(CurrentUser.Id, note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to modify an existing private note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("private-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateNote([FromBody]PrivateNoteInput note)
        {
            var result = await _privateNoteService.UpdateNote(CurrentUser.Id, note);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing private note
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpDelete("private-notes/{clientId}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote(string clientId)
        {
            var result = await _privateNoteService.DeleteNote(CurrentUser.Id, clientId);
            return GetResult(result);
        }

    }
}