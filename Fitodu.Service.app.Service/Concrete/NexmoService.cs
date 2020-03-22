//using Fitodu.Core.Helpers;
//using Fitodu.Service.Abstract;
//using Microsoft.Extensions.Configuration;
//using Nexmo.Api;
//using Nexmo.Api.Request;

//namespace Fitodu.Service.Concrete
//{
//    public class NexmoService : ITextMessageService
//    {
//        private readonly string _apiKey;
//        private readonly string _apiSecret;
//        private readonly string _from;

//        public NexmoService(IConfiguration configuration)
//        {
//            _apiKey = configuration["Nexmo:ApiKey"];
//            _apiSecret = configuration["Nexmo:ApiSecret"];
//            _from = configuration["Nexmo:From"];
//        }

//        public SMS.SMSResponse Send(string to, string text)
//        {
//            var client = new Client(new Credentials
//            {
//                ApiKey = _apiKey,
//                ApiSecret = _apiSecret
//            });

//            var request = new SMS.SMSRequest
//            {
//                from = _from,
//                to = to,
//                text = text
//            };

//            if (Language.CurrentCulture != Language.DefaultCulture)
//            {
//                request.type = "unicode";
//            }

//            var results = client.SMS.Send(request);

//            return results;
//        }
//    }
//}