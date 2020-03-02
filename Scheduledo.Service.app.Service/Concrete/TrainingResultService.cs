using AutoMapper;
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
    public class TrainingResultService : ITrainingResultService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        public TrainingResultService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<ICollection<TrainingResult>>> GetTrainingsResults(int idTraining)
        {
            var result = new Result<ICollection<TrainingResult>>();

            var trainingResults = await _context.TrainingResults.Where(x => x.IdTraining == idTraining).ToListAsync();

            if (trainingResults != null)
            {
                result.Data = trainingResults;
            }
            else
            {
                result.Error = ErrorType.NotFound; //może inny?
            }
            return result;
        }

        public async Task<Result> AddTrainingResult(TrainingResultInput trainingResultInput)
        {
            var result = new Result();

            var existingResult = await _context.TrainingResults.Where(x => x.IdTrainingExercise == trainingResultInput.IdTrainingExercise).FirstOrDefaultAsync();

            if (existingResult != null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This exercise already has a result in this training";
                return result;
            }

            var trainingResult = new TrainingResult();
            trainingResult.IdTraining = trainingResultInput.IdTraining ;
            trainingResult.IdTrainingExercise = trainingResultInput.IdTrainingExercise;
            trainingResult.IdExercise = trainingResultInput.IdExercise;
            trainingResult.Note = trainingResultInput.Note;
            trainingResult.Repetitions = trainingResultInput.Repetitions;
            trainingResult.Time = trainingResultInput.Time;
            trainingResult.Description = trainingResultInput.Description;

            _context.TrainingResults.Add(trainingResult);
            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;

        }

        public async Task<Result> EditTrainingResult(TrainingResult trainingResult)
        {
            var result = new Result();

            var existingResult = await _context.TrainingResults.Where(x => x.IdTrainingExercise == trainingResult.IdTrainingExercise).FirstOrDefaultAsync();

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
            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;
        }

        public async Task<Result> DeleteTrainingResult(TrainingResult trainingResult)
        {
            var result = new Result();

            var existingResult = await _context.TrainingResults.Where(x => x.IdTrainingResult == trainingResult.IdTrainingResult).FirstOrDefaultAsync();

            if (existingResult == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "This exercise does not have a result in this training";
                return result;
            }

            _context.TrainingResults.Remove(existingResult);
            if (await _context.SaveChangesAsync() > 0)
            {
                result = new Result(true);
            }
            else
            {
                result.Error = ErrorType.BadRequest; //może być co innego, może dodać nowy?
            }
            return result;
        }
    }
}
