using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
namespace SMS_Service.Consumers{
    public class InvesmentCreatedConsumer : IConsumer<NewInvesmentAdded>
    {
        private readonly ILogger<InvesmentCreatedConsumer> _logger;
        public InvesmentCreatedConsumer(ILogger<InvesmentCreatedConsumer> logger){
            _logger = logger ; 
        }
        public async Task Consume(ConsumeContext<NewInvesmentAdded> context)
        {
            // send SMS message to user notifying him his invesment was created 
            _logger.LogInformation($"new invesment created with  {context.Message.InvesmentId} Created Event Received BY SMS Service");
        }
    }


}