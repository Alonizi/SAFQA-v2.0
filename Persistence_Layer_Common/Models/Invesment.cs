using System.ComponentModel;

namespace Persistence_Layer_Common.Models{


    public class Invesment {
        public int Id {get;set;}
        public int InvestorId {get;set;}
        public int OppertunityId {get;set;}
        
        [Description("Total Amount the user invested in this Opportunity")]
        public double Amount {get;set;}
    }
}