using Scheduledo.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Service.Models
{
    public class WeekPlanInput
    {
        [Required]
        public string IdCoach { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public virtual ICollection<DayPlanInput> DayPlans { get; set; }

    }
}
