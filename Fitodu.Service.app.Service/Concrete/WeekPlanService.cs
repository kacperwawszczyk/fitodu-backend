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
            result.Data = await weekPlans.ProjectTo<WeekPlanOutput>(_mapper.ConfigurationProvider).ToListAsync();
            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find any weekplans of that coach";
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

            if (!IsValidEditInput(weekPlan))
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid input (more than 7 days or one day is written twice)";
                return result;
            }

            if (weekPlan.StartDate == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid input: date is null";
                return result;
            }

            var exisitingWeekPlanStartDate = await _context.WeekPlans.Where(x => x.IdCoach == coachId && x.StartDate.Value.Date == weekPlan.StartDate.Value.Date && x.Id != weekPlan.Id).FirstOrDefaultAsync();


            //if (exisitingWeekPlanStartDate.IsDefault == false)
            //{
            //    result.Error = ErrorType.BadRequest;
            //    result.ErrorMessage = "Cannot assign empty date to non default weekplan";
            //    return result;
            //}

            if (exisitingWeekPlanStartDate != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Can not change this weekplans StartDate to the one given because this coach already has a different weekplan starting at that date";
                return result;
            }

            if (weekPlan.StartDate.Value.Date.DayOfWeek != DayOfWeek.Monday && exisitingWeekPlanStartDate.IsDefault == false)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Week plan must start at a Monday if it's not default";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var exisitngWeekPlan = await _context.WeekPlans
                    .Where(x => x.Id == weekPlan.Id && x.IdCoach == coachId).FirstOrDefaultAsync();

                if (exisitngWeekPlan.IsDefault == true)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Can't modify default weekplan with this method.";
                    return result;
                }

                if (exisitngWeekPlan == null)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "WeekPlan with this Id does not exist in the database";
                    return result;
                }


                exisitngWeekPlan.StartDate = weekPlan.StartDate;

                _context.DayPlans.RemoveRange(exisitngWeekPlan.DayPlans);

                exisitngWeekPlan.IdCoach = coachId;
                exisitngWeekPlan.StartDate = new DateTime(weekPlan.StartDate.Value.Date.Year, weekPlan.StartDate.Value.Date.Month, weekPlan.StartDate.Value.Date.Day, 0, 0, 0);

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
                    result.ErrorMessage = "Couldn't save changes to the database";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }

        public async Task<Result> EditDefaultWeekPlan(string coachId, UpdateDefaultWeekPlanInput weekPlan)
        {
            var result = new Result();

            if (!IsValidEditInput(weekPlan))
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid input (more than 7 days or one day is written twice)";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var exisitngWeekPlan = await _context.WeekPlans
                    .Where(x => x.Id == weekPlan.Id && x.IdCoach == coachId).FirstOrDefaultAsync();

                if (exisitngWeekPlan.IsDefault == false)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Can't modify non default weekplan with this method.";
                    return result;
                }

                if (exisitngWeekPlan == null)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "WeekPlan with this Id does not exist in the database";
                    return result;
                }


                //exisitngWeekPlan.StartDate = weekPlan.StartDate;
                // exisitngWeekPlan.StartDate = new DateTime(weekPlan.StartDate.Date.Year, weekPlan.StartDate.Date.Month, weekPlan.StartDate.Date.Day, 0, 0, 0);

                _context.DayPlans.RemoveRange(exisitngWeekPlan.DayPlans);
                exisitngWeekPlan.IdCoach = coachId;
                exisitngWeekPlan.StartDate = null;

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
                    result.ErrorMessage = "Couldn't save changes to the database";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }


        public async Task<Result<int>> CreateWeekPlan(string coachId, WeekPlanInput weekPlanInput)
        {
            var result = new Result<int>();


            if (!IsValidInput(weekPlanInput))
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid input (more than 7 days or one day is written twice)";
                return result;
            }

            if (weekPlanInput.StartDate.Date.DayOfWeek != DayOfWeek.Monday) //&& weekPlanInput.IsDefault == false)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Week plan must start at a Monday if it's not default";
                return result;
            }

            var exisitingWeekPlan = await _context.WeekPlans.Where(x => x.IdCoach == coachId && x.StartDate.Value.Date == weekPlanInput.StartDate.Date && x.IsDefault == false).FirstOrDefaultAsync();

            if (exisitingWeekPlan != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Week plan of this coach, starting at this date already exists";
                return result;
            }

            WeekPlan _weekPlan = new WeekPlan();
            _weekPlan.IdCoach = coachId;
            _weekPlan.StartDate = new DateTime(weekPlanInput.StartDate.Date.Year, weekPlanInput.StartDate.Date.Month, weekPlanInput.StartDate.Date.Day, 0, 0, 0);

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

            //if (weekPlanInput.IsDefault)
            //{
            //    var existingDefaultWeekPlan = await _context.WeekPlans.Where(x => x.IdCoach == coachId && x.IsDefault == true).ToListAsync();
            //    if (existingDefaultWeekPlan != null)
            //    {
            //        foreach (WeekPlan defaultWeekPlan in existingDefaultWeekPlan)
            //        {
            //            defaultWeekPlan.IsDefault = false;
            //        }
            //    }

            //    _weekPlan.IsDefault = true;
            //}
            //else
            //{
            //    _weekPlan.IsDefault = false;
            //}

            _weekPlan.DayPlans = dayPlans;

            _context.WeekPlans.Add(_weekPlan);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Couldn't save changes to the database.";
            }
            result.Data = _weekPlan.Id;
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

                if (exisitngWeekPlan.IsDefault == true)
                {
                    _context.DayPlans.RemoveRange(exisitngWeekPlan.DayPlans);
                }
                else
                {
                    _context.WeekPlans.Remove(exisitngWeekPlan);
                }

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database";
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

            for (int i = 0; i < 7; i++)
            {
                if (counters[i] > 1)
                {
                    return false;
                }
            }
            return true;
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

            for (int i = 0; i < 7; i++)
            {
                if (counters[i] > 1)
                {
                    return false;
                }
            }
            return true;
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

            for (int i = 0; i < 7; i++)
            {
                if (counters[i] > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidEditInput(UpdateDefaultWeekPlanInput editWeekPlanInput)
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

            for (int i = 0; i < 7; i++)
            {
                if (counters[i] > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<Result<WeekPlanOutput>> GetWeekPlansBookedHours(string requesterId, UserRole requesterRole, int weekPlanId)
        {
            var result = new Result<WeekPlanOutput>();

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

            //IQueryable weekPlan = _context.WeekPlans.Where(x => x.IdCoach == coachId && x.Id == weekPlanId);
            //if (weekPlan == null)
            //{
            //    result.Error = ErrorType.NotFound;
            //    return result;
            //}

            //var weekPlanOutput = await weekPlan.ProjectTo<WeekPlanOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var weekPlan = await _context.WeekPlans.Where(x => x.IdCoach == coachId && x.Id == weekPlanId).FirstOrDefaultAsync();

            var trainings = await _context.Trainings.Where(x => x.StartDate >= weekPlan.StartDate && x.EndDate <= weekPlan.StartDate.Value.AddDays(7)
                && x.IdCoach == coachId).ToListAsync();

            WeekPlanOutput weekPlanOutput = new WeekPlanOutput();
            List<DayPlanOutput> dayPlanOutputs = new List<DayPlanOutput>();

            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Monday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Tuesday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Wednesday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Thursday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Friday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Saturday, WorkoutTimes = new List<WorkoutTimeOutput>() });
            dayPlanOutputs.Add(new DayPlanOutput { Day = Day.Sunday, WorkoutTimes = new List<WorkoutTimeOutput>() });

            List<WorkoutTime> workoutTimes = new List<WorkoutTime>();
            if (trainings != null)
            {
                foreach (Training training in trainings)
                {
                    switch (training.StartDate.Value.Date.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dayPlanOutputs[0].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Tuesday:
                            dayPlanOutputs[1].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Wednesday:
                            dayPlanOutputs[2].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Thursday:
                            dayPlanOutputs[3].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Friday:
                            dayPlanOutputs[4].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Saturday:
                            dayPlanOutputs[5].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                        case DayOfWeek.Sunday:
                            dayPlanOutputs[6].WorkoutTimes.Add(new WorkoutTimeOutput { StartTime = (DateTime)training.StartDate, EndTime = (DateTime)training.EndDate });
                            break;
                    }

                    //foreach(DayPlanOutput dayPlan in weekPlanOutput.DayPlans)
                    //{
                    //    if(training.StartDate >= weekPlanOutput.StartDate.Value.Date.AddDays((int)dayPlan.Day))
                    //    {
                    //        foreach (WorkoutTimeOutput workoutTime in dayPlan.WorkoutTimes)
                    //        {

                    //            //workout time zaczyna się przed i kończy w trakcie
                    //            //workout time zaczyna się przed i kończy po treningu
                    //            //workout time zaczyna się w trakcie i kończy po treningu
                    //            //workout time zaczyna się w trakcie i kończy w trakcie treningu

                    //        }
                    //    }
                    //}


                }
            }

            foreach (DayPlanOutput dayPlanOutput in dayPlanOutputs.ToList())
            {
                if (dayPlanOutput.WorkoutTimes.Count == 0)
                {
                    dayPlanOutputs.Remove(dayPlanOutput);
                }
            }

            weekPlanOutput.IdCoach = weekPlan.IdCoach;
            weekPlanOutput.StartDate = weekPlan.StartDate;
            weekPlanOutput.IsDefault = weekPlan.IsDefault;
            weekPlanOutput.DayPlans = dayPlanOutputs;

            result.Data = weekPlanOutput;
            return result;
        }
    }
}
