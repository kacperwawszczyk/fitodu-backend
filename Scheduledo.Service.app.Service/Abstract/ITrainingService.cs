using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface ITrainingService
    {
        Task<Result<string>> GetTrainingsCoach(int idTraining);
        Task<Result<ICollection<Training>>> GetCoachsTrainings(string idCoach);
        Task<Result<ICollection<Training>>> GetClientsTrainings(string idClient);
    }
}
