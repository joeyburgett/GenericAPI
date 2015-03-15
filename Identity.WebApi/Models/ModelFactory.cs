#region Using Directives
using Identity.WebApi.Infrastructure;
using System.Net.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace Identity.WebApi.Models
{
    /// <summary>
    /// Model Factory
    /// </summary>
    public class ModelFactory
    {
        #region Private Fields
        
        private readonly UrlHelper _urlHelper;
        private readonly ApplicationUserManager _appUserManager;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactory"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="manager">The manager.</param>
        public ModelFactory(HttpRequestMessage request, ApplicationUserManager manager)
        {
            _urlHelper = new UrlHelper(request);
            _appUserManager = manager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public RoleModel Create(IdentityRole role)
        {
            return new RoleModel
            {
                Url = _urlHelper.Link("GetRoleById", new {id = role.Id}),
                Id = role.Id,
                Name = role.Name
            };
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public UserModel Create(ApplicationUser user)
        {
            return new UserModel
            {
                Url = _urlHelper.Link("GetUserById", new {id = user.Id}),
                Id = user.Id,
                UserName = user.UserName,
                FullName = string.Format("{0}.{1}", user.FirstName, user.LastName),
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Level = user.Level,
                JoinDate = user.JoinDate,
                Roles = _appUserManager.GetRolesAsync(user.Id).Result,
                Claims = _appUserManager.GetClaimsAsync(user.Id).Result
            };
        }

        #endregion
    }
}