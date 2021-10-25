namespace Thor.ViewModels
{
    public interface ISearchSymbolViewModel
    {
        string SearchResultSymbol { get; set; }
        string StatusMessage { set; }
        double StockPrice { get; set; }
        string Symbol { get; }
    }
}