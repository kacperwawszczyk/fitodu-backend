using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models.Maximum
{
    public class MaximumInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string IdClient { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public int IdExercise { get; set; }
        public string Max { get; set; }
    }
}
