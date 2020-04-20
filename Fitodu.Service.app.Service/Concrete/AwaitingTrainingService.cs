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
                var _awaitingTrainings = await _context.AwaitingTrainings.Where(x => x.IdCoach == requesterId)
                   .Select(x => new AwaitingTrainingOutput
                   {
                       Id = x.Id,
                       EndDate = x.EndDate,
                       StartDate = x.StartDate,
                       IdCoach = x.IdCoach,
                       IdClient = x.IdClient,
                   })
                   .ToListAsync();

                foreach (var x in _awaitingTrainings)
                {
                    var user = await _context.Clients.Where(y => y.Id == x.IdClient).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        x.RequestedName = user.Name;
                        x.RequestedSurname = user.Surname;
                    }
                }

                awaitingTrainings = _awaitingTrainings;
            }
            else if (requesterRole == UserRole.Client)
            {
                var _awaitingTrainings = await _context.AwaitingTrainings.Where(x => x.IdClient == requesterId)
                .Select(x => new AwaitingTrainingOutput
                {
                    Id = x.Id,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IdCoach = x.IdCoach,
                    IdClient = x.IdClient
                })
                .ToListAsync();

                foreach (var x in _awaitingTrainings)
                {
                    var user = await _context.Coaches.Where(y => y.Id == x.IdCoach).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        x.RequestedName = user.Name;
                        x.RequestedSurname = user.Surname;
                    }
                }
                awaitingTrainings = _awaitingTrainings;
            }

            if (awaitingTrainings == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find any awaiting trainings.";
                return result;
            }
            result.Data = awaitingTrainings;
            return result;
        }

        public async Task<Result<int>> CreateAwaitingTraining(string requesterId, UserRole requesterRole, AwaitingTrainingInput awaitingTrainingInput)
        {
            var result = new Result<int>();

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
                if (coachClient.AvailableTrainings <= 0)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Client does not have any trainings left";
                    return result;
                }
                coachClient.AvailableTrainings--;
                awaitingTraining.IdCoach = requesterId;
                awaitingTraining.IdClient = awaitingTrainingInput.IdReceiver;
                awaitingTraining.Sender = UserRole.Coach;
                awaitingTraining.Receiver = UserRole.Client;

                model.Subject = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingClientSubject;
                model.HtmlBody = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingClientBody;

                var coach = await _context.Coaches.Where(x => x.Id == requesterId).FirstOrDefaultAsync();

                var client = await _context.Clients.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();

                if (coach != null && client != null)
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", client.Name + " " + client.Surname).Replace("-coachName-", coach.Name + " " + coach.Surname);

                }
                else
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", "name").Replace("-coachName-", "Coach name");
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
                var coachClient = await _context.CoachClients.Where(x => x.IdCoach == clientsCoach.Data.Id && x.IdClient == requesterId).FirstOrDefaultAsync();
                if (coachClient == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "User is not a client of this coach";
                    return result;
                }
                if (coachClient.AvailableTrainings <= 0)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Client does not have any trainings left";
                    return result;
                }
                coachClient.AvailableTrainings--;

                awaitingTraining.IdCoach = awaitingTrainingInput.IdReceiver;
                awaitingTraining.IdClient = requesterId;
                awaitingTraining.Sender = UserRole.Client;
                awaitingTraining.Receiver = UserRole.Coach;

                model.Subject = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingCoachSubject;
                model.HtmlBody = Resource.AwaitingTrainingMailTemplate.NewAwaitingTrainingCoachBody;

                var coach = await _context.Coaches.Where(x => x.Id == awaitingTrainingInput.IdReceiver).FirstOrDefaultAsync();

                var client = await _context.Clients.Where(x => x.Id == requesterId).FirstOrDefaultAsync();

                if (coach != null && client != null)
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", coach.Name + " " + coach.Surname).Replace("-clientName-", client.Name + " " + client.Surname);

                }
                else
                {
                    model.HtmlBody = model.HtmlBody.Replace("-fullName-", "fullname").Replace("-clientName-", "Client name");
                }
            }

            if (awaitingTrainingInput.StartDate >= awaitingTrainingInput.EndDate || awaitingTrainingInput.StartDate.Date != awaitingTrainingInput.EndDate.Date)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Incorrect date, start date is greater that end date or requester training does not start and end the same day";
            }

            if (requesterRole == UserRole.Coach)
            {
                awaitingTraining.EndDate = awaitingTrainingInput.EndDate;
                awaitingTraining.StartDate = awaitingTrainingInput.StartDate;
            }
            else
            {
                var workoutTimes = _context.WorkoutTimes.Where(x =>
                                                        x.DayPlan.WeekPlan.IdCoach == awaitingTraining.IdCoach &&
                                                        x.DayPlan.WeekPlan.StartDate.Value.Date <= awaitingTrainingInput.StartDate.Date &&
                                                        x.DayPlan.WeekPlan.StartDate.Value.Date.AddDays(7) > awaitingTrainingInput.EndDate.Date &&
                                                        x.StartTime.TimeOfDay <= awaitingTrainingInput.StartDate.TimeOfDay &&
                                                        x.EndTime.TimeOfDay >= awaitingTrainingInput.EndDate.TimeOfDay);

                var defaultWeekplan = await _context.WeekPlans.Where(x => x.IsDefault == true && x.IdCoach == awaitingTraining.IdCoach).FirstOrDefaultAsync();

                if (defaultWeekplan != null)
                {
                    var defaultWorkoutTimes = _context.WorkoutTimes.Where(x => x.DayPlan.WeekPlan.Id == defaultWeekplan.Id &&
                                x.StartTime.TimeOfDay <= awaitingTrainingInput.StartDate.TimeOfDay &&
                                x.EndTime.TimeOfDay >= awaitingTrainingInput.EndDate.TimeOfDay);

                    workoutTimes = workoutTimes.Concat(defaultWorkoutTimes);
                }

                bool found = false;
                foreach (WorkoutTime workoutTime in workoutTimes)
                {
                    if ((workoutTime.DayPlan.Day == Day.Monday && awaitingTrainingInput.StartDate.DayOfWeek == DayOfWeek.Monday) ||
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

                if (!found)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "no workoutTimes matching requirements";
                    return result;
                }
            }

            _context.AwaitingTrainings.Add(awaitingTraining);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Couldn't save changes to the database";
                return result;
            }

            model.HtmlBody = model.HtmlBody.Replace("-url-", url);

            var response = await _emailService.Send(model);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Added awaiting training but failed to send to mail";
            }

            result.Data = awaitingTraining.Id;
            return result;
        }

        public async Task<Result<int>> DeleteAwaitingTraining(string requesterId, UserRole requesterRole, int id, bool? accept)
        {
            var result = new Result<int>();
            result.Data = -1;

            string clientName;
            string coachName;
            string date;
            string email = "";

            using (var transaction = _context.Database.BeginTransaction())
            {
                var exisitingAwaitingTraining = await _context.AwaitingTrainings.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (exisitingAwaitingTraining == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "awaiting training with this id does not exist";
                    return result;
                }
                if (requesterRole == UserRole.Coach)
                {
                    if (requesterId != exisitingAwaitingTraining.IdCoach && exisitingAwaitingTraining.Sender != UserRole.Coach)
                    {
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "awaiting training with this id, sender role and coach id does not exist";
                        return result;
                    }

                    var receiverEmail = await _context.Users.Where(x => x.Id == exisitingAwaitingTraining.IdClient).FirstOrDefaultAsync();

                    if (receiverEmail != null)
                    {
                        email = receiverEmail.NormalizedEmail;
                    }

                }
                else if (requesterRole == UserRole.Client)
                {
                    if (requesterId != exisitingAwaitingTraining.IdClient && exisitingAwaitingTraining.Sender != UserRole.Client)
                    {
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "awaiting training with this id, sender role and client id does not exist";
                        return result;
                    }

                    var receiverEmail = await _context.Users.Where(x => x.Id == exisitingAwaitingTraining.IdClient).FirstOrDefaultAsync();

                    if (receiverEmail != null)
                    {
                        email = receiverEmail.NormalizedEmail;
                    }
                }
                Training training = new Training();
                if (accept == true)
                {
                    //utworzenie treningu

                    training.IdClient = exisitingAwaitingTraining.IdClient;
                    training.IdCoach = exisitingAwaitingTraining.IdCoach;
                    training.StartDate = exisitingAwaitingTraining.StartDate;
                    training.EndDate = exisitingAwaitingTraining.EndDate;
                    training.Note = "";
                    training.Description = "";

                    _context.Trainings.Add(training);
                }
                else
                {
                    var coachClient = await _context.CoachClients.Where(x => x.IdCoach == exisitingAwaitingTraining.IdCoach && x.IdClient == exisitingAwaitingTraining.IdClient).FirstOrDefaultAsync();
                    coachClient.AvailableTrainings += 1;
                }



                _context.AwaitingTrainings.Remove(exisitingAwaitingTraining);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database";
                    return result;
                }
                if (accept == true)
                {
                    result.Data = training.Id;
                }

                var client = await _context.Clients.Where(x => x.Id == exisitingAwaitingTraining.IdClient).FirstOrDefaultAsync();
                var coach = await _context.Coaches.Where(x => x.Id == exisitingAwaitingTraining.IdCoach).FirstOrDefaultAsync();

                if (client == null || coach == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Mail: no coach or client with that Id, mail not sent";
                    return result;
                }
                clientName = client.Name + " " + client.Surname;
                coachName = coach.Name + " " + coach.Surname;
                date = exisitingAwaitingTraining.StartDate.ToString();

                transaction.Commit();
            }

            //wysyłanie maili
            if (accept != null)
            {
                if (email == "" || email == null)
                {
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Mail: receiver email not found, mail not sent";
                    return result;
                }

                var model = new EmailInput()
                {
                    To = email
                };

                if (requesterRole == UserRole.Coach)
                {
                    if (accept == true)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingAcceptedClientSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingAcceptedClientBody;
                    }
                    else if (accept == false)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingRejectedClientSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingRejectedClientBody;
                    }
                    else if (accept == null)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingWithdrawnClientSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingWithdrawnClientBody;
                    }
                }
                else if (requesterRole == UserRole.Client)
                {
                    if (accept == true)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingAcceptedCoachSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingAcceptedCoachBody;
                    }
                    else if (accept == false)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingRejectedCoachSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingRejectedCoachBody;
                    }
                    else if (accept == null)
                    {
                        model.Subject = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingWithdrawnCoachSubject;
                        model.HtmlBody = Resource.AwaitingTrainingMailTemplate.AwaitingTrainingWithdrawnCoachBody;
                    }
                    model.HtmlBody = model.HtmlBody.Replace("-coachName-", coachName);
                }


                string url = "https://fitodu.azurewebsites.net";
                model.HtmlBody = model.HtmlBody.Replace("-url-", url);
                model.HtmlBody = model.HtmlBody.Replace("-clientName-", clientName);
                model.HtmlBody = model.HtmlBody.Replace("-date-", date);

                var response = await _emailService.Send(model);

                if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
                {
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Mail: mail not sent";
                }

            }
            return result;
        }
    }
}
