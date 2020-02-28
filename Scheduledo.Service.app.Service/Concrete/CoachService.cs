using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scheduledo.Core.Enums;
using Scheduledo.Core.Extensions;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
using Scheduledo.Model.Extensions;
using Scheduledo.Service.Abstract;
using Scheduledo.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduledo.Service.Concrete
{
    public class CoachService : ICoachService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;

        //private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly Context _context;
        private readonly IMapper _mapper;

        //private readonly IEmailService _emailService;
        private readonly IEmailMarketingService _emailMarketingService;
        //private readonly IBillingService _billingService;

        public CoachService(
            IConfiguration configuration,
            Context context,
            IMapper mapper,
            IDateTimeService dateTimeService,
            UserManager<User> userManager,
            IEmailMarketingService emailMarketingService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _userManager = userManager;
            _emailMarketingService = emailMarketingService;
        }

        public async Task<Result> CoachRegister(RegisterCoachInput model)
        {
            var result = new Result();

            var existingUser = await _userManager.FindByNameAsync(model.Email);
            if (existingUser != null)
            {
                result.ErrorMessage = Resource.Validation.EmailTaken;
                result.Error = ErrorType.BadRequest;
                return result;
            }

            //var existingCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Url == model.Url);
            //if (existingCompany != null)
            //{
            //    result.ErrorMessage = Resource.Validation.UrlTaken;
            //    result.Error = ErrorType.BadRequest;
            //    return result;
            //}

            var user = new User();
            user.Role = UserRole.Coach;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;
            user.Id = Guid.NewGuid().ToString();
            user.Company = new Company()
            {
                //Url = model.Url,
                Plan = PricingPlan.Trial,
                PlanExpiredOn = _dateTimeService.Now().AddDays(30)
            };

            var coach = new Coach();
            coach.Id = user.Id;
            coach.Name = model.Name;
            coach.Surname = model.Surname;

            user.SetCredentials(model.Password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                //var subscribeResult = await _emailMarketingService.Subscribe(user);
                //if (subscribeResult.Success)
                //    user.SubscriberId = subscribeResult.Data;
                //else
                //    _logger.LogCritical("Can't subscribe to email marketing", user.UserName);

                var addToRoleResult = await _userManager.AddToRoleAsync(user, user.Role.GetName());
                if (!addToRoleResult.Succeeded)
                {
                    transaction.Rollback();

                    var unsubscribeResult = await _emailMarketingService.Unsubscribe(user.SubscriberId);
                    if (!unsubscribeResult)
                        _logger.LogCritical("Can't unsubscribe from email marketing", user.UserName);

                    result.Error = ErrorType.InternalServerError;
                    return result;
                }

                var createCoachResult = await _context.Coaches.AddAsync(coach);

                if (createCoachResult.State != EntityState.Added)
                {
                    transaction.Rollback();

                }
                _context.SaveChanges();

                transaction.Commit();
            }

            return result;
        }

        public async Task<Result<ICollection<CoachOutput>>> GetAllCoaches()
        {
            var result = new Result<ICollection<CoachOutput>>();
            var coaches = await _context.Coaches.ToListAsync();
            if(coaches == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coaches found";
                return result;
            }
            var coachesResult = new List<CoachOutput>();
            foreach (Coach c in coaches)
            {
                CoachOutput nc = new CoachOutput();
                nc.Id = c.Id;
                nc.AddressCity = c.AddressCity;
                nc.AddressCountry = c.AddressCountry;
                nc.AddressLine1 = c.AddressLine1;
                nc.AddressLine2 = c.AddressLine2;
                nc.AddressPostalCode = c.AddressPostalCode;
                nc.AddressState = c.AddressState;
                nc.Name = c.Name;
                nc.Rules = c.Rules;
                nc.Surname = c.Surname;
                nc.TimeToResign = c.TimeToResign;
                coachesResult.Add(nc);
            }
            result.Data = coachesResult;
            return result;
        }

        public async Task<Result<CoachOutput>> GetCoach(string Id)
        {
            var result = new Result<CoachOutput>();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }
            else
            {
                CoachOutput nc = new CoachOutput();
                nc.Id = coach.Id;
                nc.AddressCity = coach.AddressCity;
                nc.AddressCountry = coach.AddressCountry;
                nc.AddressLine1 = coach.AddressLine1;
                nc.AddressLine2 = coach.AddressLine2;
                nc.AddressPostalCode = coach.AddressPostalCode;
                nc.AddressState = coach.AddressState;
                nc.Name = coach.Name;
                nc.Rules = coach.Rules;
                nc.Surname = coach.Surname;
                nc.TimeToResign = coach.TimeToResign;
                result.Data = nc;
                return result;
            }
        }

        //public async Task<Result> RegisterCoach(Coach coach)
        //{
        //    var result = new Result();

        //    await _context.AddAsync(coach);
        //    await _context.SaveChangesAsync();
        //    return result;
        //}

        public async Task<Result> UpdateCoach(string Id, UpdateCoachInput coachNew)
        {
            var result = new Result();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);

            coach.Name = coachNew.Name;
            coach.Surname = coachNew.Surname;
            coach.TimeToResign = coachNew.TimeToResign;
            coach.AddressCity = coachNew.AddressCity;
            coach.AddressCountry = coachNew.AddressCountry;
            coach.AddressPostalCode = coachNew.AddressPostalCode;
            coach.Rules = coachNew.Rules;
            coach.AddressLine1 = coachNew.AddressLine1;
            coach.AddressLine2 = coachNew.AddressLine2;
            coach.UpdatedOn = DateTime.UtcNow;

            
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Update(coach);
                if(await _context.SaveChangesAsync() > 0)
                {
                    result = new Result(true);
                    transaction.Commit();
                }
                else
                {
                    result.Error = ErrorType.Forbidden;
                    transaction.Rollback();
                }
            }
            return result;
        }

        public async Task<Result<ICollection<ClientOutput>>> GetAllClients(string Id)
        {
            var result = new Result<ICollection<ClientOutput>>();
            Coach coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            if(coach == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coach found";
                return result;
            }
            List<CoachClient> coachClients = await _context.CoachClients.Where(x => x.IdCoach == Id).ToListAsync();
            if(coachClients.Count == 0)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No clients found";
                return result;
            }
            List<ClientOutput> clients = new List<ClientOutput>();
            foreach(var x in coachClients)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(z => z.Id == x.IdClient);
                ClientOutput nClient = new ClientOutput();
                nClient.AddressCity = client.AddressCity;
                nClient.AddressCountry = client.AddressCountry;
                nClient.AddressLine1 = client.AddressLine1;
                nClient.AddressLine2 = client.AddressLine2;
                nClient.AddressPostalCode = client.AddressPostalCode;
                nClient.AddressState = client.AddressState;
                nClient.FatPercentage = client.FatPercentage;
                nClient.Height = client.Height;
                nClient.Name = client.Name;
                nClient.Surname = client.Surname;
                nClient.Weight = client.Weight;
                nClient.Id = client.Id;
                nClient.IsRegistered = client.IsRegistered;
                clients.Add(nClient);
            }
            result.Data = clients;
            return result;
        }
    }
}
