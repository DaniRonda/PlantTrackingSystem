using infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using service;
using Microsoft.Extensions.Logging;
using System;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataRecordController : ControllerBase
    {
        private readonly DataRecordService _dataRecordService;
        private readonly ILogger<DataRecordController> _logger;

        public DataRecordController(ILogger<DataRecordController> logger, DataRecordService dataRecordService)
        {
            _dataRecordService = dataRecordService;
            _logger = logger;
        }

        [HttpGet("data")]
        public IActionResult Get()
        {
            try
            {
                var dataRecords = _dataRecordService.GetDataRecords();
                return Ok(dataRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los registros de datos: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public IActionResult Post([FromBody] DataRecord sensorData)
        {
            try
            {
                _dataRecordService.PostSensorData(sensorData.IdPlant, sensorData.DateTime, sensorData.Temperature, sensorData.Humidity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar un nuevo registro de datos: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("plant/{idPlant}/date")]
        public IActionResult GetByDate(int idPlant, [FromQuery] DateTime date)
        {
            try
            {
                var dataRecords = _dataRecordService.GetDataRecordsByDate(idPlant, date);
                return Ok(dataRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los registros de datos por fecha: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        
        [HttpGet("data/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var dataRecord = _dataRecordService.GetDataRecordById(id);
                if (dataRecord == null)
                {
                    return NotFound($"Registro de datos con ID {id} no encontrado.");
                }
                return Ok(dataRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el registro de datos por ID: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
