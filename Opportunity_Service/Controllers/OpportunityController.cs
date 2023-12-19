using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using MassTransit;
using MassTransit.Initializers;
using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opportunity_Service.DTO;
using Persistence_Layer_Common.Repository;

namespace Opportunity_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpportunityController : ControllerBase
    {

        private readonly ILogger<OpportunityController> _logger;
        private readonly ApplicationDbContext _appDb;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<CheckUserFunds> _requestClient;
        private readonly IGenericRepository<Opportunity> _opportunitiesRepo;
        private readonly IGenericRepository<Invesment> _invesmentsRepo;
        public OpportunityController(
            ILogger<OpportunityController> logger ,
            ApplicationDbContext appDb , 
            IPublishEndpoint publishEndpoint, 
            IRequestClient<CheckUserFunds> requestClient , 
            IGenericRepository<Opportunity> opportunitiesRepo,
            IGenericRepository<Invesment> invesmentsRepo
            )
        {
            _logger = logger;
            _appDb = appDb;
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient; 
            _opportunitiesRepo = opportunitiesRepo;
            _invesmentsRepo = invesmentsRepo;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOpportunity ([FromBody]CreateOpportunityDTO request){
            
            Opportunity opportunity = new Opportunity{Name = request.name};
            await _opportunitiesRepo.AddAsync(opportunity);
            // await _appDb.SaveChangesAsync();
            
            //Publish event newely created Opportunity 
            await _publishEndpoint.Publish<NewOpportunityCreated>(
                new NewOpportunityCreated{
                    OppertunityId =opportunity.Id 
                });
            
            return Ok("Opportunity Created");
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get (){
            var allOpportunities =  await _appDb.opportunities
                .Include(i=>i.Investors)
                .Include(i=>i.Wallet)
                .ToListAsync();

            var response = allOpportunities.Select(r=>new {
                Opportunityname = r.Name ,
                totalInvesment = r.Wallet.money ,
                investors = r.Investors.Select(i=>i.Fullname)
                });
            return response;

        }
        
        [HttpPost]
        [Route("{opportunityId}/invest")]
        public async Task<IActionResult> CreateInvesment(int opportunityId ,[FromBody] CreateInvesmentDTO request){
            string responseMsg;

            //check from wallet service if User has enough balance , manage deduction/transactions 
            var userFundsStatus = await _requestClient.GetResponse<UserFundsResult>(
                new CheckUserFunds{
                     amount = request.amount,
                     userId = request.investorId,
                     OppertunityId = opportunityId
                 });
            
            // add/update  invesment if user has balance 
            if(userFundsStatus.Message.enoughFunds){
                
                // Invesment invesment = await _appDb.invesments.SingleOrDefaultAsync(i=>i.InvestorId==request.investorId && i.OppertunityId==opportunityId);
                Invesment invesment = await _invesmentsRepo.FindAsync(i=>i.InvestorId==request.investorId && i.OppertunityId==opportunityId);
               
                //add additional funds to already exisiting invesment
                if(invesment != null) {
                    invesment.Amount += request.amount; 
                    await _invesmentsRepo.SaveChangesAync();
                    responseMsg = $"Added Additional Funds to your Invesment\nTotal invested = {invesment.Amount}";
                }
                // create new invesment if it wasnt created before
                else {
                    invesment = new Invesment{
                        InvestorId = request.investorId ,
                        Amount = request.amount ,
                        OppertunityId = opportunityId  
                    };
                    await _invesmentsRepo.AddAsync(invesment);
                    // await _appDb.SaveChangesAsync();
                    
                    responseMsg = "New Invesment Created" ;
                }
                // publish invesment event
                await _publishEndpoint.Publish<NewInvesmentAdded>(
                new NewInvesmentAdded{ 
                    amount = request.amount ,
                    InvesmentId = invesment.Id , 
                    InvestorId = request.investorId ,
                    OppertunityId = opportunityId
                });
                
                return Ok(responseMsg);
            }
            else return BadRequest("User doesn't have enough funds"); 
        }


#region helperFunctions
#endregion helperFunctions
    }
}
