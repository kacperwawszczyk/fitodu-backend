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

namespace Scheduledo.Api.Controllers
{
    [Route("api/private-notes")]
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
        [HttpGet]
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
        [HttpGet("{clientId}")]
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
        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateNote([FromBody]PrivateNote note)
        {
            note.IdCoach = CurrentUser.Id;
            var result = await _privateNoteService.CreateNote(note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to modify an existing private note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateNote([FromBody]PrivateNote note)
        {
            note.IdCoach = CurrentUser.Id;
            var result = await _privateNoteService.UpdateNote(note);
            return GetResult(result);
        }


        /// <summary>
        /// Used to delete an existing private note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote([FromBody]PrivateNote note)
        {
            var result = await _privateNoteService.DeleteNote(CurrentUser.Id, note.IdClient);
            return GetResult(result);
        }

    }
}