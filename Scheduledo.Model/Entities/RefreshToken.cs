using System;

namespace Scheduledo.Model.Entities
{
    public class RefreshToken : BaseEntity<long>
    {
        public DateTime ExpiresOn { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
