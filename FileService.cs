using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using YourNamespace.Models;

public class FileService : IFileService
{
    private readonly FileDbContext _context;
    private readonly string _storageConnectionString;
    private readonly _storageAccount = "demotestapi";
    private readonly _cosmosdbName = "fileupload";
    private readonly string _key = "AzurekeyVault";

    public FileService(FileDbContext context, IConfiguration config)
    {
        _context = context;
        _storageConnectionString = config.GetConnectionString("AzureStorageAccount");
    }

    public IEnumerable<FileMetadata> GetFiles()
    {
        return _context.FileMetadata.ToList();
    }

    public async Task UploadFile(IFormFile file)
    {
        var fileName = Path.GetFileName(file.FileName);
        var blobClient = GetBlobClient();

        CloudBlockBlob cloudBlockBlob = blobClient.GetBlockBlobReference(fileName);
        await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

        var fileMetadata = new FileMetadata
        {
            FileName = fileName,
            FilePath = cloudBlockBlob.Uri.ToString(),
            UploadedDate = DateTime.Now
        };

        _context.FileMetadata.Add(fileMetadata);
        await _context.SaveChangesAsync();
    }

    public void DeleteFile(int id)
    {
        var file = _context.FileMetadata.Find(id);
        if (file == null)
            return;

        var blobClient = GetBlobClient();
        var blob = blobClient.GetBlobReference(Path.GetFileName(file.FilePath));
        blob.DeleteIfExists();

        _context.FileMetadata.Remove(file);
        _context.SaveChanges();
    }

    private CloudBlobContainer GetBlobClient()
    {
        var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference("files");
        container.CreateIfNotExists();
        return container;
    }
}
