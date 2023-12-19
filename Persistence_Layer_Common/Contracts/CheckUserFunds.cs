namespace Persistence_Layer_Common.Contracts{

    public class CheckUserFunds { 

        public int userId {get;set;}
        public double amount {get;set;}
        public int OppertunityId {get;set;}

    }

    public class UserFundsResult { 
        public int userId {get;set;}
        public bool enoughFunds {get;set;}

    }
}