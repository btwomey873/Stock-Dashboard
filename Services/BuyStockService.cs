using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Services;
using Newtonsoft.Json;
using System.Net.Http;
using Thor.API.APIModel;
using Thor.Domain.Exceptions;
using Alpaca.Markets;
using Thor.Domain.Models;

namespace Thor.API.Services
{
    public class BuyStockService : IBuyStockService
    {
        private readonly IAccountService _accountService;
           
        private IAlpacaTradingClient _client;
      

        public BuyStockService(IAccountService accountService)
        {
            _accountService = accountService;
            _client = Environments.Paper.GetAlpacaTradingClient(new SecretKey(APIManager.APIManager.ALPACA_KEY, APIManager.APIManager.ALPACA_SECRET_KEY));

        }

        public async Task<bool> BuyStock(string symbol, int shares)
        {
            UserBalance balance = await _accountService.GetBalance();

            double stockPrice = await GetCurrentPrice(symbol);
            double totalPrice = stockPrice * shares;

            var clock = await _client.GetClockAsync();

            DateTime Now = new DateTime();



            if(!clock.IsOpen)
            {
                throw new MarketClosedException(Now);
            }

            if(balance.BuyingPower < (decimal?)totalPrice)
            {
                throw new InsufficientFundsException(balance.BuyingPower, (decimal?)totalPrice);
            }
                
                var order = await _client.PostOrderAsync(MarketOrder.Buy(symbol, shares));

            return true;
        }

       
        public async Task<double> GetCurrentPrice(string symbol)
        {
            using (HttpClient client = new HttpClient())
            {
                string uri = "https://financialmodelingprep.com/api/v3/quote-short/" + symbol + APIManager.APIManager.FINANCIAL_MODEL_KEY;

                HttpResponseMessage response = await client.GetAsync(uri);

                string jsonResponse = await response.Content.ReadAsStringAsync();


                string newJson = SanitizeJson(jsonResponse);



                StockQuote stockQuote = JsonConvert.DeserializeObject<StockQuote>(newJson);

                if (stockQuote == null)
                {
                    throw new InvalidSymbolException(symbol);
                }

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
