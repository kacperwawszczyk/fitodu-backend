using Scheduledo.Core.Enums;
using Scheduledo.Service.Models.WorkoutTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Service.Models
{
    public class DayPlanInput
    {
        [Required]
        public Day Day { get; set; }

        public virtual ICollection<WorkoutTimeInput> WorkoutTimes { get; set; }
    }
}
