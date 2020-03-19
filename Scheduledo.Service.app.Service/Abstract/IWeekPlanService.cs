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
        Task<Result<ICollection<WeekPlanOutput>>> GetWeekPlans(string requesterId, UserRole requesterRole);
        Task<Result> EditWeekPlan(WeekPlan weekPlan);
        Task<Result> CreateWeekPlan(WeekPlanInput weekPlanInput);
        Task<Result> DeleteWeekPlan(WeekPlan weekPlan);

        bool IsValid(WeekPlan weekPlan);

        bool IsValidInput(WeekPlanInput weekPlanInput);
        Task<Result<ICollection<WeekPlanListOutput>>> GetWeekPlansShort(string requesterId, UserRole requesterRole, WeekPlanListInput model);
    }
}
