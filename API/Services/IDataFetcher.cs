namespace API.Services
{
    public interface IDataFetcher
    {
        List<string> GetAvailableInstruments();
        Task<string> GetCurrentPriceAsync(string instrument);
    }

}
