using Fitodu.Model.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Coach
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(30)]
        public string Surname { get; set; }
        [Column(TypeName = "text")]
        public string Rules { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string VatIn { get; set; }
        public uint? CancelTimeHours { get; set; }
        public uint? CancelTimeMinutes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndOfLicenseDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        //public virtual ICollection<CoachClient> CoachClients { get; set; }
        //public virtual ICollection<AwaitingTraining> AwaitingTrainings { get; set; }
        //public virtual ICollection<PublicNote> PublicNotes { get; set; }
        //public virtual ICollection<PrivateNote> PrivateNotes { get; set; }
        //public virtual ICollection<Exercise> Exercises { get; set; }
        //public virtual ICollection<PlannedTraining> PlannedTrainings { get; set; }
        //public virtual ICollection<Training> Trainings { get; set; }
        //public virtual ICollection<WorkTime> WorkTimes { get; set; }

        public Coach()
        {
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
