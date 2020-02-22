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
    [Route("api/publicNotes")]
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


        //all public notes of a coach
        [HttpGet("allNotes")]
        [Authorize]
        //[ProducesResponseType(typeof(ICollection<PublicNote>), 200)]
        public async Task<IActionResult> GetAllNotes([FromHeader]string Authorization)
        {

            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult.Data != null)
            {
                string coachId = coachIdResult.Data;
                var result = await _publicNoteService.GetAllNotes(coachId);
                return GetResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        //all public notes of a coach's client
        [HttpGet("allNotes/{clientId}")]
        [Authorize]
        //[ProducesResponseType(PublicNote), 200)]
        public async Task<IActionResult> GetUsersNote([FromHeader]string Authorization, string clientId)
        {

            bool authorized = false;
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);

            if (coachIdResult.Data != null)
            {
                authorized = true;
            }
            else
            {
                var clientIdResult = await _tokenService.GetRequesterClientId(Authorization);
                if (clientIdResult.Data != null)
                {
                    authorized = true;
                }
            }

            if(authorized)
            {
                var result = await _publicNoteService.GetClientsNote(clientId);
                return GetResult(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNote([FromHeader]string Authorization, [FromBody]PublicNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            var result = await _publicNoteService.CreateNote(note);
            return GetResult(result);
        }


        [HttpPut]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNote([FromHeader]string Authorization, [FromBody]PublicNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            var result = await _publicNoteService.UpdateNote(note);
            return GetResult(result);
        }

        [HttpDelete]
        //[AuthorizePolicy(UserRole.Coach)]
        public async Task<IActionResult> DeleteNote([FromHeader]string Authorization, [FromBody]PublicNote note)
        {
            var coachIdResult = await _tokenService.GetRequesterCoachId(Authorization);
            if (note == null || note.IdCoach != coachIdResult.Data)
            {
                return BadRequest();
            }
            string coachId = coachIdResult.Data;
            var result = await _publicNoteService.DeleteNote(coachId, note.IdClient);
            return GetResult(result);
        }

    }
}