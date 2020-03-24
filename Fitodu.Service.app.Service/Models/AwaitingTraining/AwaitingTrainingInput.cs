using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models.AwaitingTraining
{
    public class AwaitingTrainingInput
    {
        [Required]
        public string IdReceiver { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
    }
}
