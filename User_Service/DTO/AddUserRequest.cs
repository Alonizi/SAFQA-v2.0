using System.ComponentModel.DataAnnotations;

namespace User_Service.DTO{


    public class AddUserRequest{

        public string Fullname {get;set;}
        public string Phone {get;set;}
        [EmailAddress]
        public string Email {get;set;}
        
    }
}