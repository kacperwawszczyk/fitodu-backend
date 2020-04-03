using Fitodu.Core.Enums;
using Fitodu.Service.Models;
using Fitodu.Service.Models.AwaitingTraining;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IAwaitingTrainingService
    {
        Task<Result<ICollection<AwaitingTrainingOutput>>> GetAwaitingTraining(string requesterId, UserRole requesterRole);
        Task<Result<int>> CreateAwaitingTraining(string requesterId, UserRole requesterRole, AwaitingTrainingInput awaitingTrainingInput);
        Task<Result<int>> DeleteAwaitingTraining(string requesterId, UserRole requesterRole, int id, bool? accept);
    }
}
