using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Azure_Storage.TableStorage
{
    public class TableStorageRepository<T> where T : TableEntity, new()
    {
        private readonly CloudTable _cloudTable;

        public TableStorageRepository(string storageConnectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = cloudTableClient.GetTableReference(tableName);
            _cloudTable.CreateIfNotExists();
        }

        public async Task AddOrUpdateAsync(T tableEntity, CancellationToken cancellationToken = default)
        {
            await _cloudTable.ExecuteAsync(TableOperation.InsertOrMerge(tableEntity), cancellationToken);
        }

        public async Task<T> RetrieveAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _cloudTable.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey), cancellationToken);

                return result.Result as T;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 404)
                {
                    return null;
                }
                throw;
            }
        }
    }
}
