using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.Domain.Exceptions;
using Thor.Domain.Services;
using Thor.ViewModels;

namespace Thor.Commands
{
    public class SellStockCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        private readonly SellViewModel _sellViewModel;
        private readonly ISellStockService _sellStockService;

        public SellStockCommand(SellViewModel sellViewModel, ISellStockService sellStockService)
        {
            _sellViewModel = sellViewModel;
            _sellStockService = sellStockService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _sellViewModel.StatusMessage = "";

            try
            {
                bool transaction = await _sellStockService.sellStock(_sellViewModel.Symbol, _sellViewModel.SharesToSell);

                _sellViewModel.StatusMessage = "Transaction Success";

            }
            catch (InvalidSymbolException)
            {
                _sellViewModel.StatusMessage = "Invalid Symbol";
            }
            catch (MarketClosedException)
            {
                _sellViewModel.StatusMessage = "Market is currently closed";
            }
            catch (StockNotOwnedException)
            {
                _sellViewModel.StatusMessage = "Can't sell a stock you don't own";
            }
            catch(TooManySharesException)
            {
                _sellViewModel.StatusMessage = "You don't own this amount of stocks to sell";
            }
            catch (Exception)
            {
                _sellViewModel.StatusMessage = "Transaction Failed";
            }
        }
    }
}
