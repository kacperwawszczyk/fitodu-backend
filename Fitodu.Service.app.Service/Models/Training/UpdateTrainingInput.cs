using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Service.Models.Training
{
    public class UpdateTrainingInput
    {
        [Required]
        public int Id { get; set; }
        public string IdClient { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string Note { get; set; }
    }
}
