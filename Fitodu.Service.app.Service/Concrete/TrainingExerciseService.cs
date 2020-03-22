using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.TrainingExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result<ICollection<TrainingExercise>>> GetTrainingsExercises(int idTraining, string userId, UserRole role)
        {
            var result = new Result<ICollection<TrainingExercise>>();
            if (role == UserRole.Coach)
            {
                var trainingsCoachResult = await _trainingService.GetTrainingsCoach(idTraining);

                //check if this training is related to the requesting coach
                if (trainingsCoachResult.Data != userId)
                {
                    result.Error = ErrorType.Forbidden;
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
                    return result;
                }
            }

            var trainingResults = await _context.TrainingExercises.Where(x => x.IdTraining == idTraining).ToListAsync();

            if (trainingResults != null)
            {
                result.Data = trainingResults;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }
            return result;
        }

        public async Task<Result> AddTrainingExercise(string coachId, TrainingExerciseInput trainingExerciseInput)
        {
            var result = new Result();

            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExerciseInput.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                return result;
            }

             TrainingExercise _trainingExercise = new TrainingExercise();
            _trainingExercise.IdExercise = trainingExerciseInput.IdExercise;
            _trainingExercise.IdTraining = trainingExerciseInput.IdTraining;
            _trainingExercise.Repetitions = trainingExerciseInput.Repetitions;
            _trainingExercise.Description = trainingExerciseInput.Description;
            _trainingExercise.Time = trainingExerciseInput.Time;
            _trainingExercise.TrainerNote = trainingExerciseInput.TrainerNote;

            _context.TrainingExercises.Add(_trainingExercise);

            if (await _context.SaveChangesAsync() == 0)
            {
                result.Error = ErrorType.InternalServerError;
            }
            return result;
        }

        public async Task<Result> EditTrainingExercise(string coachId, TrainingExercise trainingExercise)
        {
            var result = new Result();

            var trainingsCoachResult = await _trainingService.GetTrainingsCoach(trainingExercise.IdTraining);

            //check if this training is related to the requesting coach
            if (trainingsCoachResult.Data != coachId)
            {
                result.Error = ErrorType.Forbidden;
                return result;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingTrainingExercise = await _context.TrainingExercises.Where
                (x => x.IdTrainingExercise == trainingExercise.IdTrainingExercise).FirstOrDefaultAsync();

                if (existingTrainingExercise == null)
                {
                    result.Error = ErrorType.NotFound;
                    result.ErrorMessage = "This exercise does not exist in the database";
                    return result;
                }

                existingTrainingExercise.IdExercise = trainingExercise.IdExercise;
                existingTrainingExercise.Repetitions = trainingExercise.Repetitions;
                existingTrainingExercise.Time = trainingExercise.Time;
                existingTrainingExercise.TrainerNote = trainingExercise.TrainerNote;
                existingTrainingExercise.Description = trainingExercise.Description;
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


        public async Task<Result> DeleteTrainingExercise(string coachId, int trainingExerciseId)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingTrainingExercise = await _context.TrainingExercises.Where
                (x => x.IdTrainingExercise == trainingExerciseId).FirstOrDefaultAsync();

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
                    return result;
                }

                _context.TrainingExercises.Remove(existingTrainingExercise);
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
