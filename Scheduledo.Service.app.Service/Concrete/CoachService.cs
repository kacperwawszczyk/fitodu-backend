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
        //private readonly IMapper _mapper;

        //private readonly IEmailService _emailService;
        //private readonly IEmailMarketingService _emailMarketingService;
        //private readonly IBillingService _billingService;

        public CoachService(
            IConfiguration configuration,
            Context context)
        {
            _configuration = configuration;
            _context = context;
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

        public async Task<Result<Coach>> GetCoach(string id)
        {
            var result = new Result<Coach>();
            var coach = await _context.Coaches.FirstOrDefaultAsync(x => x.Id == id);
            if (coach == null)
            {
                result.Error = ErrorType.NotFound;
                return result;
            }
            result.Data = coach;
            return result;
        }

        public async Task<Result> RegisterCoach(Coach coach)
        {
            var result = new Result();

            await _context.AddAsync(coach);
            await _context.SaveChangesAsync();
            return result;
        }


    }
}
