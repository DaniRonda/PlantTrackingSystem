using Microsoft.AspNetCore.Mvc;
using service;
using infrastructure.DataModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantsController : ControllerBase
    {
        private readonly PlantService _plantService;
        private readonly ILogger<PlantsController> _logger;

        public PlantsController(PlantService plantService, ILogger<PlantsController> logger)
        {
            _plantService = plantService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetPlants()
        {
            try
            {
                var plants = _plantService.GetPlants();
                return Ok(plants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener las plantas: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPlantById(int id)
        {
            try
            {
                var plant = _plantService.GetPlantById(id);
                if (plant == null)
                {
                    return NotFound();
                }
                return Ok(plant);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la planta con id {id}: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreatePlant([FromBody] Plant plant)
        {
            if (plant == null || string.IsNullOrEmpty(plant.PlantName) || string.IsNullOrEmpty(plant.PlantType) || string.IsNullOrEmpty(plant.Location))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            try
            {
                _plantService.CreatePlant(plant);
                return CreatedAtAction(nameof(GetPlantById), new { id = plant.Id }, plant);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear una nueva planta: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdatePlant(int id, [FromBody] Plant plant)
        {
            try
            {
                if (id != plant.Id)
                {
                    return BadRequest();
                }

                var updated = _plantService.UpdatePlant(plant);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la planta con id {id}: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlant(int id)
        {
            try
            {
                var deleted = _plantService.DeletePlant(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la planta con id {id}: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("search/name/{name}")]
        public IActionResult GetPlantsByName(string name)
        {
            try
            {
                var plants = _plantService.GetPlantsByName(name);
                return Ok(plants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener las plantas por nombre: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("search/location/{location}")]
        public IActionResult GetPlantsByLocation(string location)
        {
            try
            {
                var plants = _plantService.GetPlantsByLocation(location);
                return Ok(plants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener las plantas por ubicación: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("search/type/{type}")]
        public IActionResult GetPlantsByType(string type)
        {
            try
            {
                var plants = _plantService.GetPlantsByType(type);
                return Ok(plants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener las plantas por tipo: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
