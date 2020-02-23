using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scheduledo.Core.Enums;
using Scheduledo.Model;
using Scheduledo.Model.Entities;
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
        //private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        //private readonly IDateTimeService _dateTimeService;

        //private readonly SignInManager<User> _signInManager;
       // private readonly UserManager<User> _userManager;

        private readonly Context _context;
        private readonly IMapper _mapper;

        //private readonly IEmailService _emailService;
        //private readonly IEmailMarketingService _emailMarketingService;
        //private readonly IBillingService _billingService;

        public CoachService(
            IConfiguration configuration,
            Context context,
            IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<UpdateCoachInput>>> GetAllCoaches()
        {
            var result = new Result<List<UpdateCoachInput>>();
            var coaches = await _context.Coaches.ToListAsync();
            if(coaches == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coaches found";
                return result;
            }
            var coachesResult = new List<UpdateCoachInput>();
            foreach(Coach c in coaches)
            {
                UpdateCoachInput nc = new UpdateCoachInput();
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

        public async Task<Result<Coach>> GetCoach(string Id)
        {
            var result = new Result<Coach>();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);
            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }
            else
            {
                result.Data = coach;
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

            if (!String.IsNullOrEmpty(coachNew.Name))
            {
                coach.Name = coachNew.Name;
            }
            if (!String.IsNullOrEmpty(coachNew.Surname))
            {
                coach.Surname = coachNew.Surname;
            }
            if (!String.IsNullOrEmpty(coachNew.TimeToResign))
            {
                coach.TimeToResign = coachNew.TimeToResign;
            }
            if(!String.IsNullOrEmpty(coachNew.AddressCity))
            {
                coach.AddressCity = coachNew.AddressCity;
            }
            if(!String.IsNullOrEmpty(coachNew.AddressCountry))
            {
                coach.AddressCountry = coachNew.AddressCountry;
            }
            if(!String.IsNullOrEmpty(coachNew.AddressPostalCode))
            {
                coach.AddressPostalCode = coachNew.AddressPostalCode;
            }
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
    }
}
