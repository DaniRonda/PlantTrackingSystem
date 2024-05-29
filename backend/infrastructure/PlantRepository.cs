using Dapper;
using Npgsql;
using infrastructure.DataModels;
using System.Collections.Generic;

namespace infrastructure
{
    public class PlantRepository
    {
        private readonly NpgsqlConnection _connection;

        public PlantRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Plant> GetPlants()
        {
            string sql = @"
                SELECT 
                    id_plant as Id,
                    plant_name as PlantName,
                    plant_type as PlantType,
                    location
                FROM plants;";

            return _connection.Query<Plant>(sql);
        }

        public Plant GetPlantById(int id)
        {
            string sql = @"
                SELECT 
                    id_plant as Id,
                    plant_name as PlantName,
                    plant_type as PlantType,
                    location
                FROM plants
                WHERE id_plant = @Id;";

            return _connection.QueryFirstOrDefault<Plant>(sql, new { Id = id });
        }

        public void CreatePlant(Plant plant)
        {
            string sqlGetLastId = "SELECT MAX(id_plant) FROM plants";
            int lastId = _connection.ExecuteScalar<int>(sqlGetLastId);
            int newPlantId = lastId + 1;
            
            plant.Id = newPlantId;
            
            string sqlInsertPlant = @"
        INSERT INTO plants (id_plant, plant_name, plant_type, location) 
        VALUES (@Id, @PlantName, @PlantType, @Location)";
    
            _connection.Execute(sqlInsertPlant, plant);
        }


        public bool UpdatePlant(Plant plant)
        {
            string sql = @"
                UPDATE plants 
                SET plant_name = @PlantName,
                    plant_type = @PlantType,
                    location = @Location
                WHERE id_plant = @Id;";

            int rowsAffected = _connection.Execute(sql, plant);
            return rowsAffected > 0;
        }

        public bool DeletePlant(int id)
        {
            string sql = @"
                DELETE FROM plants 
                WHERE id_plant = @Id;";
            int rowsAffected = _connection.Execute(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public IEnumerable<Plant> GetPlantsByName(string plantName)
        {
            string sql = @"
                SELECT 
                    id_plant as Id,
                    plant_name as PlantName,
                    plant_type as PlantType,
                    location
                FROM plants
                WHERE plant_name ILIKE @PlantName;";

            return _connection.Query<Plant>(sql, new { PlantName = "%" + plantName + "%" });
        }

        public IEnumerable<Plant> GetPlantsByLocation(string location)
        {
            string sql = @"
                SELECT 
                    id_plant as Id,
                    plant_name as PlantName,
                    plant_type as PlantType,
                    location
                FROM plants
                WHERE location ILIKE @Location;";

            return _connection.Query<Plant>(sql, new { Location = "%" + location + "%" });
        }

        public IEnumerable<Plant> GetPlantsByType(string plantType)
        {
            string sql = @"
                SELECT 
                    id_plant as Id,
                    plant_name as PlantName,
                    plant_type as PlantType,
                    location
                FROM plants
                WHERE plant_type ILIKE @PlantType;";

            return _connection.Query<Plant>(sql, new { PlantType = "%" + plantType + "%" });
        }
    }
}
