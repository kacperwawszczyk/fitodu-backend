using AutoMapper;
using AutoMapper.Configuration;
using Fitodu.Core.Enums;
using Fitodu.Model;
using Fitodu.Model.Entities;
using Fitodu.Service.Abstract;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Concrete
{
    public class UserFeedbackService : IUserFeedbackService
    {
        private readonly Context _context;

        public UserFeedbackService( Context context)
        {
            _context = context;
        }

        public async Task<Result> AddUserFeedback(string userId, UserRole userRole, UserFeedbackInput input)
        {
            var result = new Result();
            UserFeedback userFeedback = new UserFeedback();
            if (String.IsNullOrEmpty(input.Message))
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Cannot add new user feedback without message!";
                return result;
            }
            if (String.IsNullOrEmpty(input.URL))
            {
                result.Error = ErrorType.BadRequest;
                result.ErrorMessage = "Cannot add new user feedback without URL!";
                return result;
            }
            userFeedback.Role = userRole;
            userFeedback.Message = input.Message;
            userFeedback.URL = input.URL;
            userFeedback.UserId = userId;
            userFeedback.Date = DateTime.UtcNow;

            using(var transaction = _context.Database.BeginTransaction())
            {
                await _context.UserFeedbacks.AddAsync(userFeedback);
                if(await _context.SaveChangesAsync() == 0)
                {
                    transaction.Rollback();
                    result.Error = ErrorType.InternalServerError;
                    result.ErrorMessage = "Couldn't save changes to the database";
                    return result;
                }
                else
                {
                    transaction.Commit();
                }
            }

            return result;
        }
    }
}
