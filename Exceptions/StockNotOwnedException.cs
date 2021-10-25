using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Exceptions
{
    public class StockNotOwnedException : Exception
    {
        public string Symbol { get; set; }

        public StockNotOwnedException(string symbol)
        {
            Symbol = symbol;
        }

        public StockNotOwnedException(string symbol, string message) : base(message)
        {
            Symbol = symbol;
        }

        public StockNotOwnedException(string symbol, string message, Exception innerException) : base(message, innerException)
        {
            Symbol = symbol;
        }
    }
}
