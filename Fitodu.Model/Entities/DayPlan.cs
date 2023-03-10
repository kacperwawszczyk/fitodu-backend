using Fitodu.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class DayPlan
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        //public int WeekPlanId { get; set; }
        [Required]
        public Day Day { get; set; }

        public virtual ICollection<WorkoutTime> WorkoutTimes { get; set; }

        public virtual WeekPlan WeekPlan  { get; set; }
}
}
