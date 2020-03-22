using System;
using System.Collections.Generic;
using System.Text;

namespace Fitodu.Model.Entities
{
	public class CreateClientToken : BaseEntity<long>
	{
		public DateTime ExpiresOn { get; set; }
		public string Token { get; set; }
		public string UserId { get; set; }
		public virtual Client Client { get; set; }
	}
}
