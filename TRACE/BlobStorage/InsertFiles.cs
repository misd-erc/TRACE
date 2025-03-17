using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TRACE.BlobStorage
{
    public class FileUploadService
    {
        private readonly string _connectionString = "";
        private readonly string _containerName = "tracecontainer";

        public FileUploadService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                string fileName = Path.GetFileName(file.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                using (Stream fileStream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                return fileName; // Return uploaded file name or URL
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error uploading file: {ex.Message}");
                return null;
            }
        }

        public async Task CreateFolders(string folderName)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
                // Ensure the folder name ends with a "/"
                folderName = folderName.TrimEnd('/') + "/";

                // Create an empty blob to simulate a folder
                BlobClient blobClient = containerClient.GetBlobClient(folderName);

                using var emptyStream = new MemoryStream(new byte[0]); // Empty file
                await blobClient.UploadAsync(emptyStream, overwrite: true);

                Console.WriteLine($"📁 Folder '{folderName}' created successfully in Blob Storage.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error creating folder '{folderName}': {ex.Message}");
            }
        }


    }
}
