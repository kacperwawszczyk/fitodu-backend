using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IWeekPlanService
    {
        Task<Result<ICollection<WeekPlan>>> GetWeekPlans(string coachId, string requesterId, UserRole requesterRole);
    }
}
