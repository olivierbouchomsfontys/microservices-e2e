﻿using System.Text;
using CustomerService.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMq.Shared.Messaging;

namespace CustomerService.Messaging
{
    public class CustomerMessagePublisher
    {
        private readonly RabbitMqConfiguration _configuration;

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ()
                    {
                        HostName = _configuration.Hostname,
                        Port = _configuration.Port,
                        UserName = _configuration.UserName,
                        Password = _configuration.Password
                    };
                }

                return _connectionFactory;
            }
        }

        private IConnection Connection => ConnectionFactory.CreateConnection();

        public CustomerMessagePublisher(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }

        public void Send(Customer customer)
        {
            using (var channel = Connection.CreateModel())
            {
                channel.QueueDeclare(_configuration.QueueName, durable: true, exclusive: false, autoDelete: false,
                    arguments: null);

                var json = JsonConvert.SerializeObject(customer);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _configuration.QueueName, basicProperties: null,
                    body: body);
            }
        }
    }
}