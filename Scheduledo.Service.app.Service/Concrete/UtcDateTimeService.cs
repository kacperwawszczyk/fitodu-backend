using System;
using Scheduledo.Service.Abstract;

namespace Scheduledo.Service.Concrete
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
