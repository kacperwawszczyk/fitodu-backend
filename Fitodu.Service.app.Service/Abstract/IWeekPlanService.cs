using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.WeekPlan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
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
        Task<Result<WeekPlanOutput>> GetWeekPlansBookedHours(string requesterId, UserRole requesterRole, int weekPlanId);
    }
}
