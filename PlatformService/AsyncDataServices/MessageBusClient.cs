using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public  MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var  factory = new ConnectionFactory() 
            {
                HostName= _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout );

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                 Console.WriteLine($"==> Connected to Message Bus RabbitMQ");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"--> Could not connect to Message Bus:  {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            
            if (_connection.IsOpen)
            {
                 Console.WriteLine($"==> Rabbitmq Connection Open, Send new messages...");
                //  Sending messages
                SendMessage(message);
            }else{
                 Console.WriteLine($"==> RabbitMQ connection is Closed!, don't send in messages");
            }
        }

        private void SendMessage(string message){
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "trigger", 
                routingKey: "",
                body: body
                );
              Console.WriteLine($"==> Message we have sent: {message}");
        }

        public void Dispose()
        {
              Console.WriteLine($"==> Message Bus Disposed.");
              if (_channel.IsOpen)
              {
                _channel.Close();
                _connection.Close();
              }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"==> RabbitMQ connection shutdown!");
;        }
    }
}