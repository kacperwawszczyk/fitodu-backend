using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class UpdateUserInput : CreateUserInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Id { get; set; }

        public new string Password { get; set; }
    }
}