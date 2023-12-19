using System.Collections.Generic;
using System.ComponentModel;

namespace Persistence_Layer_Common.Models {


    public class Opportunity {
        public int Id {get;set;} 
        public string Name {get;set;}
        
        [Description("Opportunity Investors")]
        public  List<Investor> Investors {get;set;}

        public OpportunityWallet Wallet {get;set;}        
    }
}