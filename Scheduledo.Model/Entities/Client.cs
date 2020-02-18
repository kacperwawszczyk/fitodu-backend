using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class Client
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(30)]
        public string Surname { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? Weight { get; set; }
        [Column(TypeName = "decimal(3, 2)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(2, 2)")]
        public decimal? FatPercentage { get; set; }

        public ICollection<CoachClient> ClientCoaches { get; set; }
    }
}
