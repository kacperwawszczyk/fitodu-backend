using System.Threading.Tasks;
using Scheduledo.Service.Models;

namespace Scheduledo.Service.Abstract
{
    public interface IEmailService
    {
        Task<EmailResult> Send(EmailInput model);
    }
}
