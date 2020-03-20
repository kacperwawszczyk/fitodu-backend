using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Service.Models.Exercise
{
    public class UpdateExerciseInput
    {
        [Required]
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Archived { get; set; } = false;
    }
}
