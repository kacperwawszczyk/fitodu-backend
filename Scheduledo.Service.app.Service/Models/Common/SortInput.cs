using Scheduledo.Core.Enums;

namespace Scheduledo.Service.Models
{
    public class SortInput
    {
        public string Column { get; set; }
        public SortDirection Direction { get; set; }
    }
}
