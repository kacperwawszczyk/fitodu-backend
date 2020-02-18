using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class PrivateNote
    {
        [Required]
        public int IdTrainer { get; set; }
        [Required]
        public int IdClient { get; set; }
        public string Note { get; set; }
    }
}
