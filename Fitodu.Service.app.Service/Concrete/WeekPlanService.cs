using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Extensions;
using Fitodu.Service.Models;
using Fitodu.Service.Models.WeekPlan;
using Fitodu.Service.Models.WorkoutTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class WeekPlanService : IWeekPlanService
    {
        private readonly Context _context;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public WeekPlanService(Context context, IClientService clientService, IMapper mapper)
        {
            _context = context;
            _clientService = clientService;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<WeekPlanOutput>>> GetWeekPlans(string requesterId, UserRole requesterRole)
        {
            var result = new Result<ICollection<WeekPlanOutput>>();

            string coachId = "";

            if(requesterRole == UserRole.Coach)
            {
                coachId = requesterId;
            }
            else if(requesterRole == UserRole.Client)
            {
                //TODO: zmienić jeśli client może mieć wielu trenerów
                var clientsCoach = await _clientService.GetClientCoach(requesterId);

                if(clientsCoach == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Coach not found";
                    return result;
                }
                else
                {
                    coachId = clientsCoach.Data.Id;
                }
            }
            IQueryable weekPlans = null;
            weekPlans = _context.WeekPlans.Where(x => x.IdCoach == coachId);

            if(weekPlans != null)
            {
                result.Data = await weekPlans.ProjectTo<WeekPlanOutput>(_mapper.ConfigurationProvider)
                .ToListAsync();
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;

        }

        public async Task<Result<ICollection<WeekPlanListOutput>>> GetWeekPlansShort(string requesterId, UserRole requesterRole, WeekPlanListInput model)
        {
            var result = new Result<ICollection<WeekPlanListOutput>>();

            string coachId = "";

            if (requesterRole == UserRole.Coach)
            {
                coachId = requesterId;
            }
            else if (requesterRole == UserRole.Client)
            {
                //TODO: zmienić jeśli client może mieć wielu trenerów
                var clientsCoach = await _clientService.GetClientCoach(requesterId);

                if (clientsCoach == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Coach not found";
                    return result;
                }
                else
                {
                    coachId = clientsCoach.Data.Id;
                }
            }
            IQueryable weekPlans = null;
            weekPlans = _context.WeekPlans.Where(x => x.IdCoach == coachId);

            if (weekPlans != null)
            {
                result.Data = await weekPlans
                    //.OrderBy(model.Sort.Column, model.Sort.Direction)
                    .ProjectTo<WeekPlanListOutput>(_mapper.ConfigurationProvider)
                    .OrderBy(model.Sort.Column, model.Sort.Direction)
                    .Paging(model.Page, model.PageSize)
                    .ToListAsync();
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;

        }

        public async Task<Result> EditWeekPlan(string coachId, UpdateWeekPlanInput weekPlan)
        {
            var result = new Result();

            if(!IsValidEditInput(weekPlan))
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var exisitngWeekPlan = await _context.WeekPlans
                    .Where(x => x.Id == weekPlan.Id && x.IdCoach == coachId).FirstOrDefaultAsync();

                if(exisitngWeekPlan == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "WeekPlan with this Id does not exist in the database";
                    return result;
                }

                exisitngWeekPlan.StartDate = weekPlan.StartDate;

                _context.DayPlans.RemoveRange(exisitngWeekPlan.DayPlans);

                //exisitngWeekPlan.DayPlans = weekPlan.DayPlans;


                exisitngWeekPlan.IdCoach = coachId;
                exisitngWeekPlan.StartDate = weekPlan.StartDate;

                List<DayPlan> dayPlans = new List<DayPlan>();
                foreach (DayPlanInput dayPlanInput in weekPlan.DayPlans)
                {
                    DayPlan _dayPlan = new DayPlan();
                    _dayPlan.Day = dayPlanInput.Day;
                    _dayPlan.WeekPlan = exisitngWeekPlan;

                    List<WorkoutTime> workoutTimes = new List<WorkoutTime>();
                    foreach (WorkoutTimeInput workoutTimeInput in dayPlanInput.WorkoutTimes)
                    {
                        WorkoutTime _workoutTime = new WorkoutTime();
                        _workoutTime.DayPlan = _dayPlan;
                        _workoutTime.StartTime = workoutTimeInput.StartTime;
                        _workoutTime.EndTime = workoutTimeInput.EndTime;
                        workoutTimes.Add(_workoutTime);

                    }
                    _dayPlan.WorkoutTimes = workoutTimes;
                    dayPlans.Add(_dayPlan);
                }

                exisitngWeekPlan.DayPlans = dayPlans;

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }


        public async Task<Result> CreateWeekPlan(string coachId, WeekPlanInput weekPlanInput)
        {
            var result = new Result();

            if (!IsValidInput(weekPlanInput))
            {
                result.Error = ErrorType.BadRequest;
                return result;
            }

            WeekPlan _weekPlan = new WeekPlan();
            _weekPlan.IdCoach = coachId;
            _weekPlan.StartDate = weekPlanInput.StartDate;

            List<DayPlan> dayPlans = new List<DayPlan>();
            foreach (DayPlanInput dayPlanInput in weekPlanInput.DayPlans)
            {
                DayPlan _dayPlan = new DayPlan();
                _dayPlan.Day = dayPlanInput.Day;
                _dayPlan.WeekPlan = _weekPlan;

                List<WorkoutTime> workoutTimes = new List<WorkoutTime>();
                foreach (WorkoutTimeInput workoutTimeInput in dayPlanInput.WorkoutTimes)
                {
                    WorkoutTime _workoutTime = new WorkoutTime();
                    _workoutTime.DayPlan = _dayPlan;
                    _workoutTime.StartTime = workoutTimeInput.StartTime;
                    _workoutTime.EndTime = workoutTimeInput.EndTime;
                    workoutTimes.Add(_workoutTime);
                    
                }
                _dayPlan.WorkoutTimes = workoutTimes;
                dayPlans.Add(_dayPlan);
            }

            _weekPlan.DayPlans = dayPlans;

            _context.WeekPlans.Add(_weekPlan);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
            }
            return result;
        }

        public async Task<Result> DeleteWeekPlan(string coachId, int weekPlanId)
        {
            var result = new Result();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var exisitngWeekPlan = await _context.WeekPlans
                    .Where(x => x.Id == weekPlanId && x.IdCoach == coachId).FirstOrDefaultAsync();

                if (exisitngWeekPlan == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "WeekPlan with this Id does not exist in the database";
                    return result;
                }

                _context.WeekPlans.Remove(exisitngWeekPlan);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public bool IsValid(WeekPlan weekPlan)
        {

            if (weekPlan.DayPlans.Count > 7)
            { 
                return false;
            }

            int[] counters = { 0, 0, 0, 0, 0, 0, 0 };

            List<DayPlan> _dayPlans = weekPlan.DayPlans.ToList();

            for (int i=0; i<_dayPlans.Count; i++)
            {
                if(_dayPlans[i].Day == Day.Monday)
                {
                    counters[0]++;
                }
                else if (_dayPlans[i].Day == Day.Tuesday)
                {
                    counters[1]++;
                }
                else if (_dayPlans[i].Day == Day.Wednesday)
                {
                    counters[2]++;
                }
                else if (_dayPlans[i].Day == Day.Thursday)
                {
                    counters[3]++;
                }
                else if (_dayPlans[i].Day == Day.Friday)
                {
                    counters[4]++;
                }
                else if (_dayPlans[i].Day == Day.Saturday)
                {
                    counters[5]++;
                }
                else if (_dayPlans[i].Day == Day.Sunday)
                {
                    counters[6]++;
                }
            }

            if(counters.Contains(2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsValidInput(WeekPlanInput weekPlanInput)
        {
            if (weekPlanInput.DayPlans.Count > 7)
            {
                return false;
            }

            int[] counters = { 0, 0, 0, 0, 0, 0, 0 };

            List<DayPlanInput> _dayPlans = weekPlanInput.DayPlans.ToList();

            for (int i = 0; i < _dayPlans.Count; i++)
            {
                if (_dayPlans[i].Day == Day.Monday)
                {
                    counters[0]++;
                }
                else if (_dayPlans[i].Day == Day.Tuesday)
                {
                    counters[1]++;
                }
                else if (_dayPlans[i].Day == Day.Wednesday)
                {
                    counters[2]++;
                }
                else if (_dayPlans[i].Day == Day.Thursday)
                {
                    counters[3]++;
                }
                else if (_dayPlans[i].Day == Day.Friday)
                {
                    counters[4]++;
                }
                else if (_dayPlans[i].Day == Day.Saturday)
                {
                    counters[5]++;
                }
                else if (_dayPlans[i].Day == Day.Sunday)
                {
                    counters[6]++;
                }
            }

            if (counters.Contains(2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsValidEditInput(UpdateWeekPlanInput editWeekPlanInput)
        {

            if (editWeekPlanInput.DayPlans.Count > 7)
            {
                return false;
            }

            int[] counters = { 0, 0, 0, 0, 0, 0, 0 };

            List<DayPlanInput> _dayPlans = editWeekPlanInput.DayPlans.ToList();

            for (int i = 0; i < _dayPlans.Count; i++)
            {
                if (_dayPlans[i].Day == Day.Monday)
                {
                    counters[0]++;
                }
                else if (_dayPlans[i].Day == Day.Tuesday)
                {
                    counters[1]++;
                }
                else if (_dayPlans[i].Day == Day.Wednesday)
                {
                    counters[2]++;
                }
                else if (_dayPlans[i].Day == Day.Thursday)
                {
                    counters[3]++;
                }
                else if (_dayPlans[i].Day == Day.Friday)
                {
                    counters[4]++;
                }
                else if (_dayPlans[i].Day == Day.Saturday)
                {
                    counters[5]++;
                }
                else if (_dayPlans[i].Day == Day.Sunday)
                {
                    counters[6]++;
                }
            }

            if (counters.Contains(2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
