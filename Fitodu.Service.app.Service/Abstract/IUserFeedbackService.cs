using Fitodu.Core.Enums;
using Fitodu.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fitodu.Service.Abstract
{
    public interface IUserFeedbackService
    {
        Task<Result> AddUserFeedback(string userId, UserRole userRole, UserFeedbackInput input);
    }
}
