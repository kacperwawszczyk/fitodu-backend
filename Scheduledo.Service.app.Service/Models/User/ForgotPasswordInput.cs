﻿using System.ComponentModel.DataAnnotations;

namespace Scheduledo.Service.Models
{
    public class ForgotPasswordInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Email { get; set; }
    }
}
