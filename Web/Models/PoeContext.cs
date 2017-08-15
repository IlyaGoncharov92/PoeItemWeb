using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Web.Models
{
    public class PoeContext : IdentityDbContext<ApplicationUser>
    {
        public PoeContext() : base("PoeContext") { }

        public static PoeContext Create()
        {
            return new PoeContext();
        }

        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDetail> ItemDetails { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserConnections = new List<UserConnection>();
            Items = new List<Item>();
        }
        
        public virtual ICollection<UserConnection> UserConnections { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var db = context.Get<PoeContext>();
            return new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }
    }
}