using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models
{
    public class SummaryOutput
    {
        public int Id { get; set; }
        public string IdClient { get; set; }
        public decimal? Weight { get; set; }
        public decimal? FatPercentage { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
