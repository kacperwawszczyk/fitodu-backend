using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.Training;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface ITrainingService
    {

        //TODO: Usunąć jak na pewno nie będzie potrzebne
        //Task<Result<ICollection<Training>>> GetCoachsTrainings(string idCoach);
        //Task<Result<ICollection<Training>>> GetClientsTrainings(string idClient);
        //Task<Result<ICollection<Training>>> GetTrainings(string id, UserRole role);
        Task<Result<int>> AddTraining(string coachId, TrainingInput trainingInput);
        Task<Result> EditTraining(string coachId, UpdateTrainingInput trainingInput);
        Task<Result> DeleteTraining(string requesterId, UserRole requesterRole, int trainingId);
        Task<Result<string>> GetTrainingsClient(int idTraining);
        Task<Result<string>> GetTrainingsCoach(int idTraining);
        Task<Result<ICollection<TrainingListOutput>>> GetTrainings(string id, UserRole role, string date, string idClient);
        Task<Result<TrainingOutput>> GetTraining(string userId, UserRole role, int trainingId);
    }
}
