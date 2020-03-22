using System;
using Fitodu.Service.Abstract;

namespace Fitodu.Service.Concrete
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
