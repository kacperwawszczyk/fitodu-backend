using Microsoft.AspNetCore.Mvc;
using Fitodu.Core.Enums;
using Fitodu.Service.Abstract;
using Fitodu.Service.Infrastructure.Attributes;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitodu.Service.Models.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Fitodu.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        public ClientController(ITokenService tokenService, IUserService userService, IClientService clientService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _clientService = clientService;
        }

        /// <summary>
        /// Used by Coach to create dummy Client account.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Id of dummy client</returns>
        [AuthorizePolicy(UserRole.Coach)]
        [HttpPost("clients/dummy-register")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DummyClientRegister([FromBody]RegisterDummyClientInput model)
        {
            //var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
            var result = await _clientService.DummyClientRegister(CurrentUser.Id, model);
            return GetResult(result);
        }
        /// <summary>
        /// Used to delete dummy client by coach.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Coach)]
        [HttpDelete("clients/dummy-clients/{id}")]
        public async Task<IActionResult> DummyClientDelete(string id)
        {
            var result = await _clientService.DummyClientDelete(CurrentUser.Id, CurrentUser.Role, id);
            return GetResult(result);
        }
        /// <summary>
        /// Used by coach to update dummy client information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Coach)]
        [HttpPut("clients/dummy-clients/{id}")]
        public async Task<IActionResult> DummyClientUpdate(string id, DummyClientUpdateInput model)
        {
            var result = await _clientService.DummyClientUpdate(CurrentUser.Id, CurrentUser.Role, id, model);
            return GetResult(result);
        }
        /// <summary>
        /// Used by Client to create User account from dummy Client account.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("clients")]
        public async Task<IActionResult> CreateClientAccount([FromBody]RegisterClientInput model)
        {
            var result = await _clientService.CreateClientAccount(model);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to send invitation to Client (to create user account).
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Coach)]
        [HttpPost("clients/send-link")]
        public async Task<IActionResult> SendCreationLinkToClient([FromBody]CreateClientVerificationTokenInput model)
        {
            var result = await _clientService.SendCreationLinkToClient(CurrentUser.Id, model);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Client to create User account by oneself (from invitation from Coach)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("clients/self-create-account")]
        public async Task<IActionResult> SelfCreateClientAccount([FromBody]SelfRegisterClientInput model)
        {
            var result = await _clientService.SelfCreateClientAccount(model);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Coach to send invitation to Client and by Client to create his User account by oneself.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Coach)]
        [HttpPost("clients/self-send-link")]
        public async Task<IActionResult> SendSelfCreationLinkToClient([FromBody]CreateSelfClientVerificationTokenInput model)
        {
            var result = await _clientService.SendSelfCreationLinkToClient(CurrentUser.Id, model);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Client to get information about oneself.
        /// </summary>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Client)]
        [HttpGet("clients/me")]
        [ProducesResponseType(typeof(ClientOutput), 200)]
        public async Task<IActionResult> GetClient()
        {
            var result = await _clientService.GetClient(CurrentUser.Id);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Client to update information about onself.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Client)]
        [HttpPut("clients")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientInput model)
        {
            var result = await _clientService.UpdateClient(CurrentUser.Id, model);
            return GetResult(result);
        }

        /// <summary>
        /// Used by Client to get information about its Coach.
        /// </summary>
        /// <returns></returns>
        [AuthorizePolicy(UserRole.Client)]
        [HttpGet("clients/coach")]
        [ProducesResponseType(typeof(CoachOutput), 200)]
        public async Task<IActionResult> GetClientCoach()
        {
            var result = await _clientService.GetClientCoach(CurrentUser.Id);
            return GetResult(result);
        }

        [AuthorizePolicy(UserRole.Coach)]
        [HttpGet("clients/{id}")]
        [ProducesResponseType(typeof(ClientOutput), 200)]
        public async Task<IActionResult> GetClientById(string id)
        {
            var result = await _clientService.GetClientById(CurrentUser.Id, id);
            return GetResult(result);
        }

        /// <summary>
        /// Used to get both private and public note of a client (can by used by a coach to return notes of one of his clients, or by a client to return his own notes)
        /// </summary>
        /// <param name="id">Id of the client</param>
        /// <returns></returns>
        [HttpGet("clients/{id}/notes")]
        [Authorize]
        [ProducesResponseType(typeof(ClientNotes), 200)]
        public async Task<IActionResult> GetClientNotes(string id)
        {
            var result = await _clientService.GetClientNotes(CurrentUser.Id, CurrentUser.Role, id);
            return GetResult(result);
        }

        //[HttpPut("clients/update-avatar")]
        //[AuthorizePolicy(UserRole.Client)]
        //public async Task<IActionResult> UpdateAvatar(IFormFile file)
        //{
        //    var result = await _clientService.UpdateAvatar(CurrentUser.Id, CurrentUser.Role, file);
        //    return GetResult(result);
        //}
    }
}
