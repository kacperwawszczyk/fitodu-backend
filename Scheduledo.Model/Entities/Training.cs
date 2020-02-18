using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class Training
    {
        [Key]
        public int Id { get; set; }
        public int IdClient { get; set; }
        public int IdTrainer { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
