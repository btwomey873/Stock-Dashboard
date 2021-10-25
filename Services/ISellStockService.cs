using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Services
{
    public interface ISellStockService
    {
        Task<bool> sellStock(string symbol, int shares);
    }
}
