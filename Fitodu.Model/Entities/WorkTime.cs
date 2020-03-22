using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    //TODO: usunąć jak na pewno nie będzie potrzebne
    public partial class WorkTime
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string IdCoach { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Day { get; set; }
        public TimeSpan? StartHour { get; set; }
        public TimeSpan? EndHour { get; set; }
    }
}