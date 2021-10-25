using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Skender.Stock.Indicators;
using Thor.Domain.Models;

namespace Thor.Domain.Services
{
    public interface IStockStrategyService
    {
        Task<TopStock> CalculateStockIndicators(string symbol);

        Task<List<string>> GetLargestGainStocks();

        Task<PremarketStock> GetPremarketStocks(string symbol);

        Task<List<string>> GetBiggestStockLosers();
    }
}
