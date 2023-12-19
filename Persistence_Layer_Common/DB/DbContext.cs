using Persistence_Layer_Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence_Layer_Common.DB {


    public class ApplicationDbContext :DbContext {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){
    }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                       
            //  modelBuilder.Entity<Opportunity>()
            //     .HasMany(e=>e.Investors)
            //     .WithMany(e=>e.opportunities)
            //     .UsingEntity<Invesment>(
            //         invesment=> invesment
            //         .HasOne(invesment=>invesment.Opportunity)
            //         .WithMany().HasForeignKey(""))


            modelBuilder.Entity<Opportunity>()
                .HasMany(e => e.Investors)
                .WithMany(e => e.opportunities)
                .UsingEntity<Invesment>(
                    l => l.HasOne<Investor>().WithMany().HasForeignKey(e => e.InvestorId),
                    r => r.HasOne<Opportunity>().WithMany().HasForeignKey(e => e.OppertunityId));
            
            // Investor added to Opportunity ONLY ONCE !
            modelBuilder.Entity<Invesment>().HasIndex(i=>new {i.OppertunityId,i.InvestorId}).IsUnique();
        }

        public DbSet<User> users {get; set;}  
        public DbSet<Investor> investors {get; set;}
        public DbSet<Opportunity> opportunities {get;set;}
        public DbSet<Invesment> invesments {get;set;}
        public DbSet<InvesmentTransaction> InvesmentsTransactions {get;set;}
        public DbSet<OpportunityWallet> opportunitiesWallets {get;set;}
        public DbSet<InvestorWallet> InvestorsWallets {get;set;}



    }





}