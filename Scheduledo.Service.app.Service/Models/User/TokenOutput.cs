using System;
using Scheduledo.Core.Enums;

namespace Scheduledo.Service.Models
{
    public class TokenOutput
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public bool FirstLogin { get; set; } = false;
        public UserRole Role { get; set; }
        public PricingPlan Plan { get; set; }
        public DateTime? PlanExpiredOn { get; set; }
    }
}
