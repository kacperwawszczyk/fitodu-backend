using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class WorkoutTime
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        //public int DayPlanId { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }
    }
}
