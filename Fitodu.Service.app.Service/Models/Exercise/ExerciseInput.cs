using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models.Exercise
{
    public class ExerciseInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [StringLength(30)]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public bool Archived { get; set; } = false;
    }
}
