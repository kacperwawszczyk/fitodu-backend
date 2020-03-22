using System.ComponentModel.DataAnnotations;

namespace Fitodu.Service.Models
{
    public class PaginationInput
    {
        [Range(1, int.MaxValue, ErrorMessage = "RangeError")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RangeError")]
        public int PageSize { get; set; }

        public PaginationInput()
        {
            Page = 1;
            PageSize = int.MaxValue;
        }
    }
}
