using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Services;
using Alpaca.Markets;
using Thor.API.APIModel;
using Thor.Domain.Models;
using System.Net.Http;
using HtmlAgilityPack;
using System.Diagnostics;
using Newtonsoft.Json;
using Thor.Domain.Exceptions;

namespace Thor.API.Services
{
    public class StockStrategyService : IStockStrategyService
    {
        private IAlpacaDataClient _dataClient;
     
        public StockStrategyService()
        {
            _dataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(APIManager.APIManager.ALPACA_KEY, APIManager.APIManager.ALPACA_SECRET_KEY));
        }
          

        private async Task<List<Quote>> GetHistorialData(string symbol)
        {
            var to = DateTime.Today;
            var into = to.AddDays(-3);
            var from = into.AddDays(-1000);

            var bars = await _dataClient.ListHistoricalBarsAsync(new HistoricalBarsRequest(symbol, from, into, BarTimeFrame.Day));

            List<Quote> quotes = new List<Quote>();

            foreach(var item in bars.Items)
            {
                quotes.Add(new Quote()
                {
                    Date = item.TimeUtc,
                    Close = item.Close,
                    Open = item.Open,
                    Low = item.Low,
                    High = item.High,
                    Volume = item.Volume
                });
            }

            return quotes;
        }

        public async Task<TopStock> CalculateStockIndicators(string symbol)
        {
            IEnumerable<SmaResult> SMA12Results = new List<SmaResult>();
            IEnumerable<SmaResult> SMA25Results = new List<SmaResult>();
            IEnumerable<SlopeResult> SlopeResults = new List<SlopeResult>();
            IEnumerable<RsiResult> RSIResults = new List<RsiResult>();
            IEnumerable<MacdResult> MACDResults = new List<MacdResult>();

            List<Quote> stockData = new List<Quote>();

            //stockData = await GetHistorialData(symbol);
            try
            {
                stockData = await GetHistorialData(symbol);
            }
            catch(BadQuotesException)
            {

            }


            SMA12Results = stockData.GetSma(12);
            SMA25Results = stockData.GetSma(25);
            SlopeResults = stockData.GetSlope(14);
            RSIResults = stockData.GetRsi();
            MACDResults = stockData.GetMacd();

            TopStock topStock = new TopStock()
            {
                Symbol = symbol,
                SMA12 = SMA12Results.Last().Sma,
                SMA25 = SMA25Results.Last().Sma,
                MACD = MACDResults.Last().Macd,
                RSI = RSIResults.Last().Rsi,
                Slope = SlopeResults.Last().Slope

            };

            return topStock;
        }

        public async Task<List<string>> GetLargestGainStocks()
        {
            List<string> symbols = new List<string>();

            string url = "https://finance.yahoo.com/u/yahoo-finance/watchlists/fiftytwo-wk-gain";

            HttpClient httpClient = new HttpClient();

            string html = await httpClient.GetStringAsync(url);

            HtmlDocument htmlDocument = new HtmlDocument();


            htmlDocument.LoadHtml(html);

            var stocks = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("cwl-symbols W(100%)")).ToList();

            var StockList = stocks[0].Descendants("a")
                .Where(node => node.GetAttributeValue("href", "").Contains("/quote/")).ToList();


            foreach(var item in StockList)
            {
                symbols.Add(item.InnerHtml.ToString());
            }

            return symbols;

        }

        public async Task<List<string>> GetBiggestStockLosers()
        {
            var url = "https://finance.yahoo.com/u/yahoo-finance/watchlists/fiftytwo-wk-loss";

            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();


            htmlDocument.LoadHtml(html);

            var stocks = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("cwl-symbols W(100%)")).ToList();

            var StockList = stocks[0].Descendants("a")
                .Where(node => node.GetAttributeValue("href", "").Contains("/quote/")).ToList();


            List<string> symbols = new List<string>();

            foreach (var item in StockList)
            {
                symbols.Add(item.InnerHtml.ToString());

            }

            return symbols;
        }

        public async Task<PremarketStock> GetPremarketStocks(string symbol)
        {

            var url = "https://finance.yahoo.com/quote/" + symbol + "?p=" + symbol + "&.tsrc=fin-srch";
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();


            htmlDocument.LoadHtml(html);

            var stocks = htmlDocument.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("C($primaryColor) Fz(24px) Fw(b)")).ToList();

            //GET THE PREMARKET VALUE
            double preMarketPrice = Convert.ToDouble(stocks[0].InnerHtml.ToString());

            //GET THE PREVIOUS CLOSE VALUE

            double closePrice = await GetCurrentPrice(symbol);

            //CREATE THE PREMARKETSTOCK

            return new PremarketStock()
            {
                Symbol = symbol,
                PremarketPrice = preMarketPrice,
                PreviousClosePrice = closePrice
            };

        }

        private async Task<double> GetCurrentPrice(string symbol)
        {
            using (HttpClient client = new HttpClient())
            {
                string uri = "https://financialmodelingprep.com/api/v3/quote-short/" + symbol + "?apikey=b9d949fca460c4befd7defef11687656";

                HttpResponseMessage response = await client.GetAsync(uri);

                string jsonResponse = await response.Content.ReadAsStringAsync();

                string newJson = SanitizeJson(jsonResponse);

                StockQuote stockQuote = JsonConvert.DeserializeObject<StockQuote>(newJson);

                return stockQuote.price;


            }
        }

        private string SanitizeJson(string json)
        {
            string updatedJsonMessage = json.Replace("[", "").Replace("]", "");


            return updatedJsonMessage;

        }


    }
}
