using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class PublicNote
    {
        [Required]
        public int IdTrainer { get; set; }
        [Required]
        public int IdClient { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
