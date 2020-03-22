using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class CreateSuperTokenInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Email { get; set; }
    }
}
