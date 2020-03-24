using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models
{
    public class ExerciseOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; } = false;
    }
}

