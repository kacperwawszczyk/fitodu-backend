using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;
using Scheduledo.Service.Models.Maximum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
    public interface IMaximumService
    {
        Task<Result<ICollection<Maximum>>> GetAllMaximums(string IdCoach, string IdClient);
        Task<Result<Maximum>> GetClientMaximum(string IdCoach, string IdClient, int IdExercise);
        Task<Result> CreateMaximum(string IdCoach, CreateMaximumInput max);
        Task<Result> UpdateMaximum(string IdCoach, Maximum max);
        Task<Result> DeleteMaximum(string IdCoach, Maximum max);
    }
}
