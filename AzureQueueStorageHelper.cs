using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureFunctionDriver
{
    /// <summary>
    /// Azure queue storage helper class
    /// </summary>
    public class AzureQueueStorageHelper
    {
        /// <summary>
        /// Cloud storage account
        /// </summary>
        private CloudStorageAccount storageAccount { get; set; }

        /// <summary>
        /// Gets a new cloud queue client
        /// </summary>
        /// <returns>cloud queue client</returns>
        private CloudQueueClient QueueClient => storageAccount.CreateCloudQueueClient();

        public AzureQueueStorageHelper(string AzureQueueConnectionString)
        {
            storageAccount = CloudStorageAccount.Parse(AzureQueueConnectionString);
        }

        /// <summary>
        /// Adds a message to the message queue
        /// </summary>
        public void AddMessageToQueue(string queueReference, string content)
        {
            CloudQueue queue = QueueClient.GetQueueReference(queueReference);
            var message = new CloudQueueMessage(content);
            queue.AddMessageAsync(message);
        }
        
        /// <summary>
        /// Gets a new cloud queue client
        /// </summary>
        /// <returns>returns messages</returns>
        public IEnumerable<CloudQueueMessage> GetMessagesFromQueue(string queueReference, int take = 100)
        {
            CloudQueue queue = QueueClient.GetQueueReference(queueReference);
            return queue.GetMessagesAsync(take).Result;
        }


    }
}