using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class UpdateDefaultWeekPlanInput
    {
        [Required]
        public int Id { get; set; }
        //public bool IsDefault { get; set; }
        public virtual ICollection<DayPlanInput> DayPlans { get; set; }
    }
}
