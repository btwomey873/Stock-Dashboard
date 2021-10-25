using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.Commands;
using Thor.Domain.Services;

namespace Thor.ViewModels
{
    public class BuyViewModel : ViewModelBase, ISearchSymbolViewModel
    {
        public BuyViewModel(IBuyStockService buyStockService, IAccountService accountService)
        {
            SearchSymbolCommand = new SearchSymbolCommand(this, buyStockService);
            BuyStockCommand = new BuyStockCommand(this, buyStockService);
            StatusMessageViewModel = new MessageViewModel();
        }

        public ICommand SearchSymbolCommand { get; set; }
        public ICommand BuyStockCommand { get; set; }

        private string symbol;
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
                OnPropertyChanged(nameof(Symbol));
            }
        }

        private string searchResultSymbol = string.Empty;
        public string SearchResultSymbol
        {
            get
            {
                return searchResultSymbol;
            }
            set
            {
                searchResultSymbol = value;
                OnPropertyChanged(nameof(SearchResultSymbol));
            }
        }

        private double stockPrice;
        public double StockPrice
        {
            get
            {
                return stockPrice;
            }
            set
            {
                stockPrice = value;
                OnPropertyChanged(nameof(StockPrice));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private int sharesToBuy;
        public int SharesToBuy
        {
            get
            {
                return sharesToBuy;
            }
            set
            {
                sharesToBuy = value;
                OnPropertyChanged(nameof(SharesToBuy));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public double TotalPrice
        {
            get
            {
                return SharesToBuy * StockPrice;
            }
        }

        public MessageViewModel StatusMessageViewModel { get; }
        public string StatusMessage
        {
            set => StatusMessageViewModel.Message = value;
        }




    }
}
