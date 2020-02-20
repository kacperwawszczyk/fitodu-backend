using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class TrainingResult
    {
        [Required]
        public int IdExercise { get; set; }
        [Required]
        public int IdTraining { get; set; }
        public int Repetitions { get; set; }
        public TimeSpan? Time { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
