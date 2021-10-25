using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Navigators;
using static Thor.ViewModels.ViewModelBase;

namespace Thor.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
       
        private readonly CreateViewModel<PortfolioViewModel> _createPortfolioViewModel;
        private readonly CreateViewModel<StrategyViewModel> _createStrategyViewModel;
        private readonly CreateViewModel<BuyViewModel> _createBuyViewModel;
        private readonly CreateViewModel<SellViewModel> _createSellViewModel;
        private readonly CreateViewModel<CandleStickViewModel> _createCandleStickViewModel;
        

        public ViewModelFactory(CreateViewModel<PortfolioViewModel> createPortfolioViewModel,
            CreateViewModel<StrategyViewModel> createStrategyViewModel, CreateViewModel<BuyViewModel> createBuyViewModel,
            CreateViewModel<SellViewModel> createSellViewModel, CreateViewModel<CandleStickViewModel> createCandleStickViewModel)
        {
            _createPortfolioViewModel = createPortfolioViewModel;
            _createStrategyViewModel = createStrategyViewModel;
            _createBuyViewModel = createBuyViewModel;
            _createSellViewModel = createSellViewModel;
            _createCandleStickViewModel = createCandleStickViewModel;
            
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch(viewType)
            {
                case ViewType.Portfolio:
                    return _createPortfolioViewModel();
                case ViewType.Strategy:
                    return _createStrategyViewModel();
                case ViewType.Buy:
                    return _createBuyViewModel();
                case ViewType.Sell:
                    return _createSellViewModel();
                case ViewType.Chart:
                    return _createCandleStickViewModel();
                default:
                    throw new ArgumentException("Viewtype not supported");

            }
        }

    
    }
}
