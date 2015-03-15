#region Using Directives
using Identity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Identity.WebApi.Validators
{
    /// <summary>
    /// Custom User Validator
    /// </summary>
    public class CustomUserValidator 
        : UserValidator<ApplicationUser>
    {

        #region Private Fields

        private readonly List<string> _blackListDomains = new List<string>
        {
            "tempinbox.com"
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomUserValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CustomUserValidator(ApplicationUserManager manager)
            : base(manager)
        { }

        #endregion

        #region UserValidator Overrides

        /// <summary>
        /// Validates user asynchronously.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            var result = await base.ValidateAsync(user);
            
            // Perform email domain validation
            var domain = user.Email.Split('@').Last();

            if (!_blackListDomains.Contains(domain)) 
                return result;

            var errors = result.Errors.ToList();
            errors.Add(string.Format("Domain {0} not allowed.", domain));

            result = new IdentityResult(errors);

            return result;
        }

        #endregion
    }
}