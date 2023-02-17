using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.eShopWeb.ApplicationCore.DTOs;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using OrderAggregate = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class BlobStorage : IStorage
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly BlobContainerClient _containerClient;

        public BlobStorage(IConfiguration configuration)
        {
            var storageConfig = new AzureStorageConfig()
            {
                ConnectionString = configuration["BlobStorageConnectionString"],
                FileContainerName = configuration["BlobStorageName"]
            };

            _containerClient = new BlobServiceClient(storageConfig.ConnectionString)
                .GetBlobContainerClient(storageConfig.FileContainerName);
        }

        public async Task ReserveOrderAsync(string jsonString)
        {
            var order = JsonSerializer.Deserialize<Order>(jsonString, _serializerOptions);
            BlobClient blobClient = _containerClient.GetBlobClient(GetBlobNameFrom(order));

            var stream = await ToStream(jsonString);

            // Upload the blob
            await blobClient.UploadAsync(stream);
        }

        public async Task<IEnumerable<string>> GetOrderNamesAsync()
        {
            var blobs = _containerClient.GetBlobsAsync();

            var blobNames = Enumerable.Empty<string>();
            
            await foreach (var blob in blobs)
            {
                blobNames.Append(blob.Name);
            }

            return blobNames;
        }


        public async Task<IEnumerable<OrderAggregate>> GetOrdersAsync()
        {
            var blobs = _containerClient.GetBlobsAsync();

            var blobValues = new List<Task<string>>();

            await foreach (var blob in blobs)
            {
                blobValues.Add(GetBlobValueAsync(blob.Name));
            }

            Task.WaitAll(blobValues.ToArray());

            return blobValues
                .Select(x => JsonSerializer.Deserialize<Order>(x.Result).ToEntity())
                .ToArray();
        }

        private Task<string> GetBlobValueAsync(string blobName)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"started downloading blob {blobName}");
                var result = Encoding.ASCII.GetString(_containerClient.GetBlobClient(blobName).DownloadContentAsync().Result.Value
                    .Content);
                Console.WriteLine($"finished blob {blobName}");
                return result;
            });
        }

        private string GetBlobNameFrom(Order order) =>
            $"{order.OrderDate.DateTime:O}_{order.BuyerId}_{order.Id}.json";

        private Task<Stream> ToStream(string jsonString) =>
            Task.Run(() => new MemoryStream(Encoding.ASCII.GetBytes(jsonString)) as Stream);
    }
}
