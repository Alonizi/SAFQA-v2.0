using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
namespace SMS_Service.Consumers{
    public class OpportunityCreatedConsumer : IConsumer<NewOpportunityCreated>
    {
        private readonly ILogger<OpportunityCreatedConsumer> _logger;
        public OpportunityCreatedConsumer(ILogger<OpportunityCreatedConsumer> logger){
            _logger = logger ; 
        }

        public async Task Consume(ConsumeContext<NewOpportunityCreated> context)
        {

            _logger.LogInformation($"New Opportunity Created Event Received At SMS Service /n Opportunity Id {context.Message.OppertunityId}");
        }
    }


}