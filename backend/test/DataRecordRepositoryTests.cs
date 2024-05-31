using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using Moq;
using Xunit;
using infrastructure;
using infrastructure.DataModels;

namespace test
{
    public class DataRecordRepositoryTests : IDisposable
    {
        private readonly DataRecordRepository _repository;
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        public DataRecordRepositoryTests()
        {
            var connectionString = "Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            var dataSourceMock = new Mock<NpgsqlDataSource>();
            dataSourceMock.Setup(ds => ds.OpenConnection()).Returns(_connection);

            _repository = new DataRecordRepository(dataSourceMock.Object);
        }

        [Fact]
        public void GetAllDataRecords_ShouldReturnDataRecords()
        {
            // Act
            var result = _repository.GetAllDataRecords();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddDataRecord_ShouldInsertDataRecord()
        {
            // Arrange
            int idPlant = 1;
            DateTime dateTime = DateTime.Now;
            decimal temperature = 25.0m;
            decimal humidity = 60.0m;

            // Act
            var result = _repository.AddDataRecord(idPlant, dateTime, temperature, humidity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idPlant, result.IdPlant);
            Assert.Equal(temperature, result.Temperature);
            Assert.Equal(humidity, result.Humidity);
        }

        [Fact]
        public void GetDataRecordsByDate_ShouldReturnRecordsForGivenDate()
        {
            // Arrange
            int idPlant = 1;
            DateTime date = DateTime.Now.Date;

            // Act
            var result = _repository.GetDataRecordsByDate(idPlant, date);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetDataRecordById_ShouldReturnRecordById()
        {
            // Arrange
            int idRecord = 1;

            // Act
            var result = _repository.GetDataRecordById(idRecord);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idRecord, result.IdRecord);
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
