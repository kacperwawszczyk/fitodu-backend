using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
	public interface ITokenService
	{
		string GetEmailFromToken(string token);

		Task<Result<string>> GetRequesterCoachId(string token);
	}
}
