using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
	public interface ITokenService
	{
		string GetEmailFromToken(string token);

		Task<Result<string>> GetRequesterCoachId(string token);

		Task<Result<string>> GetRequesterClientId(string token);
	}
}
