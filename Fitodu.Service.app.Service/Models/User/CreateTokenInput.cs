using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class CreateTokenInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Password { get; set; }
    }
}
