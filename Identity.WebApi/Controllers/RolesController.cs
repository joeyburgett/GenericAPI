#region Using Directives
using Identity.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace Identity.WebApi.Controllers
{
    /// <summary>
    /// Roles Controller
    /// </summary>
    [Authorize(Roles="Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController
        : BaseApiController
    {

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await AppRoleManager.FindByIdAsync(id);
            if (role != null)
                return Ok(ModelFactory.Create(role));

            return NotFound();
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            return Ok(AppRoleManager.Roles);
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new IdentityRole
            {
                Name = model.Name
            };

            var result = await AppRoleManager.CreateAsync(role);
            if (!result.Succeeded)
                return GetErrorResult(result);

            var locationHeader = new Uri(Url.Link("GetRoleById", new
            {
                id = role.Id
            }));

            return Created(locationHeader, ModelFactory.Create(role));
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {
            var role = await AppRoleManager.FindByIdAsync(id);

            if (role == null) 
                return NotFound();

            var result = await AppRoleManager.DeleteAsync(role);
            return !result.Succeeded 
                ? GetErrorResult(result) 
                : Ok();
        }

        /// <summary>
        /// Manages the users in specified role.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await AppRoleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (var user in model.EnrolledUsers)
            {
                var appUser = await AppUserManager.FindByIdAsync(user);
                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (AppUserManager.IsInRole(user, role.Name)) 
                    continue;

                var result = await AppUserManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                    ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
            }

            foreach (var user in model.RemovedUsers)
            {
                var appUser = await AppUserManager.FindByIdAsync(user);
                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                var result = await AppUserManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}