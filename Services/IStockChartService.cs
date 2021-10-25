using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Models;

namespace Thor.Domain.Services
{
    public interface IStockChartService
    {
        Task<List<StockChartDefaultInfo>> GetCandleStickData(string symbol);

    }
}
