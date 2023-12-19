using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Wallet_Service.Consumers{


    public class CheckUserFundsConsumer : IConsumer<CheckUserFunds>
    {
        private readonly ApplicationDbContext _appDb ; 
        private readonly ILogger<CheckUserFundsConsumer> _logger;
        public CheckUserFundsConsumer(ILogger<CheckUserFundsConsumer> logger ,ApplicationDbContext appDb ){

            _logger = logger ; 
            _appDb = appDb;
        }

        public async Task Consume(ConsumeContext<CheckUserFunds> context)
        {
            var investor =  await _appDb.investors
            .Where(i=>i.Id == context.Message.userId)
            .Include(i=>i.Wallet)
            .SingleOrDefaultAsync();
            await context.RespondAsync<UserFundsResult>(
                new UserFundsResult{
                     enoughFunds =  investor.Wallet.money > context.Message.amount,
                     userId = context.Message.userId
                });

            // deduct from investor balance 
            if(investor.Wallet.money>context.Message.amount) {
                var opportunityWallet = await _appDb.opportunitiesWallets.SingleOrDefaultAsync(ow=>ow.OpportunityId == context.Message.OppertunityId);
                opportunityWallet.money += context.Message.amount ; 
                investor.Wallet.money -= context.Message.amount;
                await _appDb.SaveChangesAsync();

                //send add transaction Command to transactions service 
                var sendendpoint =await  context.GetSendEndpoint(new System.Uri("queue:AddInvesmentTransaction")) ;
                await sendendpoint.Send<AddInvesmentTransaction>(
                    new AddInvesmentTransaction{
                        Amount = context.Message.amount ,
                        InvestorWalletId = investor.Wallet.Id,
                        OpportunityWalletId = opportunityWallet.Id
                    });
            }

        }
    }
}