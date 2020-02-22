using System;
using Scheduledo.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Scheduledo.Model.Entities
{
    public class User : IdentityUser
    {
        public UserRole Role { get; set; }

        public string FullName { get; set; }

        public string ResetToken { get; set; }

        public DateTime? ResetTokenExpiresOn { get; set; }

        public int SubscriberId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long CompanyId { get; set; }
        public virtual Company Company { get; set; }

        // Do rozwiązania jeszcze
        // public UserType Type { get; set; }

        public User()
        {
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
