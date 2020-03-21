using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface ITrainingResultService
    {
        Task<Result<ICollection<TrainingResult>>> GetTrainingsResults(int idTraining, string userId, UserRole role);
        Task<Result> AddTrainingResult(string coachId, TrainingResultInput trainingResultInput);
        Task<Result> EditTrainingResult(string coachId, TrainingResult trainingResult);
        Task<Result> DeleteTrainingResult(string coachId, int trainingResultId);
    }
}
