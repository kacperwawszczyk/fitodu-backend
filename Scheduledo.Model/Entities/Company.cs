using System;
using System.Collections.Generic;
using Scheduledo.Core.Enums;

namespace Scheduledo.Model.Entities
{
    public class Company : BaseEntity<long>
    {
        public string Url { get; set; }

        public PricingPlan Plan { get; set; }
        public DateTime? PlanExpiredOn { get; set; }

        public string BillingSubscriptionId { get; set; }
        public string BillingCustomerId { get; set; }

        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string VatIn { get; set; }

        public int SmsCounter { get; set; }
        public int EmailCounter { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
