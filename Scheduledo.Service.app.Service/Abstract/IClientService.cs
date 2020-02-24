using Microsoft.AspNetCore.Mvc;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
	public interface IClientService
	{
		Task<Result> DummyClientRegister(string CoachId, RegisterDummyClientInput model);
		Task<Result> CreateClientAccount(RegisterClientInput model);
		Task<Result> SelfCreateClientAccount(SelfRegisterClientInput model);
		Task<Result> SendCreationLinkToClient(string CoachId, CreateClientVerificationTokenInput model);
		Task<Result> SendSelfCreationLinkToClient(string CoachId, CreateSelfClientVerificationTokenInput model);
	}
}
