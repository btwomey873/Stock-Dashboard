using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Extensions;
using Thor.Domain.Models;
using Thor.Domain.Services;

namespace Thor.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        private readonly IStockStrategyService _stockStrategyService;
        private ObservableCollection<TopStock> topStocks;
        private ObservableCollection<TopStock> topGainers;
        private ObservableCollection<PremarketStock> preMarketStocks;
        private ObservableCollection<TopStock> topLosers;

        private readonly BackgroundWorker worker;
        public StrategyViewModel(IStockStrategyService service)
        {
            _stockStrategyService = service;
          

            //worker = new BackgroundWorker();
            //worker.DoWork += Worker_DoWork;
            //worker.RunWorkerAsync();

            TopStocks = new ObservableCollection<TopStock>();
            TopGainers = new ObservableCollection<TopStock>();
            PremarketStocks = new ObservableCollection<PremarketStock>();
            TopLosers = new ObservableCollection<TopStock>();
            TopStockData(_stockStrategyService);
            LoadTopGainers(_stockStrategyService);
            LoadTopLosers(_stockStrategyService);
            //LoadPremarketData(_stockStrategyService);

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            TopStockData(_stockStrategyService);
        }

        public ObservableCollection<TopStock> TopStocks
        {
            get
            {
                return topStocks;
            }
            set
            {
                topStocks = value;
                OnPropertyChanged(nameof(TopStocks));
            }
        }

        public ObservableCollection<TopStock> TopGainers
        {
            get
            {
                return topGainers;
            }
            set
            {
                topGainers = value;
                OnPropertyChanged(nameof(TopGainers));
            }
        }

        public ObservableCollection<TopStock> TopLosers
        {
            get
            {
                return topLosers;
            }
            set
            {
                topLosers = value;
                OnPropertyChanged(nameof(TopLosers));
            }
        }

        public ObservableCollection<PremarketStock> PremarketStocks
        {
            get
            {
                return preMarketStocks;
            }
            set
            {
                preMarketStocks = value;
                OnPropertyChanged(nameof(PremarketStocks));
            }
        }

        private async Task<List<TopStock>> LoadData(IStockStrategyService service)
        {
            List<TopStock> stocks = new List<TopStock>();

            List<string> symbols = new List<string>();
            symbols = Utility.LoadCSV();

            foreach (var sym in symbols)
            {
                stocks.Add(await service.CalculateStockIndicators(sym));
            }

            return stocks;
        }


        public async void TopStockData(IStockStrategyService service)
        {
            
            List<TopStock> stocks = await LoadData(service);
            decimal? smaTolerance = 1m;
            decimal? rsiUpperTolerance = 65m;
            decimal? rsiLowerTolerance = 39m;

            foreach(var item in stocks)
            {
                if(item.SMA12 > item.SMA25 && item.SMA12-item.SMA25 < smaTolerance)
                {
                    TopStocks.Add(item);
                }
            }
        }

        public async void LoadTopGainers(IStockStrategyService service)
        {
           
            List<string> symbols = new List<string>();

            symbols = await service.GetLargestGainStocks();

                      
            foreach (var sym in symbols)
            {
                try
                {
                    TopGainers.Add(await service.CalculateStockIndicators(sym));
                }
                catch (Skender.Stock.Indicators.BadQuotesException) { }
               
   
            }
          
        }


        public async void LoadTopLosers(IStockStrategyService service)
        {
            List<string> symbols = new List<string>();

            symbols = await service.GetBiggestStockLosers();


            foreach (var sym in symbols)
            {
                try
                {
                    TopLosers.Add(await service.CalculateStockIndicators(sym));
                }
                catch (Skender.Stock.Indicators.BadQuotesException) { }


            }

        }

        //public async void LoadPremarketData(IStockStrategyService service)
        //{
        //    List<string> symbols = new List<string>();

        //    symbols = await service.GetLargestGainStocks();


        //    foreach (var sym in symbols)
        //    {

        //        PremarketStock premarketStock = await service.GetPremarketStocks(sym);

        //        if(premarketStock.PreviousClosePrice <  premarketStock.PremarketPrice)
        //        {

        //            PremarketStocks.Add(premarketStock);
        //        }
                
        //    }


        //}

    }
}
