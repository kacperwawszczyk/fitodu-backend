﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scheduledo.Core.Enums;
using Scheduledo.Core.Extensions;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Model.Extensions;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
	public class ClientService : IClientService
	{
        private readonly Context _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public ClientService(Context context, IDateTimeService dateTimeService, IConfiguration configuration, IEmailService emailService, UserManager<User> userManager)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _configuration = configuration;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task<Result> CreateClientAccount(RegisterClientInput model)
		{
            var result = new Result();

            var exist = await _context.CreateClientTokens.Where(x => x.UserId == model.Id && x.Token == model.Token).FirstOrDefaultAsync();

            if(exist == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid verification code";
                return result;
            }

            var client = await _context.Clients.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (client == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid User";
                return result;
            }

            client.IsRegistered = true;

            var user = new User
            {
                Role = UserRole.Client,
                FullName = $"{client.Name} {client.Surname}",
                Id = model.Id,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Company = new Company()
                {
                    Plan = PricingPlan.Trial,
                    PlanExpiredOn = _dateTimeService.Now().AddDays(30)
                }
            };

            user.SetCredentials(model.Password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create User";
                    return result;
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, user.Role.GetName());

                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add User to role.";
                    return result;
                }

                var updateRegisteredStatus = _context.Clients.Update(client);

                if(updateRegisteredStatus.State != EntityState.Modified)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot update Client register state";
                    return result;
                }

                if(await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot save changes";
                    return result;
                }

                transaction.Commit();
            }

            return result;
		}

        public async Task<Result> DummyClientRegister(string CoachId, RegisterDummyClientInput model)
        {
            var result = new Result();

            var coach = await _context.Coaches.Where(x => x.Id == CoachId).FirstOrDefaultAsync();

            if(coach == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Coach doesn't exist";
                return result;
            }
            
            var newClient = new Client
            {
                Name = model.Name,
                Surname = model.Surname,
                Id = Guid.NewGuid().ToString(),
                IsRegistered = false
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var addClientResult = await _context.Clients.AddAsync(newClient);

                if (addClientResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create client";
                    return result;
                }

                var coachClient = new CoachClient
                {
                    IdCoach = CoachId,
                    IdClient = newClient.Id
                };

                var addCoachClientResult = await _context.CoachClients.AddAsync(coachClient);

                if (addCoachClientResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add Client to Coach";
                    return result;
                }

                _context.SaveChanges();

                transaction.Commit();

            }
            return result;
        }

        public async Task<Result> SelfCreateClientAccount(SelfRegisterClientInput model)
        {
            var result = new Result();

            var creationToken = await _context.CreateClientTokens.Where(x => x.Token == model.Token).FirstOrDefaultAsync();

            if(creationToken == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Invalid token";
                return result;
            }

            var coach = await _context.Coaches.Where(x => x.Id == model.IdCoach).FirstOrDefaultAsync();

            if(coach == null)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Coach doesn't exist";
                return result;
            }

            var newClient = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                IsRegistered = true
            };

            var newUser = new User
            {
                Id = newClient.Id,
                Role = UserRole.Client,
                FullName = $"{model.Name} {model.Surname}",
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Company = new Company()
                {
                    Plan = PricingPlan.Trial,
                    PlanExpiredOn = _dateTimeService.Now().AddDays(30)
                }
            };

            newUser.SetCredentials(model.Password);

            var coachClient = new CoachClient
            {
                IdClient = newClient.Id,
                IdCoach = model.IdCoach
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create User.";
                    return result;
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, newUser.Role.GetName());

                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add User to role.";
                    return result;
                }

                var createClient = await _context.Clients.AddAsync(newClient);

                if (createClient.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create Client.";
                    return result;
                }

                var addCoachClient = await _context.CoachClients.AddAsync(coachClient);

                if(addCoachClient.State != EntityState.Added)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot add Client to Coach.";
                    return result;
                }

                if (await _context.SaveChangesAsync() <= 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot save changes";
                    return result;
                }

                transaction.Commit();
            }

            return result;

        }

        public async Task<Result> SendCreationLinkToClient(string CoachId, CreateClientVerificationTokenInput model)
        {
            var result = new Result();

            var client = await _context.Clients.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if(client == null)
            {
                result.Error = ErrorType.BadRequest;

                result.ErrorMessage = $"Client (ID: {model.Id}) doesn't exist";

                return result;
            }

            var coachClient = await _context.CoachClients.Where(x => x.IdClient == model.Id && x.IdCoach == CoachId).FirstOrDefaultAsync();

            if (coachClient == null)
            {
                result.Error = ErrorType.BadRequest;

                result.ErrorMessage = $"Client and coach are not connected.";

                return result;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Role, UserRole.Client.GetName()),
                new Claim(ClaimTypes.Name, model.Name)
            };

            var expires = _dateTimeService.Now().AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var createClientToken = new CreateClientToken
            {
                UserId = model.Id,
                Token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken),
                ExpiresOn = _dateTimeService.Now().AddMinutes(30)
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var addTokenResult = await _context.CreateClientTokens.AddAsync(createClientToken);

                if(addTokenResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create token";
                    return result;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            var encodedToken = WebUtility.UrlEncode(jwtSecurityTokenHandler.WriteToken(jwtSecurityToken));
            var url = $"{_configuration["Environment:ClientUrl"]}/auth/clientRegister?id={model.Id}&token={encodedToken}";

            var message = new EmailInput()
            {
                To = model.Email,
                Subject = Resource.VerifyClientTemplate.VerifyClientSubject,
                HtmlBody = Resource.VerifyClientTemplate.VerifyClientBody
            };

            message.HtmlBody = message.HtmlBody.Replace("-fullName-", model.FullName).Replace("-url-", url);

            var response = await _emailService.Send(message);

            if(response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Cannot send email.";
            }

            return result;
        }

        public async Task<Result> SendSelfCreationLinkToClient(string CoachId, CreateSelfClientVerificationTokenInput model)
        {
            var result = new Result();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "noId"),
                new Claim(ClaimTypes.Role, UserRole.Client.GetName()),
                new Claim(ClaimTypes.Name, "noName")
            };

            var expires = _dateTimeService.Now().AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var createClientToken = new CreateClientToken
            {
                UserId = "noId",
                Token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken),
                ExpiresOn = _dateTimeService.Now().AddMinutes(30)
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                var addTokenResult = await _context.CreateClientTokens.AddAsync(createClientToken);

                if (addTokenResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Cannot create token.";
                    return result;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            var encodedToken = WebUtility.UrlEncode(jwtSecurityTokenHandler.WriteToken(jwtSecurityToken));
            var url = $"{_configuration["Environment:ClientUrl"]}/auth/clientSelfRegister?token={encodedToken}";

            var message = new EmailInput()
            {
                To = model.Email,
                Subject = Resource.VerifyClientTemplate.VerifyClientSubject,
                HtmlBody = Resource.VerifyClientTemplate.VerifyClientBody
            };

            message.HtmlBody = message.HtmlBody.Replace("-fullName-", model.Email).Replace("-url-", url);

            var response = await _emailService.Send(message);

            if (response.Code != HttpStatusCode.Accepted && response.Code != HttpStatusCode.OK)
            {
                result.Error = ErrorType.InternalServerError;
                result.ErrorMessage = "Cannot send email.";
            }

            return result;
        }
    }
}
