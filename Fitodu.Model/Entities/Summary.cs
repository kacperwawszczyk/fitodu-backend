using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Summary
    {
        public int Id { get; set; }
        public string IdClient { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Weight { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? FatPercentage { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
