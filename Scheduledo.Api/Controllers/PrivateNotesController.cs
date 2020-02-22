﻿using System;
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

namespace Scheduledo.Api.Controllers
{
    [Route("api/privateNotes")]
    [ApiController]
    //[Authorize]
    public class PrivateNotesController : BaseController
    {
        private readonly IPrivateNoteService _privateNoteService;

        public PrivateNotesController(IPrivateNoteService privateNoteService)
        {
            _privateNoteService = privateNoteService;
        }


        //all private notes of a coach
        [HttpGet("allNotes/{id}")]
        [Authorize]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(typeof(ICollection<PrivateNote>), 200)]
        public async Task<IActionResult> GetAllNotes(string coachId)
        {

            var result = await _privateNoteService.GetAllNotes(coachId);
            return GetResult(result);
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