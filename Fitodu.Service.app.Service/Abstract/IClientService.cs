using Microsoft.AspNetCore.Mvc;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fitodu.Service.Models.Client;
using Fitodu.Core.Enums;

namespace Fitodu.Service.Abstract
{
	public interface IClientService
	{
		Task<Result> DummyClientRegister(string CoachId, RegisterDummyClientInput model);
		Task<Result> CreateClientAccount(RegisterClientInput model);
		Task<Result> SelfCreateClientAccount(SelfRegisterClientInput model);
		Task<Result> SendCreationLinkToClient(string CoachId, CreateClientVerificationTokenInput model);
		Task<Result> SendSelfCreationLinkToClient(string CoachId, CreateSelfClientVerificationTokenInput model);
		Task<Result<ClientOutput>> GetClient(string Id);
		Task<Result<long>> UpdateClient(string Id, UpdateClientInput model);
		Task<Result<CoachOutput>> GetClientCoach(string Id);
		Task<Result<ClientOutput>> GetClientById(string coachId, string clientId);
        Task<Result<ClientNotes>> GetClientNotes(string requesterId, UserRole role, string clientId);
    }
}
