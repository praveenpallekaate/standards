using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace Core.BlobStorage
{
    /// <summary>
    /// Blob storage management
    /// </summary>
    public class Storage
    {
        private readonly string _connectionString;

        public Storage(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Uploads file to container
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        /// <param name="container"></param>
        /// <returns>Uploaded file URI</returns>
        public async Task<string> UploadFileAsBlobAsync(Stream stream, string filename, string container)
        {
            BlobContainerClient containerClient = null;

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            containerClient = blobServiceClient.GetBlobContainerClient(container);

            if (!containerClient.Exists())
            {
                // Create the container and return a container client object
                containerClient = await blobServiceClient.CreateBlobContainerAsync(container);
            }

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(filename);

            // Upload file
            await blobClient.UploadAsync(stream, true);

            stream.Dispose();

            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Download's file from container
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="container"></param>
        /// <returns>Blob file stream</returns>
        public async Task<Stream> DownloadFileAsStreamAsync(string filename, string container)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(container);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(filename);

            // Open blob stream to read
            return await blobClient.OpenReadAsync();
        }
    }
}
