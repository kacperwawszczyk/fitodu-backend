using System;
using System.Linq;
using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fitodu.Model.Migrations
{
    public class Configuration
    {
        public static void Initialize(Context context)
        {
            var initializer = new Configuration();
            initializer.Seed(context).Wait();
        }

        private async Task Seed(Context context)
        {
            context.Database.Migrate();

            await AddUserRoles(context);
            //await AddSuperAdmin(context);
        }

        private async Task AddUserRoles(Context context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            foreach (var item in Enums.GetEnumValues<UserRole>())
            {
                if (!context.Roles.Any(x => x.Name == item.GetName()))
                {
                    await roleStore.CreateAsync(new IdentityRole
                    {
                        Name = item.GetName(),
                        NormalizedName = item.GetName().ToUpper()
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        //private async Task AddSuperAdmin(Context context)
        //{
        //    string email = "admin@Fitodu.com";

        //    if (!context.Users.Any(u => u.UserName == email))
        //    {
        //        var user = new User
        //        {
        //            Role = UserRole.SuperAdmin,
        //            //FullName = "Jan Kowalski",
        //            Email = email,
        //            PhoneNumber = "+48 000 000 000",
        //            NormalizedEmail = email.ToUpper(),
        //            UserName = email,
        //            NormalizedUserName = email.ToUpper(),
        //            Company = new Company
        //            {
        //                Url = "kowalski"
        //            },
        //            SecurityStamp = Guid.NewGuid().ToString()
        //        };

        //        var password = new PasswordHasher<User>();
        //        var hash = password.HashPassword(user, "Admin123?");
        //        user.PasswordHash = hash;

        //        var userStore = new UserStore<User>(context);
        //        await userStore.CreateAsync(user);
        //        await userStore.AddToRoleAsync(user, user.Role.GetName());
        //    }

        //    await context.SaveChangesAsync();
        //}
    }
}
