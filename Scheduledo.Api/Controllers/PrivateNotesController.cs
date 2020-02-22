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

namespace Scheduledo.Api.Controllers
{
    [Route("api/privateNotes")]
    [ApiController]
    //[Authorize]
    public class PrivateNotesController : BaseController
    {
        private readonly IPrivateNoteService _privateNoteService;
        private readonly ITokenService _tokenService;

        public PrivateNotesController(IPrivateNoteService privateNoteService, ITokenService tokenService)
        {
            _privateNoteService = privateNoteService;
            _tokenService = tokenService;
        }


        //all private notes of a coach
        [HttpGet("allNotes")]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(typeof(ICollection<PrivateNote>), 200)]
        public async Task<IActionResult> GetAllNotes([FromHeader]string Authorization)
        {

            var coachId = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachId.Data != null)
            {
                string Id = coachId.Data;
                var result = await _privateNoteService.GetAllNotes(Id);
                return GetResult(result);

            }
            else
            {
                return BadRequest();
            }
        }

        //all private notes of a coach's client
        [HttpGet("allNotes/{coachId}/{clientId}")]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(PrivateNote), 200)]
        public async Task<IActionResult> GetUsersNote(string coachId, string clientId)
        {
            var result = await _privateNoteService.GetClientsNote(coachId, clientId);
            return GetResult(result);
        }

        [HttpPost]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNote([FromBody]PrivateNote note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            var result = await _privateNoteService.CreateNote(note);

            return GetResult(result);
        }


        [HttpPut]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNote([FromBody]PrivateNote note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            var result = await _privateNoteService.UpdateNote(note);

            return GetResult(result);
        }

        [HttpDelete("deleteNote/{coachId}/{clientId}")]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote(string coachId, string clientId)
        {

            var result = await _privateNoteService.DeleteNote(coachId, clientId);

            return GetResult(result);
        }

    }
}