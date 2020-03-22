using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models.WorkoutTime
{
    public class WorkoutTimeInput
    {
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }
    }
}
