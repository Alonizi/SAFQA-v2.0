using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Persistence_Layer_Common.Repository;

namespace Wallet_Service.Consumers{



    public class NewUserCreatedConsumer : IConsumer<NewUserCreated>
    {
        private readonly ILogger<NewUserCreatedConsumer> _logger;
        private readonly IGenericRepository<InvestorWallet> _investorsWalletsRepo;
        
        public NewUserCreatedConsumer (ILogger<NewUserCreatedConsumer> logger, IGenericRepository<InvestorWallet> investorsWalletsRepo){
            _logger = logger ; 

            _investorsWalletsRepo = investorsWalletsRepo;
        }
        public async Task Consume(ConsumeContext<NewUserCreated> context)
        {
            _logger.LogInformation($"User Register Event Received by Wallet Service\n Creating new wallet for the Investor Id {context.Message.userId}");

            //  create new wallet for the user 
            await _investorsWalletsRepo.AddAsync(new InvestorWallet{money = 500, InvestorId = context.Message.userId});

        }   
    }
}