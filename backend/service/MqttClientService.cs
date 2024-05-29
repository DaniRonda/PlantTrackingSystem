/*using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using infrastructure;
using infrastructure.DataModels;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace service
{
    public class MqttClientService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly DataRecordRepository _repository;

        public MqttClientService(IConfiguration configuration, DataRecordRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId(_configuration["Mqtt:ClientId"])
                .WithTcpServer(_configuration["Mqtt:Server"], int.Parse(_configuration["Mqtt:Port"]))
                .WithCredentials(_configuration["Mqtt:Username"], _configuration["Mqtt:Password"])
                .WithTls()
                .Build();

            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(mqttOptions)
                .Build();

            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            await mqttClient.StartAsync(options);

            await mqttClient.SubscribeAsync(new List<MqttTopicFilter> { new MqttTopicFilter { Topic = "sensor/data", QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce } });


            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Message received: {payload}");

                var dataRecord = JsonConvert.DeserializeObject<DataRecord>(payload);
                _repository.AddDataRecord(int idPlant, DateTime dateTime, decimal temperature, decimal humidity);
                await Task.CompletedTask;
            };

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}*/
