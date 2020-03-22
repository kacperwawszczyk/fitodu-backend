using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class ResetPasswordInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string ResetToken { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string NewPassword { get; set; }
    }
}
