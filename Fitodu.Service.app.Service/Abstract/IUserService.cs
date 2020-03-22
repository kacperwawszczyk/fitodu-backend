using System.Collections.Generic;
using System.Threading.Tasks;
using Fitodu.Service.Models;

namespace Fitodu.Service.Abstract
{
    public interface IUserService
    {    
        //Task<Result<ICollection<UserListItemOutput>>> GetList(string adminId);
        Task<Result<UserOutput>> Get(string userId);
        Task<Result<CompanyOutput>> GetCompany(string userId);
        //Task<Result<string>> Create(CreateUserInput model);
        Task<Result<string>> Update(UpdateUserInput model);
        //Task<Result<string>> UpdateMe(UpdateUserInput model);
        //Task<Result<long>> UpdateCompany(UpdateCompanyInput model);
        Task<Result<string>> Delete(string adminId, string userId);
        Task<Result<TokenOutput>> CreateToken(CreateTokenInput model, bool auth = true);
        Task<Result<TokenOutput>> RefreshToken(RefreshTokenInput model);
        Task<Result> SignOut(SignOutInput model);
        Task<Result> ForgotPassword(string email);
        Task<Result> ResetPassword(ResetPasswordInput model);
    }
}
