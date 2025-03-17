using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TRACE.BlobStorage
{
    public class DownloadFiles
    {
        
        private static readonly string connectionString = "";
        private static readonly string containerName = "container-staging";

        public static async Task<Stream> DownloadFileFromBlobStorage(string blobUrl)
        {
            try
            {
                Uri uri = new Uri(blobUrl);
                string blobName = Path.GetFileName(uri.LocalPath); // Extract file name from URL

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                BlobDownloadInfo download = await blobClient.DownloadAsync();
                MemoryStream memoryStream = new MemoryStream();
                await download.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset stream position

                return memoryStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return null;
            }
        }

    }
}