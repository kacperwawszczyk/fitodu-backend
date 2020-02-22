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
    public class PrivateNoteController : BaseController
    {
        private readonly IPrivateNoteService _privateNoteService;
        private readonly ITokenService _tokenService;

        public PrivateNoteController(IPrivateNoteService privateNoteService, ITokenService tokenService)
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

            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult.Data != null)
            {
                string coachId = coachIdResult.Data;
                var result = await _privateNoteService.GetAllNotes(coachId);
                return GetResult(result);

            }
            else
            {
                return BadRequest();
            }
        }

        //all private notes of a coach's client
        [HttpGet("allNotes/{clientId}")]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(PrivateNote), 200)]
        public async Task<IActionResult> GetUsersNote([FromHeader]string Authorization, string clientId)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (coachIdResult.Data != null)
            {
                string coachId = coachIdResult.Data;
                var result = await _privateNoteService.GetClientsNote(coachId, clientId);
                return GetResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNote([FromHeader]string Authorization, [FromBody]PrivateNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach!=coachIdResult.Data)
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
        public async Task<IActionResult> UpdateNote([FromHeader]string Authorization, [FromBody]PrivateNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            var result = await _privateNoteService.UpdateNote(note);
            return GetResult(result);
        }

        [HttpDelete]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote([FromHeader]string Authorization, [FromBody]PrivateNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            string coachId = coachIdResult.Data;
            var result = await _privateNoteService.DeleteNote(coachId, note.IdClient);
            return GetResult(result);
        }

    }
}