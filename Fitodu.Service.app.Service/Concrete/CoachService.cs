using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Model.Extensions;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class CoachService : ICoachService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly UserManager<User> _userManager;
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IEmailMarketingService _emailMarketingService;
        //private readonly IEmailService _emailService;
        //private readonly SignInManager<User> _signInManager;
        //private readonly IBillingService _billingService;

        public CoachService(
            ILogger<UserService> logger,
            IConfiguration configuration,
            IDateTimeService dateTimeService,
            UserManager<User> userManager,
            Context context,
            IMapper mapper,
            IEmailMarketingService emailMarketingService)
        {
            _logger = logger;
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
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
            //user.PhoneNumber = model.PhoneNumber;
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
            if (coaches == null)
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
                nc.CancelTimeHours = c.CancelTimeHours;
                nc.CancelTimeMinutes = c.CancelTimeMinutes;
                coachesResult.Add(nc);
            }
            result.Data = coachesResult;
            return result;
        }

        public async Task<Result<CoachOutput>> GetCoach(string Id)
        {
            var result = new Result<CoachOutput>();
            CoachOutput coach = await _context.Coaches
                .Select(nc => new CoachOutput
                {
                    Id = nc.Id,
                    Name = nc.Name,
                    Surname = nc.Surname,
                    Rules = nc.Rules,
                    AddressLine1 = nc.AddressLine1,
                    AddressLine2 = nc.AddressLine2,
                    AddressPostalCode = nc.AddressPostalCode,
                    AddressCity = nc.AddressCity,
                    AddressState = nc.AddressState,
                    AddressCountry = nc.AddressCountry,
                    CancelTimeHours = nc.CancelTimeHours,
                    CancelTimeMinutes = nc.CancelTimeMinutes
                })
                .FirstOrDefaultAsync(x => x.Id == Id);

            User coachAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            coach.PhoneNumber = coachAcc.PhoneNumber;

            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                result.ErrorMessage = "Coach with given id does not exist";
                return result;
            }
            else
            {
                result.Data = coach;
                return result;
            }
        }

        public async Task<Result<long>> UpdateCoach(string Id, UpdateCoachInput coachNew)
        {
            var result = new Result<long>();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            User coachAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            coach.Name = coachNew.Name;
            coach.Surname = coachNew.Surname;
            if (coachNew.CancelTimeMinutes > 59 || coachNew.CancelTimeHours > 23)
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Cancel time must be between 0 and 23:59";
                return result;
            }
            coach.CancelTimeHours = coachNew.CancelTimeHours;
            coach.CancelTimeMinutes = coachNew.CancelTimeMinutes;
            coach.AddressCity = coachNew.AddressCity;
            coach.AddressCountry = coachNew.AddressCountry;
            coach.AddressPostalCode = coachNew.AddressPostalCode;
            coach.AddressState = coachNew.AddressState;
            coach.Rules = coachNew.Rules;
            coach.AddressLine1 = coachNew.AddressLine1;
            coach.AddressLine2 = coachNew.AddressLine2;
            coach.UpdatedOn = DateTime.UtcNow;

            coachAcc.PhoneNumber = coachNew.PhoneNumber;

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Coaches.Update(coach);
                _context.Users.Update(coachAcc);
                if (await _context.SaveChangesAsync() == 0)
                {
                    result.Error = ErrorType.Forbidden;
                    transaction.Rollback();
                }
                else
                {
                    transaction.Commit();
                }
            }
            return result;
        }

        public async Task<Result<ICollection<ClientOutput>>> GetAllClients(string Id)
        {
            var result = new Result<ICollection<ClientOutput>>();
            Coach coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            if (coach == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coach found";
                return result;
            }
            List<CoachClient> coachClients = await _context.CoachClients
                .Where(x => x.IdCoach == Id)
                .ToListAsync();
            if (coachClients.Count == 0)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No clients found";
                return result;
            }
            List<ClientOutput> clients = new List<ClientOutput>();
            foreach (var y in coachClients)
            {
                ClientOutput client = await _context.Clients
                .Where(x => x.Id == y.IdClient)
                .Select(x => new ClientOutput
                {
                    Id = x.Id,
                    AddressCity = x.AddressCity,
                    AddressCountry = x.AddressCountry,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    AddressPostalCode = x.AddressPostalCode,
                    AddressState = x.AddressState,
                    FatPercentage = x.FatPercentage,
                    Height = x.Height,
                    Name = x.Name,
                    Surname = x.Surname,
                    Weight = x.Weight,
                    IsRegistered = x.IsRegistered,
                    AvailableTrainings = y.AvailableTrainings
                })
                .FirstOrDefaultAsync();
                User clientAcc = await _context.Users.FirstOrDefaultAsync(x => x.Id == client.Id);
                if (clientAcc != null)
                {
                    client.PhoneNumber = clientAcc.PhoneNumber;
                    client.Email = clientAcc.Email;
                }

                clients.Add(client);
            }
            result.Data = clients;
            return result;
        }

        public async Task<Result> SetClientsTrainingsAvailable(string requesterId, UserRole role, string clientId, int value)
        {
            var result = new Result();
            using (var transcation = _context.Database.BeginTransaction())
            {
                var coachClient = await _context.CoachClients.Where(x => x.IdCoach == requesterId && x.IdClient == clientId).FirstOrDefaultAsync();

                if (role != UserRole.Coach || coachClient == null || value < 0)
                {
                    result.Error = ErrorType.BadRequest;
                    result.ErrorMessage = "Invalid value of available trainings or user is not a coach or this user is not client of this coach.";
                    return result;
                }

                coachClient.AvailableTrainings = value;

                if (await _context.SaveChangesAsync() == 0)
                {
                    transcation.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database.";
                    return result;
                }
                transcation.Commit();
            }
            return result;
        }
    }
}
