using infrastructure;
using infrastructure.DataModels;
using System.Collections.Generic;

namespace service
{
    public class PlantService
    {
        private readonly PlantRepository _plantRepository;

        public PlantService(PlantRepository plantRepository)
        {
            _plantRepository = plantRepository;
        }

        public IEnumerable<Plant> GetPlants()
        {
            return _plantRepository.GetPlants();
        }

        public Plant GetPlantById(int id)
        {
            return _plantRepository.GetPlantById(id);
        }

        public void CreatePlant(Plant plant)
        {
            _plantRepository.CreatePlant(plant);
        }

        public bool UpdatePlant(Plant plant)
        {
            return _plantRepository.UpdatePlant(plant);
        }

        public bool DeletePlant(int id)
        {
            return _plantRepository.DeletePlant(id);
        }

        public IEnumerable<Plant> GetPlantsByName(string plantName)
        {
            return _plantRepository.GetPlantsByName(plantName);
        }

        public IEnumerable<Plant> GetPlantsByLocation(string location)
        {
            return _plantRepository.GetPlantsByLocation(location);
        }

        public IEnumerable<Plant> GetPlantsByType(string plantType)
        {
            return _plantRepository.GetPlantsByType(plantType);
        }
    }
}