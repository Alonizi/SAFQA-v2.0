using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Persistence_Layer_Common.Repository;

namespace Wallet_Service.Consumers{

    public class NewOpportunityCreatedConsumer : IConsumer<NewOpportunityCreated>
    {
        private readonly ILogger<NewOpportunityCreated> _logger;
        private readonly IGenericRepository<OpportunityWallet> _opportunitiesWalletsRepo;

        public NewOpportunityCreatedConsumer (ILogger<NewOpportunityCreated> logger, IGenericRepository<OpportunityWallet> opportunitiesWalletsRepo){
            _logger = logger ; 
            _opportunitiesWalletsRepo = opportunitiesWalletsRepo;
        }
        
        public async Task Consume(ConsumeContext<NewOpportunityCreated> context)
        {
            _logger.LogInformation($"Opportunity Created Event Received by Wallet Service\n Creating new wallet for the Opportunity Id {context.Message.OppertunityId}");

            //  create new wallet for the user 
            await _opportunitiesWalletsRepo.AddAsync(new OpportunityWallet{money = 500, OpportunityId = context.Message.OppertunityId});
        }
    }
}