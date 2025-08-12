namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobStorageAsync(string fileName, Stream data);
    string? GetBlobSasUrl(string? blobUrl);
}
