using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class WeekPlanService : IWeekPlanService
    {
        private readonly Context _context;
        private readonly IClientService _clientService;

        public WeekPlanService(Context context, IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        public async Task<Result<ICollection<WeekPlan>>> GetWeekPlans(string coachId, string requesterId, UserRole requesterRole)
        {
            var result = new Result<ICollection<WeekPlan>>();

            if(requesterRole == UserRole.Coach)
            {
                coachId = requesterId;
            }
            else if(requesterRole == UserRole.Client)
            {
                //TODO: zmienić jeśli client może mieć wielu trenerów
                var clientsCoach = await _clientService.GetClientCoach(requesterId);

                if(coachId != clientsCoach.Data.Id)
                {
                    result.Error = ErrorType.Forbidden;
                    return result;
                }
            }

            var weekPlans = await _context.WeekPlans.Where(x => x.IdCoach == coachId).ToListAsync();

            if(weekPlans != null)
            {
                result.Data = weekPlans;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;

        }
    }
}
