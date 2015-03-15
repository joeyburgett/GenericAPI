#region Using Directives
using Identity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace Identity.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Entity Framework Code-First DBContext Configuration
    /// </summary>
    internal sealed class Configuration 
        : DbMigrationsConfiguration<ApplicationDbContext>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #endregion

        #region DbMigrationsConfiguration Overrides

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            var manager 
                = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser
            {
                UserName = "Administrator",
                Email = "admin@mail.com",
                EmailConfirmed = true,
                FirstName = "Joey",
                LastName = "Burgett",
                Level = 1,
                JoinDate = DateTime.Now
            };

            manager.Create(user, "@Password1");
        }

        #endregion
    }
}