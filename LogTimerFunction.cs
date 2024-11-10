using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class LogTimerFunction
    {
        private readonly ILogger _logger;

        public LogTimerFunction(ILogger<LogTimerFunction> logger)
        {
            _logger = logger;
        }

        [Function("LogTimerFunction")]
        public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
            else
            {
                _logger.LogInformation("Schedule status information is unavailable.");
            }
        }
    }
}
