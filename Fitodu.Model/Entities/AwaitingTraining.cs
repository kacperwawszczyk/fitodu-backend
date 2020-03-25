using Fitodu.Core.Enums;
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
        [ForeignKey("IdCoach")]
        public virtual Coach Coach { get; set; }
        [Required]
        public string IdClient { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Required]
        public UserRole Receiver { get; set; }
        [Required]
        public UserRole Sender { get; set; }
    }
}
