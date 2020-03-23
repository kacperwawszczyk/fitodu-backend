﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Model.Entities
{
    public partial class Exercise
    {
        [Key]
        public int Id { get; set; }
        public string IdCoach { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Archived { get; set; } = false;
    }
}