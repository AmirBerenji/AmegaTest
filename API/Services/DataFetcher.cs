namespace API.Services
{
    public class DataFetcher : IDataFetcher
    {
        private readonly IConfiguration _configuration;

        public DataFetcher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> GetAvailableInstruments()
        {
            return new List<string> { "EURUSD", "USDJPY", "BTCUSD" };
        }

        public async Task<string> GetCurrentPriceAsync(string instrument)
        {
            string from, to;
            switch (instrument)
            {
                case "EURUSD":
                    from = "EUR";
                    to = "USD";
                    break;
                case "USDJPY":
                    from = "USD";
                    to = "JPY";
                    break;
                case "BTCUSD":
                    from = "BTC";
                    to = "USD";
                    break;
                default:
                    return null;
                    break;
            }

            return await CallServiceApi(from, to);

        }

        private async Task<string> CallServiceApi(string from, string to)
        {
            var apiKey = _configuration["AlphaVantage:ApiKey"];
            var api = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=" + from + "&to_currency=" + to + "&apikey=" + apiKey;
            using (HttpClient client = new HttpClient())
            {

                var request = new HttpRequestMessage(HttpMethod.Get, api);
                request.Headers.Add("Accept", "application/json");
                var response = client.SendAsync(request).Result;
                var prediction = await response.Content.ReadAsStringAsync();
                client.Dispose();

                return prediction;
            }
        }
    }

}
