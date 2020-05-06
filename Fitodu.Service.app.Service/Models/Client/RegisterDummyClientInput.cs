using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
	public class RegisterDummyClientInput
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Name { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Surname { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
	}
}
