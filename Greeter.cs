using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Demo
{
    public class Greeter
    {
        private readonly ILogger<Greeter> _logger;

        public Greeter(ILogger<Greeter> logger)
        {
            _logger = logger;
        }

        [Function("Greeter")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing greeting request.");

            string? name = req.Query["name"];
            int currentHour = DateTime.Now.Hour;

            if (string.IsNullOrEmpty(name))
            {
                return await req.CreateStringResponseAsync(HttpStatusCode.BadRequest, "Please pass a name parameter");
            }

            string message = currentHour switch
            {
                >= 5 and < 12 => $"Good morning, {name}!",
                >= 12 and < 17 => $"Good afternoon, {name}!",
                >= 17 and < 22 => $"Good evening, {name}!",
                _ => $"Hello, {name}! Have a great night!"
            };



            return await req.CreateStringResponseAsync(HttpStatusCode.OK, message);
        }
    }
}
