using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Exceptions;
using Thor.Domain.Models;
using Thor.Domain.Services;

namespace Thor.API.Services
{
    public class SellStockService : ISellStockService
    {
        private readonly IAccountService _accountService;

        private IAlpacaTradingClient _client;
       
        public SellStockService(IAccountService accountService)
        {
            _accountService = accountService;
            _client = Environments.Paper.GetAlpacaTradingClient(new SecretKey(APIManager.APIManager.ALPACA_KEY, APIManager.APIManager.ALPACA_SECRET_KEY));
        }


        public async Task<bool> sellStock(string symbol, int shares)
        {
            List<OwnedStock> OwnedStocks = new List<OwnedStock>();

            OwnedStocks = await _accountService.GetCurrentOwnedStocks();

            var clock = await _client.GetClockAsync();

            DateTime Now = new DateTime();

            if (!clock.IsOpen)
            {
                throw new MarketClosedException(Now);
            }

            OwnedStock stock = OwnedStocks.Find(x => x.Symbol == symbol);

           
            if (stock == null)
            {
                throw new StockNotOwnedException(stock.Symbol);
            }

            if(stock.Shares < shares)
            {
                throw new TooManySharesException(stock.Shares);
            }


            var order = await _client.PostOrderAsync(MarketOrder.Sell(symbol, shares));

            return true;




        }
    }
}
