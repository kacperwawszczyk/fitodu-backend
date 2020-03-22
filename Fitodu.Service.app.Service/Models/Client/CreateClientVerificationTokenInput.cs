using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitodu.Service.Models
{
	public class CreateClientVerificationTokenInput
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Name { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Surname { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
		public string Email { get; set; }
		public string FullName
		{
			get
			{
				return $"{Name} {Surname}";
			}
		}
	}
}
