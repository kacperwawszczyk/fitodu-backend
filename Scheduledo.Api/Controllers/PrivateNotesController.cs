using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;

namespace Scheduledo.Api.Controllers
{
    [Route("api/privateNotes")]
    [ApiController]
    [Authorize]
    public class PrivateNotesController : Controller
    {
        private readonly IPrivateNotesService _privateNotesService;

        public PrivateNotesController(IPrivateNotesService privateNotesService)
        {
            _privateNotesService = privateNotesService;
        }

        [HttpGet("allNotes/{id}")]
        //[AuthorizePolicy(UserRole.Coach)]
        //[ProducesResponseType(typeof(ICollection<PrivateNote>), 200)]
        public async Task<List<PrivateNote>> GetAllNotes(string id)
        {

            var result = await _privateNotesService.GetAllNotes(id);
            return result;
        }

    }
}