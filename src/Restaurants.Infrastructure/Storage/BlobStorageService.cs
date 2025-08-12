using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (string.IsNullOrEmpty(blobUrl)) return null;
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _settings.ContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl),
        };
        sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var sharedKeyCredential = new StorageSharedKeyCredential(
            blobServiceClient.AccountName,
            _settings.AccountKey);
        var sasToken = sasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
        return $"{blobUrl}?{sasToken}";
    }

    private static string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}
