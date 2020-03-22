using Fitodu.Core.Enums;

namespace Fitodu.Service.Models
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
    }
}
