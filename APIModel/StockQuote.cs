using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.API.APIModel
{
    public class StockQuote
    {
        public string symbol { get; set; }
        public double price { get; set; }
        public double volume { get; set; }
    }
}
