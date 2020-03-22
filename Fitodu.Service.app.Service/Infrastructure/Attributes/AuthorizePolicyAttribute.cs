using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Fitodu.Service.Infrastructure.Attributes
{
    public class AuthorizePolicyAttribute : AuthorizeAttribute
    {
        public AuthorizePolicyAttribute(UserRole role)
        {
            Policy = role.GetName();
        }
    }
}
