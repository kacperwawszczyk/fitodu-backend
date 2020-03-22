using System;

namespace Fitodu.Model.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsArchived { get; set; }

        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
