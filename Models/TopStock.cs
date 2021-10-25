using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Models
{
    public class TopStock : INotifyPropertyChanged
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

        //public decimal Price { get; set; }

        private decimal? macd;
        public decimal? MACD
        {
            get
            {
                return macd;
            }
            set
            {
                macd = value;
                OnPropertyChanged(nameof(MACD));

            }
        }

        private decimal? rsi;
        public decimal? RSI
        {
            get
            {
                return rsi;
            }
            set
            {
                rsi = value;
                OnPropertyChanged(nameof(RSI));

            }
        }

        private decimal? sma12;
        public decimal? SMA12
        {
            get
            {
                return sma12;
            }
            set
            {
                sma12 = value;
                OnPropertyChanged(nameof(SMA12));

            }
        }

        private decimal? sma25;
        public decimal? SMA25
        {
            get
            {
                return sma25;
            }
            set
            {
                sma25 = value;
                OnPropertyChanged(nameof(SMA25));

            }
        }

        private decimal? slope;
        public decimal? Slope
        {
            get
            {
                return slope;
            }
            set
            {
                slope = value;
                OnPropertyChanged(nameof(Slope));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
