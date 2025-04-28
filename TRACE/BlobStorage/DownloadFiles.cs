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
        private static readonly string containerName = "tracecontainer";

        public async Task<BlobDownloadInfo> DownloadBlobAsync(string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                return await blobClient.DownloadAsync();
            }
            else
            {
                return null;
            }
        }


    }
}