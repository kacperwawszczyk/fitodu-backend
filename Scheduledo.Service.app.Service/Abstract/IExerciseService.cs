using Scheduledo.Model.Entities;
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
        Task<Result> CreateExercise(ExerciseInput exercise);
        Task<Result> EditExercise(Exercise exercise);
        Task<Result> DeleteExercise(Exercise exercise);
    }
}
