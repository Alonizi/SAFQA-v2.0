using System.Collections.Generic;

namespace Persistence_Layer_Common.Models{

    public class OpportunityWallet { 
        public int Id {get;set;}
        public double money {get;set;}
        public int OpportunityId {get;set;}
        public Opportunity Opportunity {get;set;}
        public List<InvesmentTransaction> IncomingInvesmentTransactions {get;set;}

    }
}