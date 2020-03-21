using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.WeekPlan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IWeekPlanService
    {
        Task<Result<ICollection<WeekPlanOutput>>> GetWeekPlans(string requesterId, UserRole requesterRole);
        Task<Result> EditWeekPlan(string coachId, UpdateWeekPlanInput weekPlan);
        Task<Result> CreateWeekPlan(string coachId, WeekPlanInput weekPlanInput);
        Task<Result> DeleteWeekPlan(string coachId, int weekPlanId);

        bool IsValid(WeekPlan weekPlan);

        bool IsValidInput(WeekPlanInput weekPlanInput);

        bool IsValidEditInput(UpdateWeekPlanInput editWeekPlanInput);
        Task<Result<ICollection<WeekPlanListOutput>>> GetWeekPlansShort(string requesterId, UserRole requesterRole, WeekPlanListInput model);
    }
}
