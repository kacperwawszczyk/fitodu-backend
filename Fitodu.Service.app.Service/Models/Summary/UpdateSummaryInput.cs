using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class UpdateSummaryInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string IdClient { get; set; }
        [Required]
        public decimal? Weight { get; set; }
        [Required]
        public decimal? FatPercentage { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Description { get; set; }
        [Required]
        public DateTime? Date { get; set; }
    }
}
