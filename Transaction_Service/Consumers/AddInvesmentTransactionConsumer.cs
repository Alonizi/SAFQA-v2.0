using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Persistence_Layer_Common.Repository;

namespace Transaction_Service.Consumers {


    public class AddInvesmentTransactionConsumer : IConsumer<AddInvesmentTransaction>
    {
        ILogger<AddInvesmentTransactionConsumer> _logger;
        IGenericRepository<InvesmentTransaction> _transactionsRepo;

        public AddInvesmentTransactionConsumer(ILogger<AddInvesmentTransactionConsumer> logger , IGenericRepository<InvesmentTransaction> transactionsRepo){
            _logger = logger ; 
            _transactionsRepo = transactionsRepo ; 
        }

        public async Task Consume(ConsumeContext<AddInvesmentTransaction> context)
        {
            _logger.LogInformation("Invesment Transaction Received By Transaction Service /n Tranaction Creation in progress..");

            await _transactionsRepo.AddAsync(
                new InvesmentTransaction{
                    Amount = context.Message.Amount,
                    OpportunityWalletId = context.Message.OpportunityWalletId,
                    InvestorWalletId = context.Message.InvestorWalletId 
                    });
        }
    }
}