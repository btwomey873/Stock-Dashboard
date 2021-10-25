using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Thor.Domain.Extensions
{
    public static class Utility
    {
        public static List<string> LoadCSV()
        {
            List<string> stockSymbols = new List<string>();

            using (var reader = new StreamReader("C:\\Users\\Ben Twomey\\Desktop\\StockData\\StockData.csv"))
            {
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    stockSymbols.Add(line);
                }
            }

            return stockSymbols;
        }

    }
}
