using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

public static class HttpRequestDataExtensions
{
    public static async Task<HttpResponseData> CreateStringResponseAsync(this HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        var response = req.CreateResponse(statusCode);
        await response.WriteStringAsync(message);
        return response;
    }
}
