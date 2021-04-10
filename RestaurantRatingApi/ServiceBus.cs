using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using RestaurantRatingApi;

namespace RestaurantRatingApi
{

    public class ServiceBusSender
    {

        private string _connectionStringServiceBus;
        private string _connectionStringBlobStorage;
        private string _queueName;
        private string _containerName;

        private Microsoft.Azure.ServiceBus.QueueClient _queueClient;
        private BlobContainerClient _container;
        public ServiceBusSender(IConfigureServiceBus configure)
        {

            _connectionStringServiceBus = configure.ConnectionStringServiceBus;
            _connectionStringBlobStorage = configure.ConnectionStringBlobStorage;
            _queueName = configure.QueueName;
            _containerName = configure.ContainerName;

            _queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionStringServiceBus, _queueName);
            _container = new BlobContainerClient(_connectionStringBlobStorage, _containerName);
            _container.CreateIfNotExists();

        }

        public async Task UploadFile(string filePath, string fileName)
        {

            _container.DeleteBlobIfExists(fileName);
            BlobClient blob = _container.GetBlobClient(fileName);

            await blob.UploadAsync(filePath);
        }

        public async Task SendMessage(string blobName)
        {
            var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(blobName));

            await _queueClient.SendAsync(message);

        }
    }
}

