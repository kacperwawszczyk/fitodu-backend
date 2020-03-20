using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Training;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface ITrainingService
    {

        //TODO: Usunąć jak na pewno nie będzie potrzebne
        //Task<Result<ICollection<Training>>> GetCoachsTrainings(string idCoach);
        //Task<Result<ICollection<Training>>> GetClientsTrainings(string idClient);
        //Task<Result<ICollection<Training>>> GetTrainings(string id, UserRole role);
        Task<Result> AddTraining(TrainingInput trainingInput, string idCoach);
        Task<Result> EditTraining(Training training);
        Task<Result> DeleteTraining(Training training);
        Task<Result<string>> GetTrainingsClient(int idTraining);
        Task<Result<string>> GetTrainingsCoach(int idTraining);
        Task<Result<ICollection<Training>>> GetTrainings(string id, UserRole role, string date, string idClient);
        Task<Result<Training>> GetTraining(string userId, UserRole role, int trainingId);
    }
}
