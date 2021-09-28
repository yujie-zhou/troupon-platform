using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace payment_service_mock
{
    public static class PaymentFunction
    {        
        [FunctionName("PaymentFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Payment HTTP trigger function processed a request.");            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var payment = JsonConvert.DeserializeObject<ProcessPaymentDTO>(requestBody);

            if (payment.OrderId is "500")
            {
                var rejectionResponse = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

                var rejectionPaymentResponse = new PaymentResponse
                {
                    OrderId = payment.OrderId,
                    Description = "Card expired",
                    Status = "PAYMENT_REJECTED"
                };

                rejectionResponse.Content = new StringContent(JsonConvert.SerializeObject(rejectionPaymentResponse));
                return rejectionResponse;
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var successfullResponse = new PaymentResponse
            {
                OrderId = payment.OrderId,
                Description = "Successfull payment!",
                Status = "PAYMENT_SUCCESSFULL"
            };
                
            response.Content = new StringContent(JsonConvert.SerializeObject(successfullResponse));

            return response;            
        }

        public class ProcessPaymentDTO
        {
            public string OrderId { get; set; }
            public float TotalPrice { get; set; }
        }

        public class PaymentResponse
        {
            public string OrderId { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
        }
    }
}
