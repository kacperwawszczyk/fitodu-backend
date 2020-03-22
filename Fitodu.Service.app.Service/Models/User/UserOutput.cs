using System;
using Fitodu.Core.Enums;

namespace Fitodu.Service.Models
{
    public class UserOutput : BaseOutput<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public PricingPlan Plan { get; set; }
        public DateTime? PlanExpiredOn { get; set; }
    }
}
