using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
    public class DummyClientUpdateInput
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Name { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? FatPercentage { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
    }
}
