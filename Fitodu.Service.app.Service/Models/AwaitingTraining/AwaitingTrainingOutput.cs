using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models.AwaitingTraining
{
    public class AwaitingTrainingOutput
    {
        public int Id { get; set; }
        public string IdCoach { get; set; }
        public string IdClient { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public string  RequestedName {get; set;} 
        public string  RequestedSurname {get; set;} 
    }
}
