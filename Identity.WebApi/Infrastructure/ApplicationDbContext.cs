#region Using Directives
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace Identity.WebApi.Infrastructure
{
    /// <summary>
    /// Application Database Context
    /// </summary>
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #endregion
    }
}