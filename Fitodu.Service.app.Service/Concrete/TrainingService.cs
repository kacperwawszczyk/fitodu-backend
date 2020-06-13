using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using AutoMapper.QueryableExtensions;
using System.Net;

namespace Fitodu.Service.Concrete
{
    public class TrainingService : ITrainingService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Context _context;
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;
        private string azureConnectionString;

        public TrainingService(Context context, IMapper mapper, IClientService clientService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _clientService = clientService;
            _configuration = configuration;
            _emailService = emailService;
            azureConnectionString = _configuration.GetConnectionString("StorageConnection");
        }

        //TODO: Usunąć jak na pewno nie będzie potrzebne
        //public async Task<Result<ICollection<Training>>> GetTrainings(string id, UserRole role)
        //{
        //    var result = new Result<ICollection<Training>>();

        //    var trainings = new List<Training>();

        //    if(role == UserRole.Coach)
        //    {
        //        trainings = await _context.Trainings.Where(x => x.IdCoach == id).ToListAsync();
        //    }
        //    else if(role == UserRole.Client)
        //    {
        //        trainings = await _context.Trainings.Where(x => x.IdClient == id).ToListAsync();
        //    }

        //    if(trainings != null)
        //    {
        //        result.Data = trainings;
        //    }
        //    else
        //    {
        //        result.Error = ErrorType.NotFound;
        //    }
        //    return result;
        //}

        //public async Task<Result<ICollection<Training>>> GetCoachsTrainings(string idCoach)
        //{
        //    var result = new Result<ICollection<Training>>();

        //    var coachsTrainings = await _context.Trainings.Where(x => x.IdCoach == idCoach).ToListAsync();

        //    if(coachsTrainings != null)
        //    {
        //        result.Data = coachsTrainings;
        //    }
        //    else
        //    {
        //        result.Error = ErrorType.NotFound;
        //    }
        //    return result;

        //}

        //public async Task<Result<ICollection<Training>>> GetClientsTrainings(string idClient)
        //{
        //    var result = new Result<ICollection<Training>>();

        //    var clientsTrainings = await _context.Trainings.Where(x => x.IdClient == idClient).ToListAsync();

        //    if(clientsTrainings != null)
        //    {
        //        result.Data = clientsTrainings;
        //    }
        //    else
        //    {
        //        result.Error = ErrorType.NotFound;
        //    }
        //    return result;
        //}

        public async Task<Result<int>> AddTraining(string coachId, TrainingInput trainingInput)
        {
            var result = new Result<int>();

            var clientsCoach = await _clientService.GetClientCoach(trainingInput.IdClient);
            if (clientsCoach.IsDataNull || clientsCoach.Data.Id != coachId)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This coach does not work with this client";
                return result;
            }

            var client = await _context.Users.Where(x => x.Id == trainingInput.IdClient).FirstOrDefaultAsync();

