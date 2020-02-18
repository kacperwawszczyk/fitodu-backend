using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class Maximum
    {
        [Key]
        public int Id { get; set; }
        public int IdClient { get; set; }
        public int IdExercise { get; set; }
        public int? Max { get; set; }
    }
}
