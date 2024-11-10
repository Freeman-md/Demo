using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public record Person(string name, int age);
    public class BlobResponse
    {
        [BlobOutput("output-container/person.txt", Connection = "AzureWebJobsStorage")]
        public Person BlobContent { get; set; }

        public HttpResponseData HttpResponseData { get; set; }
    }

    public class LogTimerFunction
    {
        private readonly ILogger _logger;

        public LogTimerFunction(ILogger<LogTimerFunction> logger)
        {
            _logger = logger;
        }

        [Function("LogTimerFunction")]
        public BlobResponse Run(
        [TimerTrigger("*/5 * * * * *")] TimerInfo myTimer,
        FunctionContext executionContext)
        {
            var response = new BlobResponse
            {
                BlobContent = new Person("John", 18),
            };

            _logger.LogInformation("Data written to output blob");

            return response;
        }
    }
}
