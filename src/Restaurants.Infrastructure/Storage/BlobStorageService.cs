using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> options) : IBlobStorageService
{
    private readonly BlobStorageSettings _settings = options.Value;

    public async Task<string> UploadToBlobStorageAsync(string fileName, Stream data)
    {
        var client = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = client.GetBlobContainerClient(_settings.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);
        var blobUri = blobClient.Uri.ToString();
        return blobUri;
    }
}
