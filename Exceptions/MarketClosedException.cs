using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Exceptions
{
    public class MarketClosedException : Exception
    {
        public DateTime dateTime { get; set; }

        public MarketClosedException(DateTime _dateTime)
        {
            dateTime = _dateTime;
        }

        public MarketClosedException(DateTime _dateTime, string message) : base(message)
        {
            dateTime = _dateTime;
        }

        public MarketClosedException(DateTime _dateTime, string message, Exception innerException) : base(message, innerException)
        {
            dateTime = _dateTime;
        }


    }
}
