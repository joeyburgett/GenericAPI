#region Using Directives
using System;
using Identity.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
#endregion

namespace Identity.WebApi.Infrastructure
{
    /// <summary>
    /// Application User Manager
    /// </summary>
    public class ApplicationUserManager
        : UserManager<ApplicationUser>
    {
        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
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
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var dbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext))
            {
                EmailService = new EmailService()
            };

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
                                                       
            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var provider = options.DataProtectionProvider;
            if (provider != null)
            {
                appUserManager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(provider.Create("Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(6)
                    };
            }

            return appUserManager;
        }

        #endregion
    }
}