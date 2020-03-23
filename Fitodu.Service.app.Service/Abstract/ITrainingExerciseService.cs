﻿using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.TrainingExercise;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
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