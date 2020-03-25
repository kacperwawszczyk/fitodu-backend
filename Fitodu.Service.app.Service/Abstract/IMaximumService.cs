using Fitodu.Model.Entities;
using Fitodu.Service.Models;
using Fitodu.Service.Models.Maximum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IMaximumService
    {
        Task<Result<ICollection<MaximumOutput>>> GetAllMaximums(string IdCoach, string IdClient);
        Task<Result<MaximumOutput>> GetClientMaximum(string IdCoach, string IdClient, int IdExercise);
        Task<Result> CreateMaximum(string IdCoach, MaximumInput max);
        Task<Result> UpdateMaximum(string IdCoach, MaximumInput max);
        Task<Result> DeleteMaximum(string IdCoach, string IdClient, int IdExercise);
    }
}
