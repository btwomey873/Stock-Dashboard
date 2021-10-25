using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.API.Services;
using Thor.Domain.Models;
using Thor.Domain.Services;
using System.Timers;
using System.Collections.ObjectModel;
using LiveCharts.Configurations;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Thor.ViewModels
{
    public class PortfolioViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly BackgroundWorker worker;
        private ObservableCollection<OwnedStock> ownedStocks;
       
        public PortfolioViewModel(IAccountService service)
        {
         
            _accountService = service;
            OwnedStocks = new ObservableCollection<OwnedStock>();

            worker = new BackgroundWorker();
            LoadBalance(_accountService);
            LoadStocks(_accountService);
            worker.DoWork += Worker_DoWork;

            Timer timer = new Timer(1500);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
         
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadBalance(_accountService);
        }

        private async void LoadBalance(IAccountService service)
        {
            UserBalance = await service.GetBalance();
        }

        private async void LoadStocks(IAccountService service)
        {
            List<OwnedStock> stocks = await service.GetCurrentOwnedStocks();

            //Should look at just casting List into ObservableCollection OR just using ObservableCollection in the AccountService
            foreach(var item in stocks)
            {
                OwnedStocks.Add(item);
            }
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


        private UserBalance userBalance;
        public UserBalance UserBalance
        {
            get
            {
                return userBalance;
            }
            set
            {
                userBalance = value;
                OnPropertyChanged(nameof(UserBalance));
            }
        }

       

    }
}
