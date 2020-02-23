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
        Task<Result<List<Coach>>> GetAllCoaches();
        Task<Result<Coach>> GetCoach(string id);
        //Task<Result> RegisterCoach(Coach coach);

        Task<Result> UpdateCoach(string Id, UpdateCoachInput coach);
    }
}
