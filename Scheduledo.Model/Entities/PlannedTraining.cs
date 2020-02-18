using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class PlannedTraining
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdTrainer { get; set; }
        [Required]
        public int IdTraining { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
    }
}
