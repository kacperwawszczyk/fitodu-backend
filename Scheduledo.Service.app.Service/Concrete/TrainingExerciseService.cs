using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.TrainingExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class TrainingExerciseService : ITrainingExerciseService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public TrainingExerciseService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ICollection<TrainingExercise>>> GetTrainingsExercises(int idTraining)
        {
            var result = new Result<ICollection<TrainingExercise>>();

            var trainingExercises = await _context.TrainingExercises.Where(x => x.IdTraining == idTraining).ToListAsync();
        
            if(trainingExercises != null)
            {
                result.Data = trainingExercises;
            }
            else
            {
                result.Error = ErrorType.NotFound; //może inny?
            }
            return result;
        }

        public async Task<Result> AddTrainingExercise(TrainingExerciseInput trainingExerciseInput)
        {
            var result = new Result();


            TrainingExercise _trainingExercise = new TrainingExercise();
            _trainingExercise.IdExercise = trainingExerciseInput.IdExercise;
            _trainingExercise.IdTraining = trainingExerciseInput.IdTraining;
            _trainingExercise.Repetitions = trainingExerciseInput.Repetitions;
            _trainingExercise.Description = trainingExerciseInput.Description;
            _trainingExercise.Time = trainingExerciseInput.Time;
            _trainingExercise.TrainerNote = trainingExerciseInput.TrainerNote;

            _context.TrainingExercises.Add(_trainingExercise);

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

        public async Task<Result> EditTrainingExercise(TrainingExercise trainingExercise)
        {
            var result = new Result();

            var existingTrainingExercise = await _context.TrainingExercises.Where
                (x=> x.IdTrainingExercise == trainingExercise.IdTrainingExercise).FirstOrDefaultAsync();

            if(existingTrainingExercise == null)
            {
                result.Error = ErrorType.NotFound; //może inny?
                result.ErrorMessage = "This exercise does not exist in the database";
                return result;
            }

            existingTrainingExercise.IdExercise = trainingExercise.IdExercise;
            existingTrainingExercise.Repetitions = trainingExercise.Repetitions;
            existingTrainingExercise.Time = trainingExercise.Time;
            existingTrainingExercise.TrainerNote = trainingExercise.TrainerNote;
            existingTrainingExercise.Description = trainingExercise.Description;


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

        public async Task<Result> DeleteTrainingExercise(TrainingExercise trainingExercise)
        {
            var result = new Result();

            var existingTrainingExercise = await _context.TrainingExercises.Where
                (x => x.IdTrainingExercise == trainingExercise.IdTrainingExercise).FirstOrDefaultAsync();

            if (existingTrainingExercise == null)
            {
                result.Error = ErrorType.NotFound; //może inny?
                result.ErrorMessage = "This exercise does not exist in the database";
                return result;
            }

            _context.TrainingExercises.Remove(existingTrainingExercise);

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
