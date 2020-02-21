using Newtonsoft.Json.Linq;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
	public class TokenService : ITokenService
	{
		public async Task<Result<string>> GetEmailFromToken(string token)
		{
			var result = new Result<string>();
			
			token = token.Remove(0, 7);

			var handler = new JwtSecurityTokenHandler();
			var tokenS = handler.ReadToken(token) as JwtSecurityToken;
			JObject jsonPayload = JObject.Parse(tokenS.Payload.SerializeToJson());

			result.Data = jsonPayload.GetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").ToString();

			return result;
		}
	}
}
