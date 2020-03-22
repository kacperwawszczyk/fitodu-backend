using Fitodu.Core.Enums;
using Fitodu.Service.Models.WorkoutTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class DayPlanInput
    {
        [Required]
        public Day Day { get; set; }

        public virtual ICollection<WorkoutTimeInput> WorkoutTimes { get; set; }
    }
}
