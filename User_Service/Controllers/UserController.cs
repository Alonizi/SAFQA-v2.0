using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using Persistence_Layer_Common.Contracts;
using Persistence_Layer_Common.DB;
using Persistence_Layer_Common.Models;
using Persistence_Layer_Common.Repository;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using User_Service.DTO;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly ILogger<UserController> _logger;
        // private readonly ApplicationDbContext _appDb ; 
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IGenericRepository<Investor> _investorsRepo ; 

        public UserController(ILogger<UserController> logger , ApplicationDbContext appDb , IPublishEndpoint publishEndpoint , IGenericRepository<Investor> investorsRepo)
        {
            _logger = logger;
            // _appDb = appDb; 
            _publishEndPoint = publishEndpoint;
            _investorsRepo = investorsRepo;
        }

        [HttpGet]
        [Route("{userId}/")]
        public async Task<IActionResult> get(int userId){
            _logger.LogInformation("Fetching User Info");
            var user = await _investorsRepo.FindAsync(investor =>investor.Id == userId);
          // var user = await _appDb.investors.FindAsync(userId);
            if(user !=null){
            return Ok(
                new {
                    name = user.Fullname ,
                    email = user.Email ,
                    phone = user.Phone, 
                    });
            } 
            else return BadRequest("No user exist with this id");
        }

        [HttpPut]
        [Route("{userId}/")]
        public async Task<IActionResult> Update(int userId,[FromBody]AddUserRequest userInfo){
            
           var user = await _investorsRepo.FindAsync(i=>i.Id == userId);
           user.Email = userInfo.Email;
           user.Phone = userInfo.Phone;
           user.Fullname = userInfo.Fullname;
           
           await _investorsRepo.SaveChangesAync();
            
            return Ok("Investor Profile Updated Successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]AddUserRequest userInfo){
            
            var newInvestor = new Investor{ 
                 Email = userInfo.Email,
                 Phone = userInfo.Phone,
                 Fullname = userInfo.Fullname,
                } ;
            
            try {
            await _investorsRepo.AddAsync(newInvestor);
            // await _appDb.SaveChangesAsync();
            }
            catch(Exception ex){
                _logger.LogError ( ex.Message);
                return BadRequest("error adding new Investor");
            }

            // publish new user created event
            await _publishEndPoint.Publish<NewUserCreated>(new NewUserCreated{ userId = newInvestor.Id});
            
            return Ok("Investor Added Successfully");

        }
    }
}
