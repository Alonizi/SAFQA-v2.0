using System.Collections.Generic;
using System.ComponentModel;

namespace Persistence_Layer_Common.Models{



    public class Investor : User {
        public InvestorWallet Wallet {get;set;}
         
        [Description("Investor's Invesments")]
        public  List<Opportunity> opportunities {get;set;}

    }
}