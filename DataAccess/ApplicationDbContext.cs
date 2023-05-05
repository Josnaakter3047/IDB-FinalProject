using DataModels.Auth;
using DataModels.Other;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<UserTokens> UserToken { get; set; }

       

       


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(x =>
            {
                x.HasIndex(x => x.Email).IsUnique();
                x.HasIndex(x => x.PhoneNumber).IsUnique();
            });


            builder.Entity<UserTokens>(x =>
            {
                x.HasIndex(x => x.ApplicationUserId).IsUnique();
            });

           
        }
    }
}
