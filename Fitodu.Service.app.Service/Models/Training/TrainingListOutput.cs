using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models
{
    public class TrainingListOutput
    {
        public int Id { get; set; }
        public string IdClient { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientAvatar { get; set; }
        //public ICollection<TrainingExerciseOutput> TrainingExercises { get; set; }
    }
}
