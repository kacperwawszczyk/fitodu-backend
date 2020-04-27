using Microsoft.AspNetCore.Mvc;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fitodu.Service.Models.Client;
using Fitodu.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Fitodu.Service.Abstract
{
	public interface IClientService
	{
		Task<Result<string>> UpdateAvatar(string id, UserRole role, IFormFile file);
		Task<Result<string>> DummyClientRegister(string CoachId, RegisterDummyClientInput model);
		Task<Result> DummyClientUpdate(string CoachId, UserRole role, string ClientId, DummyClientUpdateInput model);
		Task<Result> CreateClientAccount(RegisterClientInput model);
		Task<Result> SelfCreateClientAccount(SelfRegisterClientInput model);
		Task<Result> SendCreationLinkToClient(string CoachId, CreateClientVerificationTokenInput model);
		Task<Result> SendSelfCreationLinkToClient(string CoachId, CreateSelfClientVerificationTokenInput model);
		Task<Result<ClientOutput>> GetClient(string Id);
		Task<Result<long>> UpdateClient(string Id, UpdateClientInput model);
		Task<Result<CoachOutput>> GetClientCoach(string Id);
		Task<Result<ClientOutput>> GetClientById(string coachId, string clientId);
        Task<Result<ClientNotes>> GetClientNotes(string requesterId, UserRole role, string clientId);
        Task<Result> DummyClientDelete(string requesterId, UserRole requesterRole, string clientId);
    }
}
