using Microsoft.AspNetCore.Mvc;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fitodu.Core.Enums;

namespace Fitodu.Service.Abstract
{
    public interface ICoachService
    {
        Task<Result<ICollection<CoachOutput>>> GetAllCoaches();
        Task<Result<ICollection<ClientOutput>>> GetAllClients(string Id);
        Task<Result<CoachOutput>> GetCoach(string id);
        Task<Result> CoachRegister(RegisterCoachInput model);
        Task<Result<long>> UpdateCoach(string Id, UpdateCoachInput coach);
        Task<Result> SetClientsTrainingsAvailable(string requesterId, UserRole role, string clientId, int value);
    }
}
