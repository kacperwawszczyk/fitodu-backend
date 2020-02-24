using Microsoft.AspNetCore.Mvc;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface ICoachService
    {
        Task<Result<ICollection<UpdateCoachInput>>> GetAllCoaches();
        Task<Result<ICollection<ClientOutput>>> GetAllClients(string Id);
        Task<Result<UpdateCoachInput>> GetCoach(string id);
        
        Task<Result> UpdateCoach(string Id, UpdateCoachInput coach);
    }
}
