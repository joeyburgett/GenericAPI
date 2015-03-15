#region Using Directives
using Identity.WebApi.Infrastructure;
using Identity.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace Identity.WebApi.Controllers
{
    /// <summary>
    /// Base API Controller
    /// </summary>
    public class BaseApiController
        : ApiController
    {
        #region Private Fields

        private ModelFactory _factory;
        private readonly ApplicationUserManager _appUserManager = null;
        private readonly ApplicationRoleManager _appRoleManager = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the application role manager.
        /// </summary>
        /// <value>
        /// The application role manager.
        /// </value>
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _appRoleManager 
                    ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        /// <summary>
        /// Gets the application user manager.
        /// </summary>
        /// <value>
        /// The application user manager.
        /// </value>
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _appUserManager 
                    ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        /// <summary>
        /// Gets the model factory.
        /// </summary>
        /// <value>
        /// The model factory.
        /// </value>
        protected ModelFactory ModelFactory
        {
            get
            {
                return _factory 
                    ?? (_factory = new ModelFactory(Request, AppUserManager));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the error result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                    foreach(var error in result.Errors)
                        ModelState.AddModelError("", error);

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}