using System.ComponentModel.DataAnnotations;

namespace Scheduledo.Service.Models
{
    public class CreateTokenInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Password { get; set; }
    }
}
