using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class SummaryInput
    {
        public string IdClient { get; set; }
        public decimal? Weight { get; set; }
        public decimal? FatPercentage { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
