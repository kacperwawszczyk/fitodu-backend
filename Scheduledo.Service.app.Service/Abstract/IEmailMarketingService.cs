using System.Threading.Tasks;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;

namespace Scheduledo.Service.Abstract
{
    public interface IEmailMarketingService
    {
        Task<Result<int>> AddOrUpdate(User user);
        Task<Result<int>> Subscribe(User user);
        Task<bool> Unsubscribe(int contactId);
    }
}
