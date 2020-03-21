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
        Task<Result> AddTrainingExercise(TrainingExerciseInput trainingExerciseInput, string coachId);
        Task<Result> EditTrainingExercise(TrainingExercise trainingExercise, string coachId);
        Task<Result> DeleteTrainingExercise(int trainingExerciseId, string coachId);
       // Task<Result> DeleteTrainingsExercises(int idTraining);
    }
}
