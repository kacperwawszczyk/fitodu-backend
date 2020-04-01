using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface ITrainingExerciseService
    {
        Task<Result<ICollection<TrainingExerciseOutput>>> GetTrainingsExercises(int idTraining, string userId, UserRole role);
        Task<Result<int>> AddTrainingExercise(string coachId, TrainingExerciseInput trainingExerciseInput);
        Task<Result> EditTrainingExercise(string coachId, TrainingExercise trainingExercise);
        Task<Result> DeleteTrainingExercise(string coachId, int trainingExerciseId);
       // Task<Result> DeleteTrainingsExercises(int idTraining);
    }
}
