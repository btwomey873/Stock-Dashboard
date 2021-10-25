using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Exceptions
{
    public class TooManySharesException : Exception
    {
        public decimal Shares { get; set; }

        public TooManySharesException(decimal shares)
        {
            Shares = shares;
        }

        public TooManySharesException(decimal shares, string message) : base(message)
        {
            Shares = shares;
        }

        public TooManySharesException(decimal shares, string message, Exception innerException) : base(message, innerException)
        {
            Shares = shares;
        }
    }
}
