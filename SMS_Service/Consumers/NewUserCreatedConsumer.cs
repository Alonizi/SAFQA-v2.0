using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SMS_Service.Consumers{

    public class NewUserCreatedConsumer : IConsumer<NewUserCreated>
    {
        private readonly ILogger<NewUserCreatedConsumer> _logger;
        public NewUserCreatedConsumer(ILogger<NewUserCreatedConsumer> logger){
            _logger = logger ; 
        }
        public async Task Consume(ConsumeContext<NewUserCreated> context)
        {
            // send Email message to user notifying him his account was created 
            _logger.LogInformation($"New User Registered event Received BY SMS Service , User Id {context.Message.userId}");
        }
    }
}