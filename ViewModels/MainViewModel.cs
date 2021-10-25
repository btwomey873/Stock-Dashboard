using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.Commands;
using Thor.Navigators;
using Thor.ViewModels.Factories;

namespace Thor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _viewModelFactory;

        public INavigator Navigator { get; set; }

        public ICommand UpdateCurrentViewModelCommand { get; }

        public MainViewModel(INavigator navigator, IViewModelFactory viewModelFactory)
        {

            Navigator = navigator;
            _viewModelFactory = viewModelFactory;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Portfolio);


            //UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }
    }
}