            if (client != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This client does have an account";
                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdCoach == clientsCoach.Data.Id && x.IdClient == trainingInput.IdClient).FirstOrDefaultAsync();
            if (coachClient == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "User is not a client of this coach";
                return result;
            }
            if (coachClient.AvailableTrainings <= 0)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Klient nie ma dostępnych treningów";
                return result;
            }
            coachClient.AvailableTrainings -= 1;

            if (trainingInput.StartDate >= trainingInput.EndDate)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "End date is lesser than or equal to start date";
                return result;
            }

            if (trainingInput.StartDate.Value.Date != trainingInput.EndDate.Value.Date)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "end date and start date are not on the same day";
                return result;
            }

            Training _training = new Training();
            _training.IdClient = trainingInput.IdClient;
            _training.IdCoach = coachId;
            _training.Name = trainingInput.Name;
            if (trainingInput.Note != null) _training.Note = trainingInput.Note;
            _training.StartDate = new DateTime(trainingInput.StartDate.Value.Year, trainingInput.StartDate.Value.Month, trainingInput.StartDate.Value.Day
                , trainingInput.StartDate.Value.Hour, trainingInput.StartDate.Value.Minute, 0);
            _training.EndDate = new DateTime(trainingInput.EndDate.Value.Year, trainingInput.EndDate.Value.Month, trainingInput.EndDate.Value.Day
                , trainingInput.EndDate.Value.Hour, trainingInput.EndDate.Value.Minute, 0);
            _training.Description = trainingInput.Description;

            _context.Trainings.Add(_training);
            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Couldn't save changes to the database";
            }

            result.Data = _training.Id;
            return result;

        }

        public async Task<Result<string>> GetTrainingsCoach(int idTraining)
        {
            var result = new Result<string>();

            Training training = await _context.Trainings.Where(x => x.Id == idTraining).FirstOrDefaultAsync();

            if (training != null)
            {
                result.Data = training.IdCoach;
            }
            else
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "This coach isn't related to that training";
            }
            return result;
        }

        public async Task<Result> EditTraining(string coachId, UpdateTrainingInput trainingInput)
        {
            var result = new Result();
            var clientsCoach = await _clientService.GetClientCoach(trainingInput.IdClient);
            if (clientsCoach.IsDataNull || clientsCoach.Data.Id != coachId)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This coach does not work with this client";
                return result;
            }
            using (var transaction = _context.Database.BeginTransaction())
            {

                var exisitngTraining = await _context.Trainings.Where(
                x => x.Id == trainingInput.Id && x.IdCoach == coachId).FirstOrDefaultAsync();

                var _tmpExistingTraining = exisitngTraining;

                if (exisitngTraining == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Training with this Id does not exist in the database";
                    return result;
                }
                if (trainingInput.StartDate >= trainingInput.EndDate)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "End date is lesser than or equal to start date";
                    return result;
                }

                if (trainingInput.StartDate.Value.Date != trainingInput.EndDate.Value.Date)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "end date and start date are not on the same day";
                    return result;
                }

                exisitngTraining.IdClient = trainingInput.IdClient;
                exisitngTraining.Name = trainingInput.Name;
                exisitngTraining.Note = trainingInput.Note;
                exisitngTraining.Description = trainingInput.Description;
                exisitngTraining.StartDate = new DateTime(trainingInput.StartDate.Value.Year, trainingInput.StartDate.Value.Month, trainingInput.StartDate.Value.Day
                , trainingInput.StartDate.Value.Hour, trainingInput.StartDate.Value.Minute, 0);
                exisitngTraining.EndDate = new DateTime(trainingInput.EndDate.Value.Year, trainingInput.EndDate.Value.Month, trainingInput.EndDate.Value.Day
                , trainingInput.EndDate.Value.Hour, trainingInput.EndDate.Value.Minute, 0);

                _context.Trainings.Update(exisitngTraining);
                if (await _context.SaveChangesAsync() == 0)
                {
                    if (exisitngTraining.Equals(_tmpExistingTraining))
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Couldn't save changes to the database";
                        return result;
                    }
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }


        public async Task<Result> DeleteTraining(string requesterId, UserRole requesterRole, int trainingId, int time_zone_offset)
        {
            var result = new Result();

            string clientName;
            string coachName;
            string date;
            string email = "";
            string url = "https://fitodu.azurewebsites.net";
            var model = new EmailInput();

            if (requesterRole == UserRole.Coach)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var existingTraining = await _context.Trainings.Where(x => x.Id == trainingId && x.IdCoach == requesterId).FirstOrDefaultAsync();

                    if (existingTraining == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "Training with this Id does not exist in the database";
                        return result;
                    }

                    var coach = await _context.Coaches.Where(x => x.Id == requesterId).FirstOrDefaultAsync();
                    var client = await _context.Clients.Where(x => x.Id == existingTraining.IdClient).FirstOrDefaultAsync();
                    var clientcoach = await _context.CoachClients.Where(x => x.IdClient == existingTraining.IdClient && x.IdCoach == existingTraining.IdCoach).FirstOrDefaultAsync();

                    if (clientcoach == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "User is not client of that coach!";
                        return result;
                    }
                    if (client == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "Client with given id does not exist!";
                        return result;
                    }
                    if (coach == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "Coach with given id does not exist!";
                        return result;
                    }
                    if (existingTraining.StartDate > DateTime.UtcNow.AddMinutes(Convert.ToDouble(-time_zone_offset)))
                    {
                        if (client.IsRegistered)
                        {
                            var user = await _context.Users.Where(x => x.Id == client.Id).FirstOrDefaultAsync();
                            if (user == null)
                            {
                                transaction.Rollback();
                                result.Error = ErrorType.InternalServerError;
                                result.ErrorMessage = "Client account does not exist (Internal error? Cause: client is registered)";
                                return result;
                            }
                            clientName = client.Name + " " + client.Surname;
                            //coachName = coach.Name + " " + coach.Surname;
                            email = user.Email;
                            if (email == "" || email == null)
                            {
                                transaction.Rollback();
                                result.Error = ErrorType.InternalServerError;
                                result.ErrorMessage = "Mail: receiver email not found, mail not sent";
                                return result;
                            }
                            date = existingTraining.StartDate.Value.Day.ToString() + "." + existingTraining.StartDate.Value.Month.ToString() + "." + existingTraining.StartDate.Value.Year.ToString() + " " + existingTraining.StartDate.Value.Hour.ToString() + ":" + existingTraining.StartDate.Value.Minute.ToString();

                            model.To = email;
                            model.Subject = Resource.TrainingMailTemplate.TrainingCoachWithdrawalSubject;
                            model.HtmlBody = Resource.TrainingMailTemplate.TrainingCoachWithdrawalBody;
                            model.HtmlBody = model.HtmlBody.Replace("-url-", url);
                            model.HtmlBody = model.HtmlBody.Replace("-clientName-", clientName);
                            model.HtmlBody = model.HtmlBody.Replace("-date-", date);
                        }
                        clientcoach.AvailableTrainings += 1;
                        //_context.CoachClients.Update(clientcoach);
                        //if (await _context.SaveChangesAsync() == 0)
                        //{
                        //    transaction.Rollback();
                        //    result.Error = ErrorType.InternalServerError;
                        //    result.ErrorMessage = "Couldn't save changes to the database";
                        //    return result;
                        //}
                    }

                    var trainingsExercises = await _context.TrainingExercises.Where
                    (x => x.IdTraining == trainingId).ToListAsync();

                    if (trainingsExercises.Count != 0)
                    {
                        _context.TrainingExercises.RemoveRange(trainingsExercises);
                        //if (await _context.SaveChangesAsync() == 0)
                        //{
                        //    transaction.Rollback();
                        //    result.Error = ErrorType.InternalServerError;
                        //    result.ErrorMessage = "Couldn't save changes to the database";
                        //    return result;
                        //}
                    }

                    _context.Trainings.Remove(existingTraining);

                    if (await _context.SaveChangesAsync() == 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Couldn't save changes to the database";
                        return result;
                    }

                    transaction.Commit();

                    if (existingTraining.StartDate > DateTime.UtcNow.AddMinutes(Convert.ToDouble(-time_zone_offset)) && client.IsRegistered)
                    {
                        var response = await _emailService.Send(model);

                        if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
                        {
                            result.Error = ErrorType.InternalServerError;
                            result.ErrorMessage = "Mail: mail not sent but training was deleted";
                            return result;
                        }
                    }
                }
            }
            else if (requesterRole == UserRole.Client)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var existingTraining = await _context.Trainings.Where(
                    x => x.Id == trainingId && x.IdClient == requesterId).FirstOrDefaultAsync();

                    if (existingTraining == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.NotFound;
                        result.ErrorMessage = "Training with this Id does not exist in the database";
                        return result;
                    }

                    var coach = await _context.Coaches.Where(x => x.Id == existingTraining.IdCoach).FirstOrDefaultAsync();
                    var client = await _context.Clients.Where(x => x.Id == existingTraining.IdClient).FirstOrDefaultAsync();
                    var clientcoach = await _context.CoachClients.Where(x => x.IdClient == existingTraining.IdClient && x.IdCoach == existingTraining.IdCoach).FirstOrDefaultAsync();

                    if (clientcoach == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "User is not client of that coach!";
                        return result;
                    }
                    if (client == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "Client with given id does not exist!";
                        return result;
                    }
                    if (coach == null)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "Coach with given id does not exist!";
                        return result;
                    }

                    if (existingTraining.StartDate > DateTime.UtcNow.AddMinutes(Convert.ToDouble(-time_zone_offset)).AddHours(Convert.ToDouble(coach.CancelTimeHours)).AddMinutes(Convert.ToDouble(coach.CancelTimeMinutes)))
                    {
                        clientcoach.AvailableTrainings += 1;
                        //_context.CoachClients.Update(clientcoach);

                        //if (await _context.SaveChangesAsync() == 0)
                        //{
                        //    transaction.Rollback();
                        //    result.Error = ErrorType.InternalServerError;
                        //    result.ErrorMessage = "Couldn't save changes to the database";
                        //    return result;
                        //}

                        var coachAcc = await _context.Users.Where(x => x.Id == coach.Id).FirstOrDefaultAsync();
                        if (coachAcc == null)
                        {
                            transaction.Rollback();
                            result.Error = ErrorType.InternalServerError;
                            result.ErrorMessage = "Coach account does not exist (Internal error?)";
                            return result;
                        }
                        clientName = client.Name + " " + client.Surname;
                        coachName = coach.Name + " " + coach.Surname;
                        email = coachAcc.Email;
                        if (email == "" || email == null)
                        {
                            transaction.Rollback();
                            result.Error = ErrorType.InternalServerError;
                            result.ErrorMessage = "Mail: receiver email not found, mail not sent";
                            return result;
                        }
                        date = existingTraining.StartDate.Value.Day.ToString() + "." + existingTraining.StartDate.Value.Month.ToString() + "." + existingTraining.StartDate.Value.Year.ToString() + " " + existingTraining.StartDate.Value.Hour.ToString() + ":" + existingTraining.StartDate.Value.Minute.ToString();

                        model.To = email;
                        model.Subject = Resource.TrainingMailTemplate.TrainingClientWithdrawalSubject;
                        model.HtmlBody = Resource.TrainingMailTemplate.TrainingClientWithdrawalBody;
                        model.HtmlBody = model.HtmlBody.Replace("-url-", url);
                        model.HtmlBody = model.HtmlBody.Replace("-clientName-", clientName);
                        model.HtmlBody = model.HtmlBody.Replace("-coachName-", coachName);
                        model.HtmlBody = model.HtmlBody.Replace("-date-", date);

                        var trainingsExercises = await _context.TrainingExercises.Where(x => x.IdTraining == trainingId).ToListAsync();

                        if (trainingsExercises.Count != 0)
                        {
                            _context.TrainingExercises.RemoveRange(trainingsExercises);
                            //if (await _context.SaveChangesAsync() == 0)
                            //{
                            //    transaction.Rollback();
                            //    result.Error = ErrorType.InternalServerError;
                            //    result.ErrorMessage = "Couldn't save changes to the database";
                            //    return result;
                            //}
                        }

                        _context.Trainings.Remove(existingTraining);

                        if (await _context.SaveChangesAsync() == 0)
                        {
                            transaction.Rollback();
                            result.Error = ErrorType.InternalServerError;
                            result.ErrorMessage = "Couldn't save changes to the database";
                            return result;
                        }

                        transaction.Commit();

                        if (existingTraining.StartDate > DateTime.UtcNow.AddMinutes(Convert.ToDouble(-time_zone_offset)).AddHours(Convert.ToDouble(coach.CancelTimeHours)).AddMinutes(Convert.ToDouble(coach.CancelTimeMinutes)))
                        {
                            var response = await _emailService.Send(model);

                            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
                            {
                                result.Error = ErrorType.InternalServerError;
                                result.ErrorMessage = "Mail: mail not sent but training was deleted";
                                return result;
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.BadRequest;
                        result.ErrorMessage = "Cannot cancel workout because you exceeded coach's time limit";
                        return result;
                    }
                }
            }
            return result;
        }

        public async Task<Result<string>> GetTrainingsClient(int idTraining)
        {
            var result = new Result<string>();

            Training training = await _context.Trainings.Where(x => x.Id == idTraining).FirstOrDefaultAsync();

            if (training != null)
            {
                result.Data = training.IdClient;
            }
            else
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "This client isn't related with that training.";
            }
            return result;
        }

        public async Task<Result<ICollection<TrainingListOutput>>> GetTrainings(string id, UserRole role, string date, string idClient)
        {
            var result = new Result<ICollection<TrainingListOutput>>();

            IQueryable trainings = null;

            //var workoutDate = new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(5, 2)), int.Parse(date.Substring(8, 2)),0,0,0);
            if (!String.IsNullOrEmpty(date))
            {
                var workoutDate = DateTime.Parse(date);
                if (!String.IsNullOrEmpty(idClient))
                {
                    if (role == UserRole.Coach)
                    {
                        var client = _context.Clients.Where(x => x.Id == idClient).FirstOrDefaultAsync();
                        if (client != null)
                        {
                            trainings = _context.Trainings.Where(x => x.IdCoach == id && x.StartDate > workoutDate && x.IdClient == idClient);
                        }
                        else
                        {
                            result.Error = ErrorType.BadRequest;
                            result.ErrorMessage = "Client does not exist";
                            return result;
                        }
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = _context.Trainings.Where(x => x.IdClient == id && x.StartDate > workoutDate);
                    }
                }
                else
                {
                    if (role == UserRole.Coach)
                    {
                        trainings = _context.Trainings.Where(x => x.IdCoach == id && x.StartDate > workoutDate);
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = _context.Trainings.Where(x => x.IdClient == id && x.StartDate > workoutDate);
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(idClient))
                {
                    if (role == UserRole.Coach)
                    {
                        var client = _context.Clients.Where(x => x.Id == idClient).FirstOrDefaultAsync();
                        if (client != null)
                        {
                            trainings = _context.Trainings.Where(x => x.IdCoach == id && x.IdClient == idClient);
                        }
                        else
                        {
                            result.Error = ErrorType.BadRequest;
                            result.ErrorMessage = "Client does not exist";
                            return result;
                        }
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = _context.Trainings.Where(x => x.IdClient == id);
                    }
                }
                else
                {
                    if (role == UserRole.Coach)
                    {
                        trainings = _context.Trainings.Where(x => x.IdCoach == id);
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = _context.Trainings.Where(x => x.IdClient == id);
                    }
                }
            }

            if(trainings == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            result.Data = await trainings.ProjectTo<TrainingListOutput>(_mapper.ConfigurationProvider).ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "No trainings found.";
            }
            else
            {
                var _trainings = result.Data;
                foreach (TrainingListOutput trainingOutput in _trainings)
                {
                    //foreach (TrainingExerciseOutput trainingExerciseOutput in trainingOutput.TrainingExercises.ToList())
                    //{
                    //    if (trainingExerciseOutput != null)
                    //    {
                    //        if (trainingExerciseOutput.Exercise != null)
                    //        {
                    //            var max = await _context.Maximums.Where(x => x.IdExercise == trainingExerciseOutput.Exercise.Id && x.IdClient == trainingOutput.IdClient).FirstOrDefaultAsync();
                    //            if (max != null)
                    //            {
                    //                MaximumOutput maxi = new MaximumOutput();
                    //                maxi.Max = max.Max;
                    //                trainingExerciseOutput.Maximum = maxi;
                    //            }

                    //        }

                    //    }
                    //}
                    var client = await _context.Clients.Where(x => x.Id == trainingOutput.IdClient).FirstOrDefaultAsync();
                    if (client != null)
                    {
                        trainingOutput.ClientName = client.Name;
                        trainingOutput.ClientSurname = client.Surname;
                        var clientAcc = await _context.Users.Where(x => x.Id == client.Id).FirstOrDefaultAsync();
                        if (clientAcc != null)
                        {
                            trainingOutput.ClientAvatar = clientAcc.Avatar;
                        }

                        //BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                        //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(client.Id);
                        //if (await blobContainerClient.ExistsAsync() == false)
                        //{
                        //    trainingOutput.ClientAvatar = null;
                        //}
                        //else
                        //{
                        //    BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                        //    if (await blobClient.ExistsAsync() == false)
                        //    {
                        //        trainingOutput.ClientAvatar = null;
                        //    }
                        //    else
                        //    {
                        //        trainingOutput.ClientAvatar = blobClient.Uri.AbsoluteUri;
                        //    }
                        //}
                    }
                    if (role == UserRole.Client)
                    {
                        trainingOutput.Note = null;
                    }
                }
                result.Data = _trainings;
            }
            return result;
        }

        public async Task<Result<TrainingOutput>> GetTraining(string userId, UserRole role, int trainingId)
        {
            var result = new Result<TrainingOutput>();

            IQueryable training = null;

            if (role == UserRole.Coach)
            {
                training = _context.Trainings.Where(x => x.IdCoach == userId && x.Id == trainingId);
            }
            else if (role == UserRole.Client)
            {
                training = _context.Trainings.Where(x => x.IdClient == userId && x.Id == trainingId);
            }
            result.Data = await training.ProjectTo<TrainingOutput>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't find training with given id";
            }
            else
            {
                foreach (TrainingExerciseOutput trainingExerciseOutput in result.Data.TrainingExercises)
                {
                    if (trainingExerciseOutput != null)
                    {
                        if (trainingExerciseOutput.Exercise != null)
                        {
                            var max = await _context.Maximums.Where(x => x.IdExercise == trainingExerciseOutput.Exercise.Id && x.IdClient == result.Data.IdClient).FirstOrDefaultAsync();
                            if (max != null)
                            {
                                MaximumOutput maxi = new MaximumOutput();
                                maxi.Max = max.Max;
                                trainingExerciseOutput.Maximum = maxi;
                            }
                        }
                    }
                }

                var client = await _context.Clients.Where(x => x.Id == result.Data.IdClient).FirstOrDefaultAsync();
                if (client != null)
                {
                    result.Data.ClientName = client.Name;
                    result.Data.ClientSurname = client.Surname;
                    var clientAcc = await _context.Users.Where(x => x.Id == result.Data.IdClient).FirstOrDefaultAsync();
                    if (clientAcc != null)
                    {
                        result.Data.ClientAvatar = clientAcc.Avatar;
                    }
                    //BlobServiceClient blobServiceClient = new BlobServiceClient(azureConnectionString);
                    //BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(client.Id);
                    //if (await blobContainerClient.ExistsAsync() == false)
                    //{
                    //    result.Data.ClientAvatar = null;
                    //}
                    //else
                    //{
                    //    BlobClient blobClient = blobContainerClient.GetBlobClient("avatar.jpg");
                    //    if (await blobClient.ExistsAsync() == false)
                    //    {
                    //        result.Data.ClientAvatar = null;
                    //    }
                    //    else
                    //    {
                    //        result.Data.ClientAvatar = blobClient.Uri.AbsoluteUri;
                    //    }
                    //}
                }
            }
            return result;
        }
    }
}
