using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class TrainingService : ITrainingService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly IClientService _clientService;
            
        public TrainingService(Context context, IMapper mapper, IClientService clientService)
        {
            _context = context;
            _mapper = mapper;
            _clientService = clientService;
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

        public async Task<Result> AddTraining(string coachId, TrainingInput trainingInput)
        {
            var result = new Result();

            var clientsCoach = await _clientService.GetClientCoach(trainingInput.IdClient);
            if(clientsCoach.IsDataNull || clientsCoach.Data.Id != coachId)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This coach does not work with this client";
                return result;
            }

            Training _training = new Training();
            _training.IdClient = trainingInput.IdClient;
            _training.IdCoach = coachId;
            if (trainingInput.Note != null) _training.Note = trainingInput.Note;
            _training.StartDate = trainingInput.StartDate;
            _training.EndDate = trainingInput.EndDate;
            _training.Description = trainingInput.Description;


            _context.Trainings.Add(_training);
            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
            }
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

                if (exisitngTraining == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Training with this Id does not exist in the database";
                    return result;
                }

                exisitngTraining.IdClient = trainingInput.IdClient;
                exisitngTraining.Note = trainingInput.Note;
                exisitngTraining.Description = trainingInput.Description;
                exisitngTraining.StartDate = trainingInput.StartDate;
                exisitngTraining.EndDate = trainingInput.EndDate;

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


        public async Task<Result> DeleteTraining(string coachId, int trainingId)
        {
            var result = new Result();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingTraining = await _context.Trainings.Where(
                x => x.Id == trainingId && x.IdCoach == coachId).FirstOrDefaultAsync();

                if (existingTraining == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "Training with this Id does not exist in the database";
                    return result;
                }

                _context.Trainings.Remove(existingTraining);

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                var trainingsExercises = await _context.TrainingExercises.Where
                (x => x.IdTraining == trainingId).ToListAsync();


                if (trainingsExercises.Count != 0)
                {
                    _context.TrainingExercises.RemoveRange(trainingsExercises);
                    if (await _context.SaveChangesAsync() == 0)
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        return result;
                    }
                }
                transaction.Commit();
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
            }
            return result;
        }

        public async Task<Result<ICollection<Training>>> GetTrainings(string id, UserRole role, string date, string idClient)
        {
            var result = new Result<ICollection<Training>>();

            var trainings = new List<Training>();

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
                            trainings = await _context.Trainings.Where(x => x.IdCoach == id && x.StartDate > workoutDate && x.IdClient == idClient).ToListAsync();
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
                        trainings = await _context.Trainings.Where(x => x.IdClient == id && x.StartDate > workoutDate).ToListAsync();
                    }
                }
                else
                {
                    if (role == UserRole.Coach)
                    {
                        trainings = await _context.Trainings.Where(x => x.IdCoach == id && x.StartDate > workoutDate).ToListAsync();
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = await _context.Trainings.Where(x => x.IdClient == id && x.StartDate > workoutDate).ToListAsync();
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
                            trainings = await _context.Trainings.Where(x => x.IdCoach == id && x.IdClient == idClient).ToListAsync();
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
                        trainings = await _context.Trainings.Where(x => x.IdClient == id).ToListAsync();
                    }
                }
                else
                {
                    if (role == UserRole.Coach)
                    {
                        trainings = await _context.Trainings.Where(x => x.IdCoach == id).ToListAsync();
                    }
                    else if (role == UserRole.Client)
                    {
                        trainings = await _context.Trainings.Where(x => x.IdClient == id).ToListAsync();
                    }
                }
            }

            if (trainings != null)
            {
                result.Data = trainings;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result<Training>> GetTraining(string userId, UserRole role, int trainingId)
        {
            var result = new Result<Training>();

            var training = new Training();

            if (role == UserRole.Coach)
            {
                training = await _context.Trainings.Where(x => x.IdCoach == userId && x.Id == trainingId).FirstOrDefaultAsync();
            }
            else if (role == UserRole.Client)
            {
                training = await _context.Trainings.Where(x => x.IdClient == userId && x.Id == trainingId).FirstOrDefaultAsync();
            }

            if (training != null)
            {
                result.Data = training;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;
        }
    }
}
