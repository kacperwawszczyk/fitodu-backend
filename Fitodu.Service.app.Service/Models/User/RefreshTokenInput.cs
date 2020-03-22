using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class RefreshTokenInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Token { get; set; }
    }
}
