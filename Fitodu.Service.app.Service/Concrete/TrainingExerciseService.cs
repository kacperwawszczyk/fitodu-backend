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
using AutoMapper.QueryableExtensions;
using Fitodu.Service.Models.TrainingExercise;

namespace Fitodu.Service.Concrete
{
    public class TrainingExerciseService : ITrainingExerciseService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly ITrainingService _trainingService;

        public TrainingExerciseService(Context context, IMapper mapper,
            ITrainingService trainingService)
        {
            _context = context;
            _mapper = mapper;
            _trainingService = trainingService;
        }

        public async Task<Result<ICollection<TrainingExerciseOutput>>> GetTrainingsExercises(int idTraining, string userId, UserRole role)
        {
            var result = new Result<ICollection<TrainingExerciseOutput>>();
            string clientId = "";
            if (role == UserRole.Coach)
            {
                var trainingsCoachResult = await _trainingService.GetTrainingsCoach(idTraining);

                //check if this training is related to the requesting coach
                if (trainingsCoachResult.Data != userId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This coach isn't related with the training or training/coach doesn't exist";
                    return result;
                }

                var training = await _context.Trainings.Where(x => x.Id == idTraining).FirstOrDefaultAsync();
                clientId = training.IdClient;
            }
            else if (role == UserRole.Client)
            {
                var trainingsCoachResult = await _trainingService.GetTrainingsClient(idTraining);

                //check if this training is related to the requesting client
                if (trainingsCoachResult.Data != userId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This client isn't related with the training or training/client doesn't exist";
                    return result;
                }
                clientId = userId;
            }

            IQueryable trainingResults = _context.TrainingExercises.Where(x => x.IdTraining == idTraining);

            result.Data = await trainingResults.ProjectTo<TrainingExerciseOutput>(_mapper.ConfigurationProvider).ToListAsync();

            if (result.Data == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Exercises not found.";
            }
            else
            {
                foreach (TrainingExerciseOutput trainingExerciseOutput in result.Data)
                {
                    var max = await _context.Maximums.Where(x => x.IdExercise == trainingExerciseOutput.Exercise.Id && x.IdClient == clientId).FirstOrDefaultAsync();
                    if (max != null)
                    {
                        MaximumOutput maxi = new MaximumOutput();
                        maxi.Max = max.Max;
                        trainingExerciseOutput.Maximum = maxi;
                    }
                }
            }
            return result;
        }

        public async Task<Result<int>> AddTrainingExercise(string coachId, TrainingExerciseInput trainingExerciseInput)
        {
            var result = new Result<int>();

            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExerciseInput.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This coach isn't related with the training or training/coach doesn't exist";
                return result;
            }

            TrainingExercise _trainingExercise = new TrainingExercise();
            _trainingExercise.IdExercise = trainingExerciseInput.IdExercise;
            _trainingExercise.IdTraining = trainingExerciseInput.IdTraining;
            if (trainingExerciseInput.Repetitions < 0)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "repetitions cannot be set to a value lesser than 0";
                return result;
            }
            _trainingExercise.Repetitions = trainingExerciseInput.Repetitions;
            _trainingExercise.Time = trainingExerciseInput.Time;

            _context.TrainingExercises.Add(_trainingExercise);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Couldn't save changes to the database.";
            }
            result.Data = _trainingExercise.Id;
            return result;
        }

        public async Task<Result> EditTrainingExercise(string coachId, UpdateTrainingExerciseInput trainingExercise)
        {
            var result = new Result();

            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExercise.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                result.ErrorMessage = "This coach isn't related with the training or training/coach doesn't exist";
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingTrainingExercise = await _context.TrainingExercises.Where
                (x => x.Id == trainingExercise.Id).FirstOrDefaultAsync();

                var _tmpExisitingTrainingExercise = existingTrainingExercise;

                if (existingTrainingExercise == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "This exercise does not exist in the database";
                    return result;
                }

                existingTrainingExercise.IdExercise = trainingExercise.IdExercise;
                if (trainingExercise.Repetitions < 0)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "repetitions cannot be set to a value lesser than 0";
                    return result;
                }
                existingTrainingExercise.Repetitions = trainingExercise.Repetitions;
                existingTrainingExercise.Time = trainingExercise.Time;
                if (trainingExercise.RepetitionsResult < 0)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "repetitions result cannot be set to a value lesser than 0";
                    return result;
                }
                existingTrainingExercise.RepetitionsResult = trainingExercise.RepetitionsResult;
                existingTrainingExercise.TimeResult = trainingExercise.TimeResult;

                if (existingTrainingExercise.Equals(_tmpExisitingTrainingExercise))
                {
                    transaction.Commit();
                    return result;
                }
                else if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }


        public async Task<Result> DeleteTrainingExercise(string coachId, int trainingExerciseId)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingTrainingExercise = await _context.TrainingExercises.Where
                (x => x.Id == trainingExerciseId).FirstOrDefaultAsync();

                if (existingTrainingExercise == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "This exercise does not exist in the database";
                    return result;
                }

                var trainingsCoachResult = await _trainingService.GetTrainingsCoach(existingTrainingExercise.IdTraining);

                //check if this training is related to the requesting coach
                if (trainingsCoachResult.Data != coachId)
                {
                    result.Error = ErrorType.Forbidden;
                    result.ErrorMessage = "This coach isn't related with the training or training/coach doesn't exist";
                    return result;
                }

                _context.TrainingExercises.Remove(existingTrainingExercise);
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

        //TODO: Usunąć jak na pewno nie będzie potrzebne
        //public async Task<Result> DeleteTrainingsExercises(int idTraining)
        //{
        //    var result = new Result();

        //    var trainingsExercises = await _context.TrainingExercises.Where
        //        (x => x.IdTraining == idTraining).ToListAsync();

        //    if (trainingsExercises.Count == 0)
        //    {
        //        result = new Result(true);
        //        return result;
        //    }

        //    _context.TrainingExercises.RemoveRange(trainingsExercises);

        //    if (await _context.SaveChangesAsync() > 0)
        //    {
        //        result = new Result(true);
        //    }
        //    else
        //    {
        //        result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
        //    }
        //    return result;
        //}
    }
}
