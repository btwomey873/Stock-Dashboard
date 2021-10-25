using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Models
{
    public class UserBalance : INotifyPropertyChanged
    {
        private decimal? buyingPower;
        public decimal? BuyingPower
        {
            get
            {
                return buyingPower;
            }
            set
            {
                buyingPower = value;
                OnPropertyChanged(nameof(BuyingPower));
                OnPropertyChanged(nameof(Total));
            }
        }

        private decimal? equity;
        public decimal? Equity
        {
            get
            {
                return equity;
            }
            set
            {
                equity = value;
                OnPropertyChanged(nameof(Equity));
                OnPropertyChanged(nameof(Total));
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        private decimal? total;
        public decimal? Total
        {
            get
            {
                return Equity + BuyingPower;
            }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private decimal? totalProfit;
        public decimal? TotalProfit
        {
            get
            {
                return Equity - 1000;
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
