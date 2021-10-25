using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Thor.API.Services;
using Thor.Domain.Services;
using Thor.Navigators;
using Thor.ViewModels;
using Thor.ViewModels.Factories;


namespace Thor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
         
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IStockStrategyService, StockStrategyService>();
            services.AddSingleton<IBuyStockService, BuyStockService>();
            services.AddSingleton<ISellStockService, SellStockService>();
            services.AddSingleton<IStockChartService, StockChartService>();
          
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<StrategyViewModel>();
            services.AddSingleton<PortfolioViewModel>();
            services.AddSingleton<BuyViewModel>();
            services.AddSingleton<SellViewModel>();
            services.AddSingleton<CandleStickViewModel>();


            services.AddSingleton<CreateViewModel<PortfolioViewModel>>(services =>
            {
                return () => new PortfolioViewModel(services.GetRequiredService<IAccountService>());
                    
            });

            services.AddSingleton<CreateViewModel<StrategyViewModel>>(services =>
            {
                return () => new StrategyViewModel(
                    services.GetRequiredService<IStockStrategyService>());
                   
            });

            services.AddSingleton<CreateViewModel<BuyViewModel>>(services =>
            {
                return () => services.GetRequiredService<BuyViewModel>();
            });

            services.AddSingleton<CreateViewModel<SellViewModel>>(services =>
            {
                return () => new SellViewModel(
                    services.GetRequiredService<ISellStockService>(),
                    services.GetRequiredService<IAccountService>(),
                    services.GetRequiredService<IBuyStockService>());
            });

            services.AddSingleton<CreateViewModel<CandleStickViewModel>>(services =>
              {
                  return () => CandleStickViewModel.LoadViewModel(
                      services.GetRequiredService<IStockChartService>(),
                      services.GetRequiredService<IAccountService>(),
                      services.GetRequiredService<IStockStrategyService>(),
                      services.GetRequiredService<IBuyStockService>());

              });

            
            services.AddSingleton<INavigator, Navigator>();
        
            services.AddScoped<MainViewModel>();
            services.AddScoped<BuyViewModel>();
            services.AddScoped<SellViewModel>();
           
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
