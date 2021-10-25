using Alpaca.Markets;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Models;
using Thor.Domain.Services;


namespace Thor.API.Services
{
    public class StockChartService : IStockChartService
    {
        private IAlpacaDataClient client;

        public StockChartService()
        {
            client = Alpaca.Markets.Environments.Paper
               .GetAlpacaDataClient(new SecretKey(APIManager.APIManager.ALPACA_KEY, APIManager.APIManager.ALPACA_SECRET_KEY));
        }

        public async Task<List<StockChartDefaultInfo>> GetCandleStickData(string symbol)
        {
            List<StockChartDefaultInfo> candlesticks = new List<StockChartDefaultInfo>();

            var into = DateTime.Today;
            var from = into.AddDays(-45);

            var bars = await client.ListHistoricalBarsAsync(
                new HistoricalBarsRequest(symbol, from, into, BarTimeFrame.Day));


            for (int i = 0; i < bars.Items.Count; i++)
            {
                candlesticks.Add(new StockChartDefaultInfo()
                {
                    Close = (double)bars.Items[i].Close,
                    Open = (double)bars.Items[i].Open,
                    High = (double)bars.Items[i].High,
                    Low = (double)bars.Items[i].Low,
                    DateTime = bars.Items[i].TimeUtc.ToString("dd MM")
                });

            }

            return candlesticks;
        }

    
    }
}
