﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scheduledo.Model.Entities
{
    public partial class Exercise
    {
        [Key]
        public int Id { get; set; }
        public int IdTrainer { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
    }
}
