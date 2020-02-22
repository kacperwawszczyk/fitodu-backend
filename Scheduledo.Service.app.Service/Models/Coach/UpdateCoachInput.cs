﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Scheduledo.Service.Models
{
    public class UpdateCoachInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Surname { get; set; }
        public string Rules { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressPostalCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressCity { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressState { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string AddressCountry { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string TimeToResign { get; set; }
    }
}
