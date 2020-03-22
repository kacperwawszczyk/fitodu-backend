using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class AwaitingTraining
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string IdCoach { get; set; }
        [Required]
        public string IdClient { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
    }
}
