using Microsoft.AspNetCore.Mvc;
using Scheduledo.Core.Enums;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Infrastructure.Attributes;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduledo.Api.Controllers
{
	[Route("api/clients")]
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
		/// <param name="Authorization"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Coach)]
		[HttpPost("dummy-register")]
		public async Task<IActionResult> DummyClientRegister([FromHeader] string Authorization, [FromBody]RegisterDummyClientInput model)
		{
			var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
			var result = await _clientService.DummyClientRegister(CoachId.Data, model);
			return GetResult(result);
		}
		/// <summary>
		/// Used by Client to create User account from dummy Client account.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CreateClientAccount([FromBody]RegisterClientInput model)
		{
			var result = await _clientService.CreateClientAccount(model);
			return GetResult(result);
		}

		/// <summary>
		/// Used by Coach to send invitation to Client and by Client to create User account from dummy Client account.
		/// </summary>
		/// <param name="Authorization"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Coach)]
		[HttpPost("send-link")]
		public async Task<IActionResult> SendCreationLinkToClient([FromHeader] string Authorization, [FromBody]CreateClientVerificationTokenInput model)
		{
			var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
			var result = await _clientService.SendCreationLinkToClient(CoachId.Data, model);
			return GetResult(result);
		}

		/// <summary>
		/// Used by Client to create User account by oneself (from invitation from Coach)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("self-create-account")]
		public async Task<IActionResult> SelfCreateClientAccount([FromBody]SelfRegisterClientInput model)
		{
			var result = await _clientService.SelfCreateClientAccount(model);
			return GetResult(result);
		}

		/// <summary>
		/// Used by Coach to send invitation to Client and by Client to create his User account by oneself.
		/// </summary>
		/// <param name="Authorization"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Coach)]
		[HttpPost("self-send-link")]
		public async Task<IActionResult> SendSelfCreationLinkToClient([FromHeader] string Authorization, [FromBody]CreateSelfClientVerificationTokenInput model)
		{
			var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
			var result = await _clientService.SendSelfCreationLinkToClient(CoachId.Data, model);
			return GetResult(result);
		}
		/// <summary>
		/// Used by Client to get information about oneself.
		/// </summary>
		/// <param name="Authorization"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Client)]
		[HttpGet("me")]
		public async Task<IActionResult> GetClient([FromHeader] string Authorization)
		{
			var clientId = await _tokenService.GetRequesterClientId(Authorization);
			var result = await _clientService.GetClient(clientId.Data);
			return GetResult(result);
		}
		/// <summary>
		/// Used by Client to update information about onself.
		/// </summary>
		/// <param name="Authorization"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Client)]
		[HttpPut]
		public async Task<IActionResult> UpdateClient([FromHeader] string Authorization, [FromBody] UpdateClientInput model)
		{
			var clientId = await _tokenService.GetRequesterClientId(Authorization);
			var result = await _clientService.UpdateClient(clientId.Data, model);
			return GetResult(result);
		}
		/// <summary>
		/// Used by Client to get information about its Coach.
		/// </summary>
		/// <param name="Authorization"></param>
		/// <returns></returns>
		[AuthorizePolicy(UserRole.Client)]
		[HttpGet("my-coach")]
		public async Task<IActionResult> GetClientCoach([FromHeader] string Authorization)
		{
			var clientId = await _tokenService.GetRequesterClientId(Authorization);
			var result = await _clientService.GetClientCoach(clientId.Data);
			return GetResult(result);
		}
	}
}
