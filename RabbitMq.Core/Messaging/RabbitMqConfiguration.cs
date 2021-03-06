// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace RabbitMq.Shared.Messaging
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public int Port { get; set; }
        
        public string ContentType { get; set; }
        
        public string Exchange { get; set; }

        public int RetryCount { get; set; }

        /// <summary>
        /// Connection retry delay in seconds
        /// </summary>
        public double RetryDelayMs { get; set; }
    }
}