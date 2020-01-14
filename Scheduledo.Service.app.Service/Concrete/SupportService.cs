using System.Net;
using System.Threading.Tasks;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Scheduledo.Service.Concrete
{
    public class SupportService : ISupportService
    {
        private readonly Context _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SupportService> _logger;

        public SupportService(Context context,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<SupportService> logger)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Result> Contact(ContactInput model)
        {
            var result = new Result();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            var response = await _emailService.Send(new EmailInput
            {
                FromAddress = user.Email,
                FromName = user.FullName,
                To = _configuration["SendGrid:Address"],
                Subject = $"[CONTACT] {model.Subject}",
                TextBody = model.Message
            });

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                _logger.LogCritical("Can't send contact message", model, response);
                result.Error = ErrorType.InternalServerError;
            }

            return result;
        }

        public async Task<Result> FeatureRequest(FeatureRequestInput model)
        {
            var result = new Result();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }

            var response = await _emailService.Send(new EmailInput
            {
                FromAddress = user.Email,
                FromName = user.FullName,
                To = _configuration["SendGrid:Address"],
                Subject = $"[FEATURE REQUEST] {model.Name}",
                TextBody = model.Description
            });

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                _logger.LogCritical("Can't send feature request", model, response);
                result.Error = ErrorType.InternalServerError;
            }

            return result;
        }
    }
}
