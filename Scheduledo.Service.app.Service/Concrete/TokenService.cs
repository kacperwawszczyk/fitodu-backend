using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Scheduledo.Service.Concrete
{
	public class TokenService : ITokenService
	{
		private readonly IMapper _mapper;
		private readonly Context _context;

		public TokenService(Context context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public string GetEmailFromToken(string token)
		{
			token = token.Remove(0, 7);

			var handler = new JwtSecurityTokenHandler();
			var tokenS = handler.ReadToken(token) as JwtSecurityToken;
			JObject jsonPayload = JObject.Parse(tokenS.Payload.SerializeToJson());

			string email = jsonPayload.GetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").ToString();

			return email;
		}

		public async Task<Result<string>> GetRequesterClientId(string token)
		{
			string email = GetEmailFromToken(token);

			var result = new Result<string>();

			User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

			if (user != null) //istnieje user z takim emailem
			{
				var client = await _context.Clients.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
				if (client != null)
				{
					result.Data = client.Id;
				}
				else //user z takim Id nie jest w tablicy Clients
				{
					result.Error = ErrorType.NotFound; //może inny?
				}
			}
			else //nie istnieje user z takim emailem
			{
				result.Error = ErrorType.NotFound;
			}
			return result;
		}
	

		public async Task<Result<string>> GetRequesterCoachId(string token)
		{
			string email = GetEmailFromToken(token);

            var result = new Result<string>();

			User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user != null) //istnieje user z takim emailem
            {
				var coach = await _context.Coaches.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
				if(coach != null)
				{
					result.Data = coach.Id;
				}
				else //user z takim Id nie jest w tablicy Coaches
				{
					result.Error = ErrorType.NotFound; //może inny?
				}
            }
            else //nie istnieje user z takim emailem
            {
                result.Error = ErrorType.NotFound;
            }
			return result;
		}
	}
}
