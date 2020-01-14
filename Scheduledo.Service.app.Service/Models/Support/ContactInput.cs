using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Scheduledo.Service.Models
{
    public class ContactInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Message { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}
