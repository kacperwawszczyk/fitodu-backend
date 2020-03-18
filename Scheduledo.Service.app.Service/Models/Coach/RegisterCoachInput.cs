using System.ComponentModel.DataAnnotations;

namespace Scheduledo.Service.Models
{
    public class RegisterCoachInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Surname { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        //[RegularExpression("^[a-z0-9]*$", ErrorMessage = "UrlFormatError")]
        //public string Url { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [RegularExpression(@"(?i)^(?=[A-Z0-9][A-Z0-9@._%+-]{5,253}$)[A-Z0-9._%+-]{1,64}@(?:(?=[A-Z0-9-]{1,63}\.)[A-Z0-9]+(?:-[A-Z0-9]+)*\.){1,8}[A-Z]{2,63}$", ErrorMessage = "EmailFormatError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Password { get; set; }

        //public string PhoneNumber { get; set; }

        public string FullName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
    }
}