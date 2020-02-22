using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class Maximum
    {
        public string IdClient { get; set; }
        public int IdExercise { get; set; }
        public string Max { get; set; }
    }
}