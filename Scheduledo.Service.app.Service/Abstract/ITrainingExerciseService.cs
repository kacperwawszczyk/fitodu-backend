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
        Task<Result<ICollection<TrainingExercise>>> GetTrainingsExercises(int idTraining);
        Task<Result> AddTrainingExercise(TrainingExerciseInput trainingExerciseInput);
        Task<Result> EditTrainingExercise(TrainingExercise trainingExercise);
        Task<Result> DeleteTrainingExercise(TrainingExercise trainingExercise);
       // Task<Result> DeleteTrainingsExercises(int idTraining);
    }
}
