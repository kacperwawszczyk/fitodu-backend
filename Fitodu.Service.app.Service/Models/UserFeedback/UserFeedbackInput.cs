using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class UserFeedbackInput
    {
        [Required]
        public string URL { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
