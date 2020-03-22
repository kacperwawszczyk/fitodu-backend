using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fitodu.Service.Models
{
    public class CreateUserInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [RegularExpression(@"(?i)^(?=[A-Z0-9][A-Z0-9@._%+-]{5,253}$)[A-Z0-9._%+-]{1,64}@(?:(?=[A-Z0-9-]{1,63}\.)[A-Z0-9]+(?:-[A-Z0-9]+)*\.){1,8}[A-Z]{2,63}$", ErrorMessage = "EmailFormatError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public string AdminId { get; set; }
    }
}