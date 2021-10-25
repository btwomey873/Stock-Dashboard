using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.Commands;
using Thor.Domain.Models;
using Thor.Domain.Services;

namespace Thor.ViewModels
{
    public class CandleStickViewModel : ViewModelBase
    {
        private readonly IStockChartService _stockChartService;
        private readonly IAccountService _accountService;
        private readonly IStockStrategyService _stockStrategyService;
        

        public ICommand ChartLoadCommand { get; set; }


        private List<StockChartDefaultInfo> candlesticks;

        //public CandleStickViewModel() { }
        public CandleStickViewModel(IStockChartService stockChartService, IAccountService accountService, IStockStrategyService stockStrategyService, IBuyStockService buyService)
        {
            candlesticks = new List<StockChartDefaultInfo>();
            _stockChartService = stockChartService;
            _accountService = accountService;
            _stockStrategyService = stockStrategyService;
            ChartLoadCommand = new ChartLoadCommand(this, buyService, stockStrategyService);
            StatusMessageViewModel = new MessageViewModel();
            
        }

        public static CandleStickViewModel LoadViewModel(IStockChartService stockChartService, IAccountService accountService, IStockStrategyService stockStrategy, IBuyStockService buyService, Action<Task> onLoaded = null)
        {
            CandleStickViewModel viewModel = new CandleStickViewModel(stockChartService, accountService, stockStrategy, buyService);
            viewModel.LoadSeriesCollection().ContinueWith(t => onLoaded?.Invoke(t));
            

            return viewModel;
        }


        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get
            {
                return seriesCollection;
            }
            set
            {
                seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private List<string> labels;
        public List<string> Labels
        {
            get
            {
                return labels;
            }
            set
            {
                labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        public async Task LoadSeriesCollection()
        {

            List<OwnedStock> ownedStock = await _accountService.GetCurrentOwnedStocks();

            string initalSymbol = ownedStock.FirstOrDefault().Symbol;
            

            if (initalSymbol != null)
            {
                Symbol = initalSymbol;
                candlesticks = await _stockChartService.GetCandleStickData(initalSymbol);
                TopStock topstock = await _stockStrategyService.CalculateStockIndicators(Symbol);
                MACD = topstock.MACD;
                SMA12 = topstock.SMA12;
                SMA25 = topstock.SMA25;
            }
            else
            {
                candlesticks = await _stockChartService.GetCandleStickData("AAPL");  //defaults to apple stock, might be worht looking into just displaying an initial message to choose a stock?  
                Symbol = "AAPL";
                TopStock topstock = await _stockStrategyService.CalculateStockIndicators(Symbol);
                MACD = topstock.MACD;
                SMA12 = topstock.SMA12;
                SMA25 = topstock.SMA25;
            }

            Points = new ChartValues<OhlcPoint>();
            DateTime now = DateTime.Now;
            Labels = new List<string>();

            foreach (var item in candlesticks)
            {
                Points.Add(new OhlcPoint()
                {
                    Low = item.Low,
                    High = item.High,
                    Open = item.Open,
                    Close = item.Close
                });

                Labels.Add(item.DateTime);
            }


            SeriesCollection = new SeriesCollection
            {
                new CandleSeries
                {
                    Title = "Stock Data",
                    Values = Points
                }
            };

        }

        public async Task LoadChartData(string symbol)
        {
            candlesticks.Clear();
            candlesticks = await _stockChartService.GetCandleStickData(symbol);
            Points = new ChartValues<OhlcPoint>();
            DateTime now = DateTime.Now;
            Labels = new List<string>();

            foreach (var item in candlesticks)
            {
                Points.Add(new OhlcPoint()
                {
                    Low = item.Low,
                    High = item.High,
                    Open = item.Open,
                    Close = item.Close
                });

                Labels.Add(item.DateTime);
            }


            SeriesCollection = new SeriesCollection
            {
                new CandleSeries
                {
                    Title = "Stock Data",
                    Values = Points
                }
            };

        }


        private ChartValues<OhlcPoint> points;
        public ChartValues<OhlcPoint> Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

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

        private decimal? macd;
        public decimal? MACD
        {
            get
            {
                return macd;
            }
            set
            {
                macd = value;
                OnPropertyChanged(nameof(MACD));

            }
        }

        private decimal? rsi;
        public decimal? RSI
        {
            get
            {
                return rsi;
            }
            set
            {
                rsi = value;
                OnPropertyChanged(nameof(RSI));

            }
        }

        private decimal? sma12;
        public decimal? SMA12
        {
            get
            {
                return sma12;
            }
            set
            {
                sma12 = value;
                OnPropertyChanged(nameof(SMA12));

            }
        }

        private decimal? sma25;
        public decimal? SMA25
        {
            get
            {
                return sma25;
            }
            set
            {
                sma25 = value;
                OnPropertyChanged(nameof(SMA25));

            }
        }

        public MessageViewModel StatusMessageViewModel { get; }
        public string StatusMessage
        {
            set => StatusMessageViewModel.Message = value;
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

    }
}
