using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fitodu.Service.Models
{
    public class FeatureRequestInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        public string Description { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}
