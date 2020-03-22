using System.Net;

namespace Fitodu.Service.Models
{
    public class EmailResult
    {
        public HttpStatusCode Code { get; set; }
        public string Errors { get; set; }
    }
}
