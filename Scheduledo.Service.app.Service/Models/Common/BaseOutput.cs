using System;

namespace Scheduledo.Service.Models
{
    public abstract class BaseOutput<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
