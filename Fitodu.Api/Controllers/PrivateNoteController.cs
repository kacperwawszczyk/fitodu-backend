using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fitodu.Service.Concrete;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Core.Enums;
using Fitodu.Service.Models.PrivateNote;

namespace Fitodu.Api.Controllers
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
        /// Used to get a list of all private notes of a requsting coach.
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
        /// Used to get a single private note of a requsting coach.
        /// </summary>
        /// <param name="client">Id of the client you wish to get the private note of</param>
        /// <returns></returns>
        [HttpGet("private-notes/{client}")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(PrivateNote), 200)]
        public async Task<IActionResult> GetUsersNote(string client)
        {
            var result = await _privateNoteService.GetClientsNote(CurrentUser.Id, client);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new private note. There can only be one private note for a single coach's client.
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
        /// Used to modify an existing private note.
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
        /// Used to delete an existing private note.
        /// </summary>
        /// <param name="client">Id of the client you wish to delete the private note of</param>
        /// <returns></returns>
        [HttpDelete("private-notes/{client}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote(string client)
        {
            var result = await _privateNoteService.DeleteNote(CurrentUser.Id, client);
            return GetResult(result);
        }

    }
}