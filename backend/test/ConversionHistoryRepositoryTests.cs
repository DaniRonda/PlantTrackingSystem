using Npgsql;
using infrastructure;
using infrastructure.DataModels;

namespace test
{
    public class PlantRepositoryTests : IDisposable
    {
        private readonly PlantRepository _repository;
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        public PlantRepositoryTests()
        {
            var connectionString = "Host=abul.db.elephantsql.com;Username=tivogyll;Password=D_aMUJJ9FWKwv0clIEsj4hoJzhuCf10E;Database=tivogyll";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _repository = new PlantRepository(_connection);
        }

        [Fact]
        public void GetPlants_ShouldReturnAllPlants()
        {
            // Act
            var result = _repository.GetPlants();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlantById_ShouldReturnPlantById()
        {
            // Arrange
            int id = 1;

            // Act
            var result = _repository.GetPlantById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public void CreatePlant_ShouldInsertPlant()
        {
            // Arrange
            var newPlant = new Plant { PlantName = "Rose", PlantType = "Flower", Location = "Garden" };

            // Act
            _repository.CreatePlant(newPlant);
            var result = _repository.GetPlantById(newPlant.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newPlant.PlantName, result.PlantName);
        }

        [Fact]
        public void UpdatePlant_ShouldUpdatePlant()
        {
            // Arrange
            var plantToUpdate = new Plant { Id = 1, PlantName = "Rose", PlantType = "Flower", Location = "Garden" };

            // Act
            var result = _repository.UpdatePlant(plantToUpdate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeletePlant_ShouldDeletePlant()
        {
            // Arrange
            int id = 1;

            // Act
            var result = _repository.DeletePlant(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetPlantsByName_ShouldReturnPlantsByName()
        {
            // Arrange
            string plantName = "Rose";

            // Act
            var result = _repository.GetPlantsByName(plantName);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlantsByLocation_ShouldReturnPlantsByLocation()
        {
            // Arrange
            string location = "Garden";

            // Act
            var result = _repository.GetPlantsByLocation(location);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPlantsByType_ShouldReturnPlantsByType()
        {
            // Arrange
            string plantType = "Flower";

            // Act
            var result = _repository.GetPlantsByType(plantType);

            // Assert
            Assert.NotNull(result);
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
