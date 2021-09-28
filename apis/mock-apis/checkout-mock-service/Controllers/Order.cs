using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace checkout_service_mock.Controllers
{
    public record PlaceOrderDTO(string ProductId, int Quantity);

    public record ProcessPaymentDTO(string OrderId, float TotalPrice);

    public record PaymentResponse(string OrderId, string Description, string Status);

    [ApiController]
    [Route("[controller]")]
    public class Order : ControllerBase
    {        
        private readonly ILogger<Order> _logger;

        public Order(ILogger<Order> logger)
        {
            _logger = logger;
        }

        [HttpPost("place-order")]
        public ProcessPaymentDTO PlaceOrder([FromBody] PlaceOrderDTO order)
        {
            var orderId = order.ProductId is "500"? "500" : Guid.NewGuid().ToString();
            var totalPrice = order.Quantity * (new Random()).Next(50, 1000);

            _logger.LogWarning($"Placing order {orderId} with {order.Quantity} items of product {order.ProductId}");

            return new ProcessPaymentDTO(orderId, totalPrice);
        }

        [HttpPut("payment-rejected")]
        public void PaymentRejected([FromBody] PaymentResponse paymentRejection)
        {
            _logger.LogWarning($"The payment for the order {paymentRejection.OrderId} was rejected: {paymentRejection.Description}.");
        }
    }
}
