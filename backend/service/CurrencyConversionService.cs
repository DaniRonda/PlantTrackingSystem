using infrastructure;
namespace service
{
    public class CurrencyConversionService
    {
        private readonly Dictionary<string, decimal> _rates;
        private readonly ConversionHistoryRepository _historyRepository;

        public CurrencyConversionService(Dictionary<string, decimal> rates, ConversionHistoryRepository historyRepository)
        {
            _rates = rates;
            _historyRepository = historyRepository;
        }

        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            if (!_rates.ContainsKey(fromCurrency) || !_rates.ContainsKey(toCurrency))
            {
                throw new ArgumentException("Unsupported currency.");
            }

            decimal rateToUSD = _rates[fromCurrency];
            decimal amountInUSD = amount / rateToUSD;
            decimal targetRate = _rates[toCurrency];
            decimal convertedAmount = amountInUSD * targetRate;
            
            _historyRepository.AddConversion(fromCurrency, toCurrency, amount, convertedAmount);

            return convertedAmount;
        }

        public List<ConversionRecord> GetConversionHistory()
        {
            return _historyRepository.GetAllConversions();
        }
    }
}
