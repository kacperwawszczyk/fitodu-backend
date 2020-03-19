using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Service.Models.PrivateNote
{
    public class PrivateNoteInput
    {
        [Required]
        public string IdClient { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
