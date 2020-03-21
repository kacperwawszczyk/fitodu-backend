using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.TrainingExercise;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface ITrainingExerciseService
    {
        Task<Result<ICollection<TrainingExercise>>> GetTrainingsExercises(int idTraining, string userId, UserRole role);
        Task<Result> AddTrainingExercise(string coachId, TrainingExerciseInput trainingExerciseInput);
        Task<Result> EditTrainingExercise(string coachId, TrainingExercise trainingExercise);
        Task<Result> DeleteTrainingExercise(string coachId, int trainingExerciseId);
       // Task<Result> DeleteTrainingsExercises(int idTraining);
    }
}
