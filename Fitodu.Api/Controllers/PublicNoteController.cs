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
using Fitodu.Service.Models.PublicNote;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class PublicNoteController : BaseController
    {
        private readonly IPublicNoteService _publicNoteService;
        private readonly ITokenService _tokenService;

        public PublicNoteController(IPublicNoteService publicNoteService, ITokenService tokenService)
        {
            _publicNoteService = publicNoteService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Used to get a list of all public notes of a requsting coach.
        /// </summary>
        /// <returns></returns>
        [HttpGet("public-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<PublicNote>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var result = await _publicNoteService.GetAllNotes(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a single public note of a client with given Id.
        /// </summary>
        /// <param name="client">Id of the client you wish to get the public note of</param>
        /// <returns></returns>
        [HttpGet("public-notes/{client}")]
        [Authorize]
        [ProducesResponseType(typeof(PublicNote), 200)]
        public async Task<IActionResult> GetUsersNote(string client)
        {
            var result = await _publicNoteService.GetClientsNote(client, CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new public note. There can only be one public note for a single coach's client.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost("public-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateNote([FromBody]PublicNoteInput note)
        {
            var result = await _publicNoteService.CreateNote(CurrentUser.Id, note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to modify an existing public note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("public-notes")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateNote([FromBody]PublicNoteInput note)
        {
            var result = await _publicNoteService.UpdateNote(CurrentUser.Id, note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to delete an existing public note.
        /// </summary>
        /// <param name="client">Id of the client you wish to delete the public note of</param>
        /// <returns></returns>
        [HttpDelete("public-notes/{client}")]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote(string client)
        {
            var result = await _publicNoteService.DeleteNote(CurrentUser.Id, client);
            return GetResult(result);
        }

    }
}