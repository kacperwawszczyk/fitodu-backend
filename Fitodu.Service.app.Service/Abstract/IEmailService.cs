using System.Threading.Tasks;
using Fitodu.Service.Models;

namespace Fitodu.Service.Abstract
{
    public interface IEmailService
    {
        Task<EmailResult> Send(EmailInput model);
    }
}
