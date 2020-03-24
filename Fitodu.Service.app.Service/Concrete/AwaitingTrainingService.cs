using AutoMapper;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.AwaitingTraining;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class AwaitingTrainingService : IAwaitingTrainingService
    {
        private readonly Context _context;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AwaitingTrainingService(Context context, IClientService clientService,
            IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _clientService = clientService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Result<ICollection<AwaitingTrainingOutput>>> GetAwaitingTraining(string requesterId, UserRole requesterRole)
        {
            var result = new Result<ICollection<AwaitingTrainingOutput>>();

            var awaitingTrainings = new List<AwaitingTrainingOutput>();

            if (requesterRole == UserRole.Coach)
            {
                 awaitingTrainings = await _context.AwaitingTrainings.Where(x => x.IdCoach == requesterId)
                    .Select(x => new AwaitingTrainingOutput { 
                    Id = x.Id,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IdCoach = x.IdCoach,
                    IdClient = x.IdClient
                    })
                    .ToListAsync();
            }
            else if (requesterRole == UserRole.Client)
            {
                awaitingTrainings = await _context.AwaitingTrainings.Where(x => x.IdClient == requesterId)
                .Select(x => new AwaitingTrainingOutput
                {
                    Id = x.Id,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IdCoach = x.IdCoach,
                    IdClient = x.IdClient
                })
                .ToListAsync();
            }

            if(awaitingTrainings == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }
            result.Data = awaitingTrainings;
            return result;
        }

        public async Task<Result> CreateAwaitingTraining(string requesterId, UserRole requesterRole, AwaitingTrainingInput awaitingTrainingInput)
        {
            var result = new Result();

            AwaitingTraining awaitingTraining = new AwaitingTraining();

            var receiverEmail = await _context.Users.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();

            if (receiverEmail == null)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Receiver id not found";
                return result;
            }
            string url = "https://fitodu.azurewebsites.net";

            string email = receiverEmail.NormalizedEmail;


            var model = new EmailInput()
            {
                To = email
            };

            if (requesterRole == UserRole.Coach)
            {
                var coachClient = await _context.CoachClients.Where(x => x.IdCoach == requesterId && x.IdClient == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();
                var existingClient = await _context.Users.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();
                if (coachClient == null || existingClient == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Client not found";
                    return result;
                }
                awaitingTraining.IdCoach = requesterId;
                awaitingTraining.IdClient = awaitingTrainingInput.IdReceiver;
                awaitingTraining.Sender = UserRole.Coach;
                awaitingTraining.Receiver = UserRole.Client;

                model.Subject = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingCoachSubject;
                model.HtmlBody = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingCoachBody;

                var coach = await _context.Coaches.Where(x => x.Id == requesterId).FirstOrDefaultAsync();

                var client = await _context.Clients.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();
                
                if(coach != null && client != null)
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", coach.Name+" "+coach.Surname).Replace("-clientName-", client.Name+" "+client.Surname);

                }
                else
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", "name").Replace("-clientName-", "fullname");
                }
            }
            else if (requesterRole == UserRole.Client)
            { 
                var clientsCoach = await _clientService.GetClientCoach(requesterId);
                if (clientsCoach == null || clientsCoach.Data.Id != awaitingTrainingInput.IdReceiver)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Coach not found";
                    return result;
                }
                awaitingTraining.IdCoach = awaitingTrainingInput.IdReceiver;
                awaitingTraining.IdClient = requesterId;
                awaitingTraining.Sender = UserRole.Client;
                awaitingTraining.Receiver = UserRole.Coach;

                model.Subject = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingClientSubject;
                model.HtmlBody = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingClientBody;

                var coach = await _context.Coaches.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();

                var client = await _context.Clients.Where(x => x.Id == requesterId).FirstOrDefaultAsync();

                if (coach != null && client != null)
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", client.Name + " " + client.Surname).Replace("-coachName-", coach.Name + " " + coach.Surname);

                }
                else
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", "fullname").Replace("-coachName-", "Coach name");
                }
            }

            if(awaitingTrainingInput.StartDate >= awaitingTrainingInput.EndDate || awaitingTrainingInput.StartDate.Date != awaitingTrainingInput.EndDate.Date)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Incorrect date, start date is greater that end date or requester training does not start and end the same day";
            }

            var workoutTimes = _context.WorkoutTimes.Where(x =>
            x.DayPlan.WeekPlan.IdCoach == awaitingTraining.IdCoach &&
            x.DayPlan.WeekPlan.StartDate.Value.Date < awaitingTrainingInput.StartDate.Date &&
            x.DayPlan.WeekPlan.StartDate.Value.Date.AddDays(7) > awaitingTrainingInput.EndDate.Date &&
            x.StartTime.TimeOfDay < awaitingTrainingInput.StartDate.TimeOfDay && 
            x.EndTime.TimeOfDay > awaitingTrainingInput.EndDate.TimeOfDay);

            bool found = false;
            foreach (WorkoutTime workoutTime in workoutTimes)
            {
                if((workoutTime.DayPlan.Day == Day.Monday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Monday) ||
                    (workoutTime.DayPlan.Day == Day.Wednesday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Wednesday) ||
                    (workoutTime.DayPlan.Day == Day.Tuesday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Tuesday) ||
                    (workoutTime.DayPlan.Day == Day.Thursday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Thursday) ||
                    (workoutTime.DayPlan.Day == Day.Saturday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Saturday) ||
                    (workoutTime.DayPlan.Day == Day.Sunday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Sunday) ||
                    (workoutTime.DayPlan.Day == Day.Friday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Friday))
                {
                    found = true;
                    awaitingTraining.StartDate = awaitingTrainingInput.StartDate;
                    awaitingTraining.EndDate = awaitingTrainingInput.EndDate;
                    break;
                }
            }

            if(!found)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "no workoutTimes matching requirements";
                return result;
            }

            _context.AwaitingTrainings.Add(awaitingTraining);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
                return result;
            }

            model.HtmlBody = model.HtmlBody.Replace("-url-", url);

            var response = await _emailService.Send(model);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Added awaiting training but failed to send to mail";
            }
            return result;
        }
    }
}
