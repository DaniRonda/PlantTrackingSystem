using infrastructure;
using infrastructure.DataModels;

namespace service;

public class DataRecordService
{
    private readonly DataRecordRepository _dataRecordRepository;

    public DataRecordService(DataRecordRepository dataRecordRepository)
    {
        _dataRecordRepository = dataRecordRepository;
    }

    public IEnumerable<DataRecord> GetDataRecords()
    {
        return _dataRecordRepository.GetAllDataRecords();
    }

    public DataRecord PostSensorData(int idPlant, DateTime dateTime, decimal temperature, decimal humidity)
    {
        return _dataRecordRepository.AddDataRecord(idPlant, dateTime, temperature, humidity);
    }
    
    public IEnumerable<DataRecord> GetDataRecordsByDate(int idPlant, DateTime date)
    {
        return _dataRecordRepository.GetDataRecordsByDate(idPlant, date);
    }

    public DataRecord GetDataRecordById (int idRecord)
    {
        return _dataRecordRepository.GetDataRecordById (idRecord);
    }
}