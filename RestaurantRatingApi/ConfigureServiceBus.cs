using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRatingApi
{
    public interface IConfigureServiceBus
    {
        string ConnectionStringServiceBus { get; set; }
        string ConnectionStringBlobStorage { get; set; }
        string QueueName { get; set; }
        string ContainerName { get; set; }
    }
    public class ConfigureServiceBus : IConfigureServiceBus
    {
        public string ConnectionStringServiceBus { get; set; }
        public string ConnectionStringBlobStorage { get; set; }
        public string QueueName { get; set; }
        public string ContainerName { get; set; }

    }
}
