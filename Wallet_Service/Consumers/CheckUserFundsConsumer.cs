using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using Persistence_Layer_Common.Repository;

namespace Wallet_Service.Consumers{


    public class CheckUserFundsConsumer : IConsumer<CheckUserFunds>
    {
        private readonly ILogger<CheckUserFundsConsumer> _logger;
        private readonly IGenericRepository<InvestorWallet> _investorsWalletsRepo;
        private readonly IGenericRepository<OpportunityWallet> _opportunitiesWalletsRepo;

        public CheckUserFundsConsumer(
            ILogger<CheckUserFundsConsumer> logger ,
            IGenericRepository<InvestorWallet> investorsWalletsRepo,
            IGenericRepository<OpportunityWallet> OpportunitiesWalletsRepo
            ){

            _logger = logger ; 
            _investorsWalletsRepo = investorsWalletsRepo ; 
            _opportunitiesWalletsRepo = OpportunitiesWalletsRepo;
        }

        public async Task Consume(ConsumeContext<CheckUserFunds> context)
        {
           // Thread.Sleep(10*1000);

            var investorWallet = await _investorsWalletsRepo.FindAsync(filter: iw=>iw.InvestorId ==context.Message.userId );

            await context.RespondAsync<UserFundsResult>(
                new UserFundsResult{
                     enoughFunds =  investorWallet.money > context.Message.amount,
                     userId = context.Message.userId
                });


            // deduct from investor Wallet , Add funds to Opportunity Wallet
            if(investorWallet.money>context.Message.amount) {

                var opportunityWallet = await _opportunitiesWalletsRepo.FindAsync(ow=>ow.OpportunityId == context.Message.OppertunityId);
                opportunityWallet.money += context.Message.amount ; 
                investorWallet.money -= context.Message.amount;
                await _opportunitiesWalletsRepo.SaveChangesAync();

                //send add transaction Command to transactions service 
                var sendendpoint =await  context.GetSendEndpoint(new System.Uri("queue:AddInvesmentTransaction")) ;
                await sendendpoint.Send<AddInvesmentTransaction>(
                    new AddInvesmentTransaction{
                        Amount = context.Message.amount ,
                        InvestorWalletId = investorWallet.Id,
                        OpportunityWalletId = opportunityWallet.Id
                    });
            }
        }
    }
}