using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Training
    {
        [Key]
        public int Id { get; set; }
        public string IdClient { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        public string IdCoach { get; set; }
        public string Name { get; set; }
        [ForeignKey("IdCoach")]
        public virtual Coach Coach { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
        public virtual ICollection<TrainingExercise> TrainingExercises { get; set; }
        public virtual ICollection<TrainingResult> TrainingResults { get; set; }
        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
