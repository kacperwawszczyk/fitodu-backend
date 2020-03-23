using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models
{
    public class WeekPlanOutput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string IdCoach { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<DayPlanOutput> DayPlans { get; set; }
    }
}
