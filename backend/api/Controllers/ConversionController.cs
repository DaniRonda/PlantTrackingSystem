using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversionController : ControllerBase
    {
        private readonly CurrencyConversionService _conversionService;

        public ConversionController(CurrencyConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost ("ConvertCurrency")]
        public IActionResult ConvertCurrency([FromBody] ConversionRequest request)
        {
            try
            {
                decimal convertedAmount =
                    _conversionService.ConvertCurrency(request.Amount, request.FromCurrency, request.ToCurrency);
                return Ok(new
                {
                    Amount = request.Amount, FromCurrency = request.FromCurrency, ConvertedAmount = convertedAmount,
                    ToCurrency = request.ToCurrency
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("history")]
        public IActionResult GetConversionHistory()
        {
            try
            {
                var history = _conversionService.GetConversionHistory();
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting conversion histories: {ex.Message}");
            }
        }


        public class ConversionRequest
        {
            public decimal Amount { get; set; }
            public string FromCurrency { get; set; }
            public string ToCurrency { get; set; }
        }
    }
}