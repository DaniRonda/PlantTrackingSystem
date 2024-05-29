using System;
using System.Collections.Generic;
using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure
{
    public class DataRecordRepository
    {
        private NpgsqlDataSource _dataSource;

        public DataRecordRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IEnumerable<DataRecord> GetAllDataRecords()
        {
            string sql = $@"
SELECT id_record as {nameof(DataRecord.IdRecord)},
       id_plant as {nameof(DataRecord.IdPlant)},
        date_time as {nameof(DataRecord.DateTime)},
        temperature as {nameof(DataRecord.Temperature)},
        humidity as {nameof(DataRecord.Humidity)}
FROM public.data_records;
";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<DataRecord>(sql);
            }
        }

        public DataRecord AddDataRecord(int idPlant, DateTime dateTime, decimal temperature, decimal humidity)
        {
            var sql = $@"
INSERT INTO public.data_records (id_plant, date_time, temperature, humidity) 
VALUES (@idPlant, @dateTime, @temperature, @humidity)
RETURNING id_record as {nameof(DataRecord.IdRecord)},
       id_plant as {nameof(DataRecord.IdPlant)},
       date_time as {nameof(DataRecord.DateTime)},
        temperature as {nameof(DataRecord.Temperature)},
        humidity as {nameof(DataRecord.Humidity)}
";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirst<DataRecord>(sql, new { idPlant, dateTime, temperature, humidity });
            }
        }

        public IEnumerable<DataRecord> GetDataRecordsByDate(int idPlant, DateTime date)
        {
            string sql = $@"
    SELECT id_record as {nameof(DataRecord.IdRecord)},
           id_plant as {nameof(DataRecord.IdPlant)},
           date_time as {nameof(DataRecord.DateTime)},
           temperature as {nameof(DataRecord.Temperature)},
           humidity as {nameof(DataRecord.Humidity)}
    FROM public.data_records
    WHERE id_plant = @idPlant AND date_trunc('day', date_time) = @date;
    ";

            using (var conn = _dataSource.OpenConnection())
            {
                return conn.Query<DataRecord>(sql, new { idPlant, date = date.Date });
            }
        }

        public DataRecord GetDataRecordById(int idRecord)
        {
            string sql = $@"
SELECT id_record as {nameof(DataRecord.IdRecord)},
       id_plant as {nameof(DataRecord.IdPlant)},
        date_time as {nameof(DataRecord.DateTime)},
        temperature as {nameof(DataRecord.Temperature)},
        humidity as {nameof(DataRecord.Humidity)}
FROM public.data_records
WHERE id_record = @IdRecord;
";
            using (var conn = _dataSource.OpenConnection())
            {
                return conn.QueryFirstOrDefault<DataRecord>(sql, new { IdRecord = idRecord });
            }
        }
    }
}
