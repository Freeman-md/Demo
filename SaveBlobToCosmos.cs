using System.IO;
using System.Threading.Tasks;
using Demo.Models;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class CosmoDBResponse
    {
        [CosmosDBOutput("DemoDB", "Demo",
                Connection = "CosmosDbConnectionString", PartitionKey = "demo", CreateIfNotExists = true)]
        public required MyDocument Document { get; set; }
        public HttpResponseData? HttpResponse { get; set; }
    }
    public class MyDocument
    {
        public required string id { get; set; }
        public required string demo { get; set; }
        
        public required string Name { get; set; }
        public required int Age { get; set; }
    }

    public class SaveBlobToCosmos
    {
        private readonly ILogger<SaveBlobToCosmos> _logger;

        public SaveBlobToCosmos(ILogger<SaveBlobToCosmos> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SaveBlobToCosmos))]
        public async Task <CosmoDBResponse>Run([BlobTrigger("output-container/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name, FunctionContext executionContext)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();

            Person person = JsonSerializer.Deserialize<Person>(content)!;

            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {person}");

            if (person == null) {
                throw new ArgumentNullException(nameof(person));
            }

            return new CosmoDBResponse {
                Document = new MyDocument {
                    id = Guid.NewGuid().ToString(),
                    demo = "Demo1",
                    Name = person.name,
                    Age = person.age
                }
            };
        }
    }
}
