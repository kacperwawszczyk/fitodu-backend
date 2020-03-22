using System.Threading.Tasks;
using Fitodu.Service.Models;

namespace Fitodu.Service.Abstract
{
    public interface ISupportService
    {
        Task<Result> Contact(ContactInput model);
        Task<Result> FeatureRequest(FeatureRequestInput model);
    }
}
