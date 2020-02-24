using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class ExerciseService : IExerciseService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public ExerciseService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task <Result<ICollection<Exercise>>> GetAllExercises(string coachId)
        {
            var result = new Result<ICollection<Exercise>>();

            var exercises = await _context.Exercises.Where(x => x.IdCoach == coachId).ToListAsync();

            if(exercises != null)
            {
                result.Data = exercises;
            }
            else
            {
                result.Error = ErrorType.NoContent; //może inny?
            }

            return result;
        }


        public async Task<Result> CreateExercise(ExerciseInput exercise)
        {
            var result = new Result();

            Exercise existingExercise = await _context.Exercises
                .Where(x => x.IdCoach == exercise.IdCoach && x.Name == exercise.Name)
                .FirstOrDefaultAsync();

            if(existingExercise != null) //this coach already has an exercise with given name
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "this coach already has an exercise with given name";
                return result;
            }

            Exercise _ex = new Exercise();
            _ex.IdCoach = exercise.IdCoach;
            _ex.Name = exercise.Name;
            _ex.Description = exercise.Description;
            _context.Exercises.Add(_ex);
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
