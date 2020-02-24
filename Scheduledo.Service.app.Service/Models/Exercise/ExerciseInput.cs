using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Service.Models.Exercise
{
    public class ExerciseInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string IdCoach { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [StringLength(30)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
