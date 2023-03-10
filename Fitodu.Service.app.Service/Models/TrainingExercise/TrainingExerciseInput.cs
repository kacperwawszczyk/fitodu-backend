using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models
{
    public class TrainingExerciseInput
    {
        [Required]
        public int IdExercise { get; set; }
        [Required]
        public int IdTraining { get; set; }
        public int Repetitions { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
