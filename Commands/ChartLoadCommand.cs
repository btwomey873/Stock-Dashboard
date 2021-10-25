using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.Domain.Exceptions;
using Thor.Domain.Models;
using Thor.Domain.Services;
using Thor.ViewModels;

namespace Thor.Commands
{
    public class ChartLoadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private CandleStickViewModel _viewModel;
        private IBuyStockService _service;
        private IStockStrategyService _strategyService;
        public ChartLoadCommand(CandleStickViewModel viewModel, IBuyStockService service, IStockStrategyService stockStrategyService)
        {
            _viewModel = viewModel;
            _service = service;
            _strategyService = stockStrategyService;
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

                double stockPrice = await _service.GetCurrentPrice(_viewModel.Symbol);
                TopStock stock = await _strategyService.CalculateStockIndicators(_viewModel.Symbol);

                _viewModel.MACD = stock.MACD;
                _viewModel.SMA12 = stock.SMA12;
                _viewModel.SMA25 = stock.SMA25;

                _viewModel.SearchResultSymbol = _viewModel.Symbol.ToUpper();

                await _viewModel.LoadChartData(_viewModel.Symbol);
                
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
            if (symbol == null)
            {
                throw new InvalidSymbolException(symbol);
            }
        }
    }
}
