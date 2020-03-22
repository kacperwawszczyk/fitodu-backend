using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fitodu.Service.Models
{
    public class SignOutInput
    {
        [Required(ErrorMessage = "Required")]
        public string Token { get; set; }
    }
}