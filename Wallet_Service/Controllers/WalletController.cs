using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet_Service.DTO;

namespace Wallet_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly ApplicationDbContext _appDb ;
        private readonly ISendEndpointProvider _bus ; 

        public WalletController(ILogger<WalletController> logger,ApplicationDbContext appDb , ISendEndpointProvider bus)
        {
            _logger = logger;
            _appDb = appDb;
            _bus = bus;
        }

        [HttpPost]
        [Route("/applepaytopup")]
        public async Task<IActionResult> TopUpWallet([FromBody] TopUpWalletRequest request){
            var investor =  await _appDb.investors.Where(i=>i.Id == request.investorId).Include(i=>i.Wallet).SingleOrDefaultAsync();

            investor.Wallet.money += request.amount ; 
            await _appDb.SaveChangesAsync();
            return Ok($"{request.amount} SAR added to {investor.Fullname} wallet \n Total = {investor.Wallet.money}");
        }

        // [HttpPost]
        // [Route("/invest")]
        // public async Task<IActionResult> Invest ([FromBody] InvestDTO request ){
        //     // add logic to deduct amount from user wallet
        //     var investor =  await _appDb.investors.FindAsync(request.investorId);

        //     if(investor == null) return BadRequest("Investor Does not Exist");

        //     investor..Money -= request.amount ; 

            
        //     // add logic to send command to Oppourtunity Service to add the invesment
        //     var endpoint = await _bus.GetSendEndpoint(new Uri("queue:CreateNewInvesment"));
        //     await endpoint.Send(new CreateNewInvesment{
        //         amount = request.amount,
        //         InvestorId = request.investorId,
        //         OpportunityId = request.opportunityId
        //     });
            

        //     return Ok("sent to Opportunity service");

        // }
    }
}
