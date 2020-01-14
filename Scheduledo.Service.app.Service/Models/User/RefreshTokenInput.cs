using System.ComponentModel.DataAnnotations;

namespace Scheduledo.Service.Models
{
    public class RefreshTokenInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Token { get; set; }
    }
}
