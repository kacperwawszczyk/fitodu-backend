using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class TrainingResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdTrainingExercise { get; set; }
        [ForeignKey("IdTrainingExercise")]
        public virtual TrainingExercise TrainingExercise { get; set; }
        [Required]
        public int IdExercise { get; set; }
        [ForeignKey("IdExercise")]
        public virtual Exercise Exercise { get; set; }
        [Required]
        public int IdTraining { get; set; }
        [ForeignKey("IdTraining")]
        public virtual Training Training { get; set; }
        public int Repetitions { get; set; }
        public TimeSpan? Time { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
