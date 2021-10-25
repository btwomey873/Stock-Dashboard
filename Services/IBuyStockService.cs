using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Services
{
    public interface IBuyStockService
    {
        Task<double> GetCurrentPrice(string symbol);

        Task<bool> BuyStock(string symbol, int shares);
    }
}
