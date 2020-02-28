using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Service.Models.Training
{
    public class TrainingInput
    {
        public string IdClient { get; set; }
        public string IdCoach { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
