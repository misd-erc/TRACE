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

        public async Task CreateFolders(string folderName, int numberOfSubFolders)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                // Ensure the main folder name ends with a "/"
                folderName = folderName.TrimEnd('/') + "/";

                // Upload dummy file to simulate the main folder
                await UploadEmptyBlob(containerClient, folderName);

                Console.WriteLine($"📁 Main folder '{folderName}' created successfully in Blob Storage.");

                for (int i = 1; i <= numberOfSubFolders; i++)
                {
                    string subFolderPath = $"{folderName}{i}/";
                    await UploadEmptyBlob(containerClient, subFolderPath);
                    Console.WriteLine($"📂 Subfolder '{subFolderPath}' created successfully.");

                    // Add actual files inside each subfolder
                    for (int j = 1; j <= 1; j++)
                    {
                        string filePath = $"{subFolderPath}SampleFile{j}.txt";
                        await UploadTextFile(containerClient, filePath, $"This is content for {filePath}");
                        Console.WriteLine($"📄 File '{filePath}' uploaded successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
        private async Task UploadTextFile(BlobContainerClient containerClient, string filePath, string content)
        {
            BlobClient blobClient = containerClient.GetBlobClient(filePath);
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        private async Task UploadEmptyBlob(BlobContainerClient containerClient, string blobName)
        {
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using var emptyStream = new MemoryStream(new byte[0]);
            await blobClient.UploadAsync(emptyStream, overwrite: true);
        }
    }



    
}
