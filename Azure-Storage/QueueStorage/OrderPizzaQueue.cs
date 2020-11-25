using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.Storage;
using Newtonsoft.Json;

namespace Azure_Storage.QueueStorage
{
    public class OrderPizzaQueue
    {
        public const string QueueName = "order-pizza-queue";
        private readonly CloudQueue _queue;

        public OrderPizzaQueue(string storageConnectionString)
        {
            var cloudQueueClient = CloudStorageAccount.Parse(storageConnectionString).CreateCloudQueueClient();
            _queue = cloudQueueClient.GetQueueReference(QueueName);
            _queue.CreateIfNotExists();
        }

        public async Task AddMessageAsync(OrderPizzaMessage orderPizzaMessage)
        {
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(orderPizzaMessage));
            await _queue.AddMessageAsync(message, null, TimeSpan.FromSeconds(10), null, null);
        }

        public async Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan visibilityTimeout, CancellationToken cancellationToken = default)
        {
            var messages = await _queue.GetMessagesAsync(messageCount, visibilityTimeout, new QueueRequestOptions(), new OperationContext(), cancellationToken);

            return messages;
        }

        public async Task DeleteMessageAsync(CloudQueueMessage message, CancellationToken cancellationToken = default)
        {
            await _queue.DeleteMessageAsync(message, cancellationToken);
        }
    }
}
