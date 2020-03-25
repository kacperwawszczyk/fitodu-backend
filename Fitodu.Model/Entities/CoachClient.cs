using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class CoachClient
    {
        [Required]
        public string IdClient { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        [Required]
        public string IdCoach { get; set; }
        [ForeignKey("IdCoach")]
        public virtual Coach Coach { get; set; }
        [Column(TypeName = "text")]
        public string Place { get; set; }
        public int PurchasedTrainings { get; set; }
        public int AvailableTrainings { get; set; }
    }
}
