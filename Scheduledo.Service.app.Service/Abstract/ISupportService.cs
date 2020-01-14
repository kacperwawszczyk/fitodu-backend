using System.Threading.Tasks;
using Scheduledo.Service.Models;

namespace Scheduledo.Service.Abstract
{
    public interface ISupportService
    {
        Task<Result> Contact(ContactInput model);
        Task<Result> FeatureRequest(FeatureRequestInput model);
    }
}
