using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Thor.Commands;
using Thor.Domain.Models;
using Thor.Domain.Services;

namespace Thor.ViewModels
{
    public class SellViewModel : ViewModelBase, ISearchSymbolViewModel
    {
        private readonly IAccountService _accountService;
        private readonly BackgroundWorker worker;
        private ObservableCollection<OwnedStock> ownedStocks;
        public SellViewModel(ISellStockService sellStockService, IAccountService accountService, IBuyStockService buyStockService)
        {
            SearchSymbolCommand = new SearchSymbolCommand(this, buyStockService);
            SellStockCommand = new SellStockCommand(this, sellStockService);

            StatusMessageViewModel = new MessageViewModel();

            _accountService = accountService;
            OwnedStocks = new ObservableCollection<OwnedStock>();
            worker = new BackgroundWorker();

            LoadStocks(_accountService);

        }

      

        public ICommand SearchSymbolCommand { get; set; }
        public ICommand SellStockCommand { get; set; }

       
        public string Symbol => SelectedStock?.Symbol;
   

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

        private int sharesToSell;
        public int SharesToSell
        {
            get
            {
                return sharesToSell;
            }
            set
            {
                sharesToSell = value;
                OnPropertyChanged(nameof(SharesToSell));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public double TotalPrice
        {
            get
            {
                return SharesToSell * StockPrice;
            }
        }

    
        public MessageViewModel StatusMessageViewModel { get; }
        public string StatusMessage
        {
            set => StatusMessageViewModel.Message = value;
        }

        public ObservableCollection<OwnedStock> OwnedStocks
        {
            get
            {
                return ownedStocks;
            }
            set
            {
                ownedStocks = value;
                OnPropertyChanged(nameof(OwnedStocks));
            }
        }

        private OwnedStock selectedStock;
        public OwnedStock SelectedStock
        {
            get
            {
                return selectedStock;
            }
            set
            {
                selectedStock = value;
                OnPropertyChanged(nameof(SelectedStock));
            }
        }


        private async void LoadStocks(IAccountService service)
        {
            List<OwnedStock> stocks = await service.GetCurrentOwnedStocks();

            
            foreach (var item in stocks)
            {
                OwnedStocks.Add(item);
            }
        }
    }
}
