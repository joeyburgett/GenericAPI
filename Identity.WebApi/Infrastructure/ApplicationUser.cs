#region Using Directives
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
#endregion

namespace Identity.WebApi.Infrastructure
{
    /// <summary>
    /// Application User
    /// </summary>
    public class ApplicationUser
        : IdentityUser
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [Required]
        public byte Level { get; set; }

        /// <summary>
        /// Gets or sets the join date.
        /// </summary>
        /// <value>
        /// The join date.
        /// </value>
        [Required]
        public DateTime JoinDate { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the user identity.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, 
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // TODO: Add custom user claims here

             return userIdentity;
        }

        #endregion
    }
}