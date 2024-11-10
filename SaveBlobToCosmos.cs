using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class SaveBlobToCosmos
    {
        private readonly ILogger<SaveBlobToCosmos> _logger;

        public SaveBlobToCosmos(ILogger<SaveBlobToCosmos> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SaveBlobToCosmos))]
        public async Task Run([BlobTrigger("output-container/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
