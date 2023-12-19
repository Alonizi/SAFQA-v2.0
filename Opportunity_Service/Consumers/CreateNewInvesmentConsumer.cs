using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Opportunity_Service.Consumers {


    class CreateNewInvesmentConsumer : IConsumer<CreateNewInvesment>
    {
        private readonly ApplicationDbContext _appDb;
        private readonly ILogger<CreateNewInvesmentConsumer> _logger;
        public CreateNewInvesmentConsumer (ApplicationDbContext appDb,ILogger<CreateNewInvesmentConsumer> logger ){
            _appDb = appDb;
            _logger = logger ;
        }

        // $$$$ NOT USED $$$$

        public async Task Consume(ConsumeContext<CreateNewInvesment> context)
        {
            _logger.LogInformation("Create New Invesment Command Being Consumed");

            // await _appDb.invesments.AddAsync(new Invesment{
            // Amount = context.Message.amount,
            // InvestorId = context.Message.InvestorId,
            // OppertunityId = context.Message.OpportunityId
            // });
            // await _appDb.SaveChangesAsync();

            _logger.LogInformation("Create New Invesment Command Consumed Successfully");

            //context.Publish<>
            // throw new System.NotImplementedException();
        }
    }



}