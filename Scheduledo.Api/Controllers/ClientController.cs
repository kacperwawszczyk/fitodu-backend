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

		[AuthorizePolicy(UserRole.Coach)]
		[HttpPost("clients/DummyClientRegister")]
		public async Task<IActionResult> DummyClientRegister([FromHeader] string Authorization, [FromBody]RegisterDummyClientInput model)
		{
			var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
			var result = await _clientService.DummyClientRegister(CoachId.Data, model);
			return GetResult(result);
		}

		[HttpPost("clients/CreateClientAccount")]
		public async Task<IActionResult> CreateClientAccount([FromBody]RegisterClientInput model)
		{
			var result = await _clientService.CreateClientAccount(model);
			return GetResult(result);
		}

		[AuthorizePolicy(UserRole.Coach)]
		[HttpPost("clients/SendCreationLinkToClient")]
		public async Task<IActionResult> SendCreationLinkToClient([FromHeader] string Authorization, [FromBody]CreateClientVerificationTokenInput model)
		{
			var CoachId = await _tokenService.GetRequesterCoachId(Authorization);
			var result = await _clientService.SendCreationLinkToClient(CoachId.Data, model);
			return GetResult(result);
		}
	}
}
