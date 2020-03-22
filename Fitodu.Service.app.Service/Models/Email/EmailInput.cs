using System.Collections.Generic;

namespace Fitodu.Service.Models
{
    public class EmailInput
    {
        public string FromAddress { get; set; }
        public string FromName { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string TemplateId { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
        public Dictionary<string, string> Substitutions { get; set; }

        public EmailInput()
        {
            Attachments = new List<EmailAttachment>();
            TemplateId = null;
            Substitutions = new Dictionary<string, string>();
        }
    }
}
