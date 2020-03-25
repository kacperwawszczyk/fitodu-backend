using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Maximum
    {
        public string IdClient { get; set; }
        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }
        public int IdExercise { get; set; }
        [ForeignKey("IdExercise")]
        public virtual Exercise Exercise { get; set; }
        public string Max { get; set; }
    }
}