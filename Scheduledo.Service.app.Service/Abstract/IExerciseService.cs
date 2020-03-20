﻿using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Exercise;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IExerciseService
    {
        Task<Result<ICollection<Exercise>>> GetAllExercises(string coachId);
        Task<Result> CreateExercise(string coachId, ExerciseInput exercise);
        Task<Result> EditExercise(string coachId, UpdateExerciseInput exercise);
        Task<Result> DeleteExercise(string coachId, int exerciseId);
        Task<Result<ICollection<Exercise>>> GetArchivedExercises(string coachId);
        Task<Result<ICollection<Exercise>>> GetNotArchivedExercises(string coachId);
    }
}
