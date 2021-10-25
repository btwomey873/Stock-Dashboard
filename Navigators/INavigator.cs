using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thor.ViewModels;

namespace Thor.Navigators
{
    public enum ViewType
    {
        Portfolio,
        Strategy,
        Buy,
        Sell,
        Login,
        Chart
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        //ICommand UpdateCurrentViewModelCommand { get; }
    }
}
