using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thor.Views
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class SellView : UserControl
    {
        public SellView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectedStockChangedCommandProperty =
            DependencyProperty.Register("SelectedStockChangedCommand", typeof(ICommand), typeof(SellView),
                new PropertyMetadata(null));

        public ICommand SelectedStockChangedCommand
        {
            get
            {
                return (ICommand)GetValue(SelectedStockChangedCommandProperty);
            }
            set
            {
                SetValue(SelectedStockChangedCommandProperty, value);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(assets.SelectedItem != null)
            {
                SelectedStockChangedCommand?.Execute(null);
            }
        }
    }
}
