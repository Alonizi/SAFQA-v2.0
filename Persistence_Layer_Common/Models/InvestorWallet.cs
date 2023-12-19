using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence_Layer_Common.Models{

    public class InvestorWallet { 
        public int Id {get;set;}
        public double money {get;set;}
        public int InvestorId {get;set;}
        public Investor Investor {get;set;}
        public List<InvesmentTransaction> OutgoingInvesmentTransactions {get;set;}
 
    }
}