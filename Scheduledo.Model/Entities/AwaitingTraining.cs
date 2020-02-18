using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class AwaitingTraining
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdTrainer { get; set; }
        [Required]
        public int IdTraining { get; set; }
    }
}
