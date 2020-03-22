using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models
{
    public class WorkoutTimeOutput
    {

        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }
    }
}
