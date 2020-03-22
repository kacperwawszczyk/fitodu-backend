using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Summary
    {
        public int Id { get; set; }
        public int IdTraining { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? WeightChange { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
    }
}
