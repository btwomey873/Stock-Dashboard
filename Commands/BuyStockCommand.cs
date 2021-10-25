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
    public class BuyStockCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private BuyViewModel _buyViewModel;
        private IBuyStockService _buyStockService;
    
        public BuyStockCommand(BuyViewModel buyViewModel, IBuyStockService buyStockService)
        {
            _buyViewModel = buyViewModel;
            _buyStockService = buyStockService;
        }



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _buyViewModel.StatusMessage = "";

            try
            {
               bool transaction = await _buyStockService.BuyStock(_buyViewModel.Symbol, _buyViewModel.SharesToBuy);

                _buyViewModel.StatusMessage = "Transaction Success";
               
            }
            catch(InvalidSymbolException)
            {
                _buyViewModel.StatusMessage = "Invalid Symbol";
            }
            catch(MarketClosedException)
            {
                _buyViewModel.StatusMessage = "Market is currently closed";
            }
            catch(InsufficientFundsException)
            {
                _buyViewModel.StatusMessage = "Insufficient funds";
            }
            catch(Exception)
            {
                _buyViewModel.StatusMessage = "Transaction Failed";
            }

       

        }

    }
}
