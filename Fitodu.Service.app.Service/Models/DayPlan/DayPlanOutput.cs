using Fitodu.Core.Enums;
using Fitodu.Service.Models.WorkoutTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models
{
    public class DayPlanOutput
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Day Day { get; set; }

        public virtual ICollection<WorkoutTimeOutput> WorkoutTimes { get; set; }
    }
}
