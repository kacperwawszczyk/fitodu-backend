using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models.TrainingExercise
{
    public class UpdateTrainingExerciseInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdExercise { get; set; }
        [Required]
        public int IdTraining { get; set; }
        public int Repetitions { get; set; }
        public TimeSpan? Time { get; set; }
        public int RepetitionsResult { get; set; }
        public TimeSpan? TimeResult { get; set; }
    }
}
