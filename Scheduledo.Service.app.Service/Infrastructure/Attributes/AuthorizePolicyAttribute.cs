using Scheduledo.Core.Enums;
using Scheduledo.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Scheduledo.Service.Infrastructure.Attributes
{
    public class AuthorizePolicyAttribute : AuthorizeAttribute
    {
        public AuthorizePolicyAttribute(UserRole role)
        {
            Policy = role.GetName();
        }
    }
}
