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
    public class SearchSymbolCommand : ICommand
    {
        private ISearchSymbolViewModel _viewModel;
        private IBuyStockService _buyStockService;

        public event EventHandler CanExecuteChanged;

        public SearchSymbolCommand(ISearchSymbolViewModel viewModel, IBuyStockService service)
        {
            _viewModel = viewModel;
            _buyStockService = service;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _viewModel.StatusMessage = "";
            try
            {
                ValidateSymbol(_viewModel.Symbol);

                double stockPrice = await _buyStockService.GetCurrentPrice(_viewModel.Symbol);

                _viewModel.SearchResultSymbol = _viewModel.Symbol.ToUpper();
                _viewModel.StockPrice = stockPrice;
            }
            catch (InvalidSymbolException)
            {
                _viewModel.StatusMessage = "Invalid symbol";
            }
            catch (Exception)
            {
                _viewModel.StatusMessage = "Failed to get symbol information";
            }

        }

        private static void ValidateSymbol(string symbol)
        {
            if(symbol == null)
            {
                throw new InvalidSymbolException(symbol);
            }
        }
    }
}
