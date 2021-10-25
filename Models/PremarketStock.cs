using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Models
{
    public class PremarketStock : INotifyPropertyChanged
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

        private double premarketPrice;
        public double PremarketPrice
        {
            get
            {
                return premarketPrice;
            }
            set
            {
                premarketPrice = value;
                OnPropertyChanged(nameof(PremarketPrice));

            }
        }

        private double previousClosePrice;
        public double PreviousClosePrice
        {
            get
            {
                return previousClosePrice;
            }
            set
            {
                previousClosePrice = value;
                OnPropertyChanged(nameof(PreviousClosePrice));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
