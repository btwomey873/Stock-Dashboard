using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Models
{
    public class OwnedStock : INotifyPropertyChanged
    {
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

        private decimal? price;
        public decimal? Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private decimal shares;
        public decimal Shares
        {
            get
            {
                return shares;
            }
            set
            {
                shares = value;
                OnPropertyChanged(nameof(Shares));
            }
        }

        private decimal? marketValue;
        public decimal? MarketValue
        {
            get
            {
                return marketValue;
            }
            set
            {
                marketValue = value;
                OnPropertyChanged(nameof(MarketValue));
            }
        }

        private decimal? totalProfit;
        public decimal? TotalProfit
        {
            get
            {
                return totalProfit;
            }
            set
            {
                totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
