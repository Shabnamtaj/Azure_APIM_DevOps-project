using Microsoft.Azure.Cosmos;
using FileModel.Models;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
    {
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync(FileMetadata file)
    {
        await _container.CreateItemAsync(file, new PartitionKey(file.Id.ToString()));
    }

    public async Task DeleteItemAsync(string id)
    {
        await _container.DeleteItemAsync<FileMetadata>(id, new PartitionKey(id));
    }
}
