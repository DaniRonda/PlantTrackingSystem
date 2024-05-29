namespace infrastructure.DataModels;

using System;


public class DataRecord
    {
        public int IdRecord { get; set; }
        public int IdPlant { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
