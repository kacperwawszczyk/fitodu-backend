using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface ISummaryService
    {
        Task<Result<ICollection<SummaryOutput>>> GetAllSummaries (string IdCoach, string IdClient);
        Task<Result<SummaryOutput>> GetClientSummary (string IdCoach, string IdClient, int Id);
        Task<Result> CreateSummary (string IdCoach, SummaryInput sum);
        Task<Result> UpdateSummary (string IdCoach, SummaryInput sum);
        Task<Result> DeleteSummary (string IdCoach, string IdClient, int Id);
    }
}
