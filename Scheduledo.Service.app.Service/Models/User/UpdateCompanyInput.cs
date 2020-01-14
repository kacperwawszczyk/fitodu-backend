using Newtonsoft.Json;

namespace Scheduledo.Service.Models
{
    public class UpdateCompanyInput
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string VatIn { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}