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

        public async Task<Result<List<Coach>>> GetAllCoaches()
        {
            var result = new Result<List<Coach>>();
            var coaches = await _context.Coaches.ToListAsync();
            if(coaches == null)
            {
                result.Error = ErrorType.NoContent;
                result.ErrorMessage = "No coaches available";
                return result;
            }
            result.Data = coaches;
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

        public async Task<Result> ModifyCoach(string Id, Coach coachNew)
        {
            var result = new Result();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == Id);

            coach.Name = coachNew.Name;
            coach.Surname = coachNew.Surname;
            coach.TimeToResign = coachNew.TimeToResign;
            coach.Rules = coachNew.Rules;
            coach.VatIn = coachNew.VatIn;
            coach.AddressCity = coachNew.AddressCity;
            coach.AddressCountry = coachNew.AddressCountry;
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
