using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace notification_service_mock
{
    public static class NotificationFunction
    {
        [FunctionName("NotificationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {                        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<PaymentResponse>(requestBody);

            log.LogInformation($"Notification saved successfully: {response.OrderId} | {response.Description}");

            return new OkObjectResult("SUCCESS");
        }
    }

    public class PaymentResponse
    {
        public string OrderId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
