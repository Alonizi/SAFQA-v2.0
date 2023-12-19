namespace Persistence_Layer_Common.Contracts{
    public class AddInvesmentTransaction{

        public int OpportunityWalletId {get;set;}
        public int InvestorWalletId {get;set;}
        public double Amount {get;set;}
    }
}