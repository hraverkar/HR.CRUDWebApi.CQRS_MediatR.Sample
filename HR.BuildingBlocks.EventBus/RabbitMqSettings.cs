namespace HR.BuildingRabbitMQ.Lib
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = "host.docker.internal";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string QueueName { get; set; } = "hrgitauditevent";
    }
}
