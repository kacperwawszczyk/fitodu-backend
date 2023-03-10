using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class TrainingResultService : ITrainingResultService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly ITrainingService _trainingService;
        public TrainingResultService(Context context, IMapper mapper,
            ITrainingService trainingService)
        {
            _context = context;
            _mapper = mapper;
            _trainingService = trainingService;
        }


        public async Task<Result<ICollection<TrainingResult>>> GetTrainingsResults(int idTraining, string userId, UserRole role)
        {
            var result = new Result<ICollection<TrainingResult>>();
            if (role == UserRole.Coach)
            {
                var trainingsCoachResult = await _trainingService.GetTrainingsCoach(idTraining);

                //check if this training is related to the requesting coach
                if (trainingsCoachResult.Data != userId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This coach doesn't belong to this training";
                    return result;
                }
            }
            else if (role == UserRole.Client)
            {
                var trainingsCoachResult = await _trainingService.GetTrainingsClient(idTraining);

                //check if this training is related to the requesting client
                if (trainingsCoachResult.Data != userId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This client doesn't belong to this training";
                    return result;
                }
            }

            var trainingResults = await _context.TrainingResults.Where(x => x.IdTraining == idTraining).ToListAsync();

            if (trainingResults != null)
            {
                result.Data = trainingResults;
            }
            else
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Couldn't training result with given id";
            }
            return result;
        }

        public async Task<Result> AddTrainingResult(string coachId, TrainingResultInput trainingResultInput)
        {
            var result = new Result();

            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingResultInput.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This coach doesn't belong to this training";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingResult = await _context.TrainingResults.Where(x => x.IdTrainingExercise == trainingResultInput.IdTrainingExercise).FirstOrDefaultAsync();

                if (existingResult != null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "This exercise already has a result in this training";
                    return result;
                }

                var trainingResult = new TrainingResult();
                trainingResult.IdTraining = trainingResultInput.IdTraining;
                trainingResult.IdTrainingExercise = trainingResultInput.IdTrainingExercise;
                trainingResult.IdExercise = trainingResultInput.IdExercise;
                trainingResult.Note = trainingResultInput.Note;
                trainingResult.Repetitions = trainingResultInput.Repetitions;
                trainingResult.Time = trainingResultInput.Time;
                trainingResult.Description = trainingResultInput.Description;

                _context.TrainingResults.Add(trainingResult);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }


        public async Task<Result> EditTrainingResult(string coachId, TrainingResult trainingResult)
        {
            var result = new Result();
            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingResult.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This coach doesn't belong to this training";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingResult = await _context.TrainingResults.Where(x => x.IdTrainingExercise == trainingResult.IdTrainingExercise).FirstOrDefaultAsync();
                var _tmpExistingResult = existingResult;
                if (existingResult == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "This exercise does not have a result in this training";
                    return result;
                }

                existingResult.Repetitions = trainingResult.Repetitions;
                existingResult.Time = trainingResult.Time;
                existingResult.Description = trainingResult.Description;
                existingResult.Note = trainingResult.Note;

                _context.TrainingResults.Update(existingResult);
                if (await _context.SaveChangesAsync() == 0)
                {
                    if (existingResult.Equals(_tmpExistingResult))
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Error = ErrorType.InternalServerError;
                        result.ErrorMessage = "Couldn't save changes to the database";
                    }
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }



        public async Task<Result> DeleteTrainingResult(string coachId, int trainingResultId)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingResult = await _context.TrainingResults.Where(x => x.Id == trainingResultId).FirstOrDefaultAsync();

                if (existingResult == null)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = " Result with this id does not exist";
                    return result;
                }

                var trainingsCoachResult = await _trainingService.GetTrainingsCoach(existingResult.IdTraining);

                //check if this training is related to the requesting coach
                if (trainingsCoachResult.Data != coachId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This coach doesn't belong to this training";
                    return result;
                }

                _context.TrainingResults.Remove(existingResult);
                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transaction.Commit();
            }
            return result;
        }
    }
}
