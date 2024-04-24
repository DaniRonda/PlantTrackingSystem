using Npgsql;

namespace infrastructure
{
    public class ConversionHistoryRepository
    {
        private readonly string _connectionString = "Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll";

        public void AddConversion(string fromCurrency, string toCurrency, decimal amount, decimal convertedAmount)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO ConversionHistory (FromCurrency, ToCurrency, Amount, ConvertedAmount, Date) VALUES (@fromCurrency, @toCurrency, @amount, @convertedAmount, @date)";
                    command.Parameters.AddWithValue("fromCurrency", fromCurrency);
                    command.Parameters.AddWithValue("toCurrency", toCurrency);
                    command.Parameters.AddWithValue("amount", amount);
                    command.Parameters.AddWithValue("convertedAmount", convertedAmount);
                    command.Parameters.AddWithValue("date", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ConversionRecord> GetAllConversions()
        {
            List<ConversionRecord> conversions = new List<ConversionRecord>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM ConversionHistory";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            conversions.Add(new ConversionRecord
                            {
                                Id = reader.GetInt32(0),
                                FromCurrency = reader.GetString(1),
                                ToCurrency = reader.GetString(2),
                                Amount = reader.GetDecimal(3),
                                ConvertedAmount = reader.GetDecimal(4),
                                Date = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }

            return conversions;
        }
    }

    public class ConversionRecord
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
