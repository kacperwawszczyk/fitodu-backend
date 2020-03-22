
namespace Fitodu.Service.Models
{
    public class UserListItemOutput : BaseOutput<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
