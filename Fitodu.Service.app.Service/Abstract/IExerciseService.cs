using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.Exercise;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IExerciseService
    {
        Task<Result<ICollection<ExerciseOutput>>> GetAllExercises(string coachId, string SearchTerm);
        Task<Result<int>> CreateExercise(string coachId, ExerciseInput exercise);
        Task<Result> EditExercise(string coachId, UpdateExerciseInput exercise);
        Task<Result> DeleteExercise(string coachId, int exerciseId);
        Task<Result<ICollection<ExerciseOutput>>> GetArchivedExercises(string coachId, string SearchTerm);
        Task<Result<ICollection<ExerciseOutput>>> GetNotArchivedExercises(string coachId, string SearchTerm);
        Task<Result<ExerciseOutput>> GetExerciseById(string coachId, int exerciseId);
    }
}
