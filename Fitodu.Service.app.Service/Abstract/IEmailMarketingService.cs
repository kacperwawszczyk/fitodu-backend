using System.Threading.Tasks;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;

namespace Fitodu.Service.Abstract
{
    public interface IEmailMarketingService
    {
        Task<Result<int>> AddOrUpdate(User user);
        Task<Result<int>> Subscribe(User user);
        Task<bool> Unsubscribe(int contactId);
    }
}
