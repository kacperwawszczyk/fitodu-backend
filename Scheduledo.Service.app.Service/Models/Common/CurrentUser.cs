using Scheduledo.Core.Enums;

namespace Scheduledo.Service.Models
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
    }
}
