using System;

namespace Fitodu.Service.Models
{
    public abstract class BaseOutput<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
