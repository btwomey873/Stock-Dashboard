using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Models;
using Thor.Domain.Services;
using Newtonsoft.Json;
using Alpaca.Markets;
using Thor.API.APIManager;

namespace Thor.API.Services
{
    public class AccountService : IAccountService
    {
        private IAlpacaTradingClient _client;
      
        public AccountService()
        {
            _client = Environments.Paper.GetAlpacaTradingClient(new SecretKey(APIManager.APIManager.ALPACA_KEY, APIManager.APIManager.ALPACA_SECRET_KEY));
        }
        public async Task<UserBalance> GetBalance()
        {
            var account = await _client.GetAccountAsync();

            UserBalance balance = new UserBalance();
            balance.BuyingPower = account.BuyingPower;
            balance.Equity = account.Equity;

            return balance;
        }

        public async Task<List<OwnedStock>> GetCurrentOwnedStocks()
        {
            List<OwnedStock> OwnedStocks = new List<OwnedStock>();

            var positions = await _client.ListPositionsAsync();

            foreach(var item in positions)
            {
                OwnedStocks.Add(new OwnedStock()
                {
                    Symbol = item.Symbol,
                    Price = item.MarketValue / item.Quantity,
                    Shares = item.Quantity,
                    MarketValue = item.MarketValue,
                    TotalProfit = item.MarketValue - (item.AverageEntryPrice * item.Quantity)
                });
            }

            return OwnedStocks;

        }
    }
}
