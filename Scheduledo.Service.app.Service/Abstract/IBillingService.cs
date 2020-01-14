using System.Threading.Tasks;
using Scheduledo.Core.Enums;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Models;

namespace Scheduledo.Service.Abstract
{
    public interface IBillingService
    {
        Task<Result> WebhookEvent(string body, string signature);
        Task<Result<string>> Subscribe(string userId, PricingPlan plan);
        Task<Result> Change(string userId, PricingPlan plan);
        Task<Result<string>> Update(string userId);
        Task<Result> Unsubscribe(string userId);
        Task UpdateCustomer(User admin);
    }
}
