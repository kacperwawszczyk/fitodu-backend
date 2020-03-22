using Nexmo.Api;

namespace Fitodu.Service.Abstract
{
    public interface ITextMessageService
    {
        SMS.SMSResponse Send(string to, string text);
    }
}
