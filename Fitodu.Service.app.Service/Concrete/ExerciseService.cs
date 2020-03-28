﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Fitodu.Service.Models.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
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



        public async Task<Result<ICollection<ExerciseOutput>>> GetAllExercises(string coachId)
        {
            var result = new Result<ICollection<ExerciseOutput>>();

            var exercises = await _context
                .Exercises.Where(x => x.IdCoach == coachId)
                .Select(x => new ExerciseOutput { 
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Archived = x.Archived
                })
                .ToListAsync();

            if (exercises != null)
            {
                result.Data = exercises;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }

            return result;
        }


        public async Task<Result<int>> CreateExercise(string coachId, ExerciseInput exercise)
        {
            var result = new Result<int>();

            Exercise existingExercise = await _context.Exercises
                .Where(x => x.IdCoach == coachId && x.Name == exercise.Name)
                .FirstOrDefaultAsync();

            if (existingExercise != null) //this coach already has an exercise with given name
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "this coach already has an exercise with given name";
                return result;
            }

            Exercise _ex = new Exercise();
            _ex.IdCoach = coachId;
            _ex.Name = exercise.Name;
            _ex.Description = exercise.Description;
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Exercises.Add(_ex);

                if (await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }
                transaction.Commit();
            }

            result.Data = _ex.Id;
            return result;
        }

        public async Task<Result> EditExercise(string coachId, UpdateExerciseInput exercise)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {
                Exercise existingExercise = await _context.Exercises
                .Where(x => x.Id == exercise.Id && x.IdCoach == coachId)
                .FirstOrDefaultAsync();

                if (existingExercise == null) //this coach does not have an exercise with that Id
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have an exercise with given Id";
                    return result;
                }

                existingExercise.Name = exercise.Name;
                existingExercise.Description = exercise.Description;
                existingExercise.Archived = exercise.Archived;

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


        public async Task<Result> DeleteExercise(string coachId, int exerciseId)
        {
            var result = new Result();

            using (var transaction = _context.Database.BeginTransaction())
            {

                Exercise existingExercise = await _context.Exercises
                .Where(x => x.Id == exerciseId && x.IdCoach == coachId)
                .FirstOrDefaultAsync();

                if (existingExercise == null) //this coach does not have an exercise with that Id
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "this coach does not have an exercise with given Id";
                    return result;
                }

                _context.Exercises.Remove(existingExercise);
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

        public async Task<Result<ICollection<ExerciseOutput>>> GetArchivedExercises(string coachId)
        {
            var result = new Result<ICollection<ExerciseOutput>>();

            var exercises = await _context.Exercises
                .Where(x => x.IdCoach == coachId && x.Archived == true)
                .Select(x => new ExerciseOutput
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Archived = x.Archived
                }).ToListAsync();

            if (exercises != null)
            {
                result.Data = exercises;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }

            return result;
        }
        public async Task<Result<ICollection<ExerciseOutput>>> GetNotArchivedExercises(string coachId)
        {
            var result = new Result<ICollection<ExerciseOutput>>();

            var exercises = await _context.Exercises
                .Where(x => x.IdCoach == coachId && x.Archived == false)
                .Select(x => new ExerciseOutput
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Archived = x.Archived
                })
                .ToListAsync();

            if (exercises != null)
            {
                result.Data = exercises;
            }
            else
            {
                result.Error = ErrorType.NotFound;
            }

            return result;
        }

        public async Task<Result<ExerciseOutput>> GetExerciseById(string coachId, int exerciseId)
        {
            var result = new Result<ExerciseOutput>();
            ExerciseOutput exercise = await _context.Exercises
               .Where(x => x.IdCoach == coachId && x.Id == exerciseId)
               .Select(x => new ExerciseOutput
               {
                   Id = x.Id,
                   Description = x.Description,
                   Name = x.Name,
                   Archived = x.Archived
               }).FirstOrDefaultAsync();
            if(exercise == null)
            {
                result.Error = ErrorType.NotFound;
            }
            else
            {
                result.Data = exercise;
            }
            return result;
        }
    }
}
