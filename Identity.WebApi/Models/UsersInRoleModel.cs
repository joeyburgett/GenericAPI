#region Using Directives
using System.Collections.Generic;
#endregion

namespace Identity.WebApi.Models
{
    /// <summary>
    /// Users In Role
    /// </summary>
    public class UsersInRoleModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the enrolled users.
        /// </summary>
        /// <value>
        /// The enrolled users.
        /// </value>
        public IList<string> EnrolledUsers { get; set; }

        /// <summary>
        /// Gets or sets the removed users.
        /// </summary>
        /// <value>
        /// The removed users.
        /// </value>
        public IList<string> RemovedUsers { get; set; }
    }
}