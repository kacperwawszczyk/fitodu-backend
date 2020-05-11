using Fitodu.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public class UserFeedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public UserRole Role { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string URL { get; set; }
    }
}
