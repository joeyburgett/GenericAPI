#region Using Directives
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
#endregion

namespace Identity.WebApi.Infrastructure
{
    /// <summary>
    /// Application Role Manager
    /// </summary>
    public class ApplicationRoleManager
        : RoleManager<IdentityRole>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleManager"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        { }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var roleManager =
                new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

            return roleManager;
        }

        #endregion
    }
}