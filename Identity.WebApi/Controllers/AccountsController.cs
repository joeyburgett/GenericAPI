#region Using Directives
using Identity.WebApi.Infrastructure;
using Identity.WebApi.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace Identity.WebApi.Controllers
{
    /// <summary>
    /// Accounts Controller
    /// </summary>
    [RoutePrefix("api/accounts")]
    public class AccountsController 
        : BaseApiController
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [Route("users")]
        [Authorize]
        public IHttpActionResult GetUsers()
        {
            return Ok(AppUserManager.Users.ToList().Select(u => ModelFactory.Create(u)));
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("user/{id:guid}", Name = "GetUserById")]
        [Authorize]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await AppUserManager.FindByIdAsync(id);

            if (user != null)
                return Ok(ModelFactory.Create(user));

            return NotFound();
        }

        /// <summary>
        /// Gets the user by name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [Route("user/{username}")]
        [Authorize]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await AppUserManager.FindByNameAsync(username);

            if (user != null)
                return Ok(ModelFactory.Create(user));

            return NotFound();
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("create")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateUser(AccountModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Level = 3,
                JoinDate = DateTime.Now
            };

            var result = await AppUserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return GetErrorResult(result);

            var token = await AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callback = new Uri(Url.Link("EmailConfirmRoute", new {id = user.Id, token = token}));

            await
                AppUserManager.SendEmailAsync(user.Id, "Attention Required",
                    string.Format("<a href='{0}'>Confirm Email Address</a>", callback));

            var locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, ModelFactory.Create(user));
        }

        /// <summary>
        /// Confirms the email address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("EmailConfirm", Name = "EmailConfirmRoute")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ConfirmEmail(string id = "", string token = "")
        {
            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("", "Missing Parameters.");
                return BadRequest(ModelState);
            }

            var result = await AppUserManager.ConfirmEmailAsync(id, token);

            return result.Succeeded 
                ? Ok() 
                : GetErrorResult(result);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =
                await
                    AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword, model.NewPassword);

            return !result.Succeeded 
                ? GetErrorResult(result) 
                : Ok();
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("user/{id:guid}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            var user = await AppUserManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await AppUserManager.DeleteAsync(user);

                return !result.Succeeded
                    ? GetErrorResult(result)
                    : Ok();
            }

            return NotFound();
        }
    }
}