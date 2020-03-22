using System;
using Fitodu.Model.Entities;

namespace Fitodu.Model.Extensions
{
    public static class RefreshTokens
    {
        public static bool IsActive(this RefreshToken refreshToken, DateTime dateTimeNow)
        {
            return refreshToken.ExpiresOn > dateTimeNow;
        }
    }
}
