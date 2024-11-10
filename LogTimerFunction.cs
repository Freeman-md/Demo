using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class BlobResponse {
        [BlobOutput("output-container/output-file.txt", Connection = "AzureWebJobsStorage")]
        public string BlobContent { get; set; }

        public HttpResponseData HttpResponseData { get; set; }
    }
    public class LogTimerFunction
    {
        private readonly ILogger _logger;

        public LogTimerFunction(ILogger<LogTimerFunction> logger)
        {
            _logger = logger;
        }

        [Function("LogTimeFunction")]
        public BlobResponse Run(
        [TimerTrigger("*/5 * * * * *")] TimerInfo myTimer,
        FunctionContext executionContext)
        {
             var response = new BlobResponse
            {
                BlobContent = "This is output to a blob!",
            };

            _logger.LogInformation("Data written to output blob");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
            else
            {
                _logger.LogInformation("Schedule status information is unavailable.");
            }

            return response;
        }
    }
}
