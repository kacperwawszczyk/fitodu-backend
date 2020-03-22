using System.Linq;
using System.Threading.Tasks;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fitodu.Service.Concrete
{
    public class SendGridService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _address;
        private readonly string _displayName;

        public SendGridService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _address = configuration["SendGrid:Address"];
            _displayName = configuration["SendGrid:DisplayName"];
        }

        public async Task<EmailResult> Send(EmailInput model)
        {
            var result = new EmailResult();

            var client = new SendGridClient(_apiKey);

            EmailAddress from;
            if (!string.IsNullOrEmpty(model.FromAddress))
            {
                from = new EmailAddress(model.FromAddress, model.FromName);
            }
            else
            {
                from = new EmailAddress(_address, _displayName);   
            }

            var to = new EmailAddress(model.To);

            var mail = MailHelper.CreateSingleEmail(
                from, to, model.Subject, model.TextBody, model.HtmlBody);

            if (model.Attachments.Any())
            {
                foreach (var attachment in model.Attachments)
                {
                    mail.AddAttachment(attachment.FileName, attachment.Content,
                        attachment.Type, attachment.Disposition, attachment.ContentId);
                }
            }

            if (model.Substitutions.Any())
            {
                mail.AddSubstitutions(model.Substitutions);
            }

            if (!string.IsNullOrEmpty(model.TemplateId))
            {
                mail.TemplateId = model.TemplateId;
            }

            var response = await client.SendEmailAsync(mail);

            result.Code = response.StatusCode;

            return result;
        }
    }
}
