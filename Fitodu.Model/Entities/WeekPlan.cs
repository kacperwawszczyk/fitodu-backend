using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class WeekPlan
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string IdCoach { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public virtual ICollection<DayPlan> DayPlans { get; set; }
    }
}
