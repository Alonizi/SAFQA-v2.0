using System;

namespace Persistence_Layer_Common.Models{




    public class InvesmentTransaction {

        public int Id {get;set;}
        public int InvestorWalletId {get;set;}
        public int OpportunityWalletId {get;set;}
        public InvestorWallet InvestorWallet {get;set;} 
        public OpportunityWallet OpportunityWallet {get;set;} 
        public double Amount {get;set;} 
        public DateTime CreatedAt {get;set;} 

    }
}