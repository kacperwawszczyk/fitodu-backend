using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
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
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Weight { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? FatPercentage { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsRegistered { get; set; }
        public virtual ICollection<CoachClient> ClientCoaches { get; set; }
        public virtual ICollection<Maximum> Maximums { get; set; }
        public virtual ICollection<AwaitingTraining> AwaitingTrainings { get; set; }
        public virtual ICollection<PrivateNote> PrivateNotes { get; set; }
        public virtual ICollection<PublicNote> PublicNotes { get; set; }
        public virtual ICollection<Training> Trainings { get; set; }

        public Client()
        {
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

    }
}
