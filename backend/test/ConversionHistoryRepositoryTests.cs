using Npgsql;
using infrastructure;

namespace test
{
    public class ConversionHistoryRepositoryTests : IDisposable
    {
        private readonly ConversionHistoryRepository _repository;
        private readonly string _connectionString = "Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll";
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public ConversionHistoryRepositoryTests()
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _repository = new ConversionHistoryRepository();
        }

        [Fact]
        public void AddConversion_ShouldAddNewConversion()
        {
            // Arrange
            string fromCurrency = "USD";
            string toCurrency = "EUR";
            decimal amount = 100;
            decimal convertedAmount = 93;

            // Act
            _repository.AddConversion(fromCurrency, toCurrency, amount, convertedAmount);

            // Assert
            List<ConversionRecord> conversions = _repository.GetAllConversions();
            Assert.Contains(conversions, c =>
                c.FromCurrency == fromCurrency &&
                c.ToCurrency == toCurrency &&
                c.Amount == amount &&
                c.ConvertedAmount == convertedAmount);
        }

        [Fact]
        public void GetAllConversions_ShouldReturnAllConversions()
        {
            // Act
            List<ConversionRecord> conversions = _repository.GetAllConversions();

            // Assert
            Assert.NotNull(conversions);
            Assert.NotEmpty(conversions);
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
