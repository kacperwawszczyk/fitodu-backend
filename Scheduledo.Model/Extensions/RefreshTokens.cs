using System;
using Scheduledo.Model.Entities;

namespace Scheduledo.Model.Extensions
{
    public static class RefreshTokens
    {
        public static bool IsActive(this RefreshToken refreshToken, DateTime dateTimeNow)
        {
            return refreshToken.ExpiresOn > dateTimeNow;
        }
    }
}
