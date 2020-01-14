using System;
using Scheduledo.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace Scheduledo.Model.Extensions
{
    public static class Users
    {
        public static void SetCredentials(this User user, string password)
        {
            var passwordHash = new PasswordHasher<User>();
            var hashed = passwordHash.HashPassword(user, password);

            user.PasswordHash = hashed;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
