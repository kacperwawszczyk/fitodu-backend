using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Abstract
{
	public interface ITokenService
	{
		Task<Result<string>> GetEmailFromToken(string token);
	}
}
