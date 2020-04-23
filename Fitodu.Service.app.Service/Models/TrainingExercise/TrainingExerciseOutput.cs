using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Service.Models
{
    public class TrainingExerciseOutput
    {
        public int Id { get; set; }
        public ExerciseOutput Exercise { get; set; }
        public int Repetitions { get; set; }
        public TimeSpan? Time { get; set; }
        public int RepetitionsResult { get; set; }
        public TimeSpan? TimeResult { get; set; }
        public MaximumOutput Maximum { get; set; }
    }
}
