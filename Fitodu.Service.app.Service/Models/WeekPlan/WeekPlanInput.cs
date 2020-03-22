using Fitodu.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models
{
    public class WeekPlanInput
    {
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public virtual ICollection<DayPlanInput> DayPlans { get; set; }

    }
}
