using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class TrainingExercise
    {
        [Required]
        public int IdExercise { get; set; }
        [Required]
        public int IdTraining { get; set; }
        public int Repeats { get; set; }
        public TimeSpan? Time { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string TrainerNote { get; set; }
    }
}
