using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;

namespace Fitodu.Service.Abstract
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
