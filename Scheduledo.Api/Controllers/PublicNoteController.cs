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

namespace Scheduledo.Api.Controllers
{
    [Route("api/public-notes")]
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
        /// Used to get a list of all public notes of a requsting coach
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizePolicy(UserRole.Coach)]
        [ProducesResponseType(typeof(ICollection<PublicNote>), 200)]
        public async Task<IActionResult> GetAllNotes()
        {
            var result = await _publicNoteService.GetAllNotes(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get a single public note of a client with given Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("{clientId}")]
        [Authorize]
        [ProducesResponseType(typeof(PublicNote), 200)]
        public async Task<IActionResult> GetUsersNote(string clientId)
        {
            var result = await _publicNoteService.GetClientsNote(clientId, CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to create a new public note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> CreateNote([FromBody]PublicNote note)
        {
            note.IdCoach = CurrentUser.Id;
            var result = await _publicNoteService.CreateNote(note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to modify an existing public note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> UpdateNote([FromBody]PublicNote note)
        {
            note.IdCoach = CurrentUser.Id;
            var result = await _publicNoteService.UpdateNote(note);
            return GetResult(result);
        }

        /// <summary>
        /// Used to delete an existing public note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote([FromBody]PublicNote note)
        {
            var result = await _publicNoteService.DeleteNote(CurrentUser.Id, note.IdClient);
            return GetResult(result);
        }

    }
}