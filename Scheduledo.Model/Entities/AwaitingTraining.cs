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
        public string IdCoach { get; set; }
        [Required]
        public string IdClient { get; set; }
        public DateTime Date { get; set; }
    }
}
