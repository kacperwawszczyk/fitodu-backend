using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class PrivateNote
    {
        [Required]
        public string IdCoach { get; set; }
        [Required]
        public string IdClient { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
