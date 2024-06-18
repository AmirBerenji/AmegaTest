using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CurrencyController : BaseApiController
    {
        private readonly IDataFetcher _dataFetcher;

        public CurrencyController(IDataFetcher dataFetcher)
        {
            _dataFetcher = dataFetcher;
        }

        [HttpGet]
        public IActionResult GetInstruments()
        {
            var instruments = _dataFetcher.GetAvailableInstruments();
            return Ok(instruments);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(string instrument)
        {
            var price = await _dataFetcher.GetCurrentPriceAsync(instrument);
            if (string.IsNullOrEmpty(price))
            {
                return NotFound("Instrument not supported");
            }
            return Ok(price);
        }
    }
}
