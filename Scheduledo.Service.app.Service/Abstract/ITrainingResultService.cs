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
        Task<Result<ICollection<TrainingResult>>> GetTrainingsResults(int idTraining);
        Task<Result> AddTrainingResult(TrainingResultInput trainingResult);
        Task<Result> EditTrainingResult(TrainingResult trainingResult);
        Task<Result> DeleteTrainingResult(TrainingResult trainingResult);
    }
}
