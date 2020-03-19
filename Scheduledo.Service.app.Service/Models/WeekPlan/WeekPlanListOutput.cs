using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduledo.Service.Models
{
    public class WeekPlanListOutput
    {
        public int Id { get; set; }
        public string IdCoach { get; set; }
        public DateTime StartDate { get; set; }
    }
}
