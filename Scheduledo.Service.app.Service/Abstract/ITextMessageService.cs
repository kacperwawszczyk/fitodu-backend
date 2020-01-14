using Nexmo.Api;

namespace Scheduledo.Service.Abstract
{
    public interface ITextMessageService
    {
        SMS.SMSResponse Send(string to, string text);
    }
}
