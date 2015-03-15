#region Using Directives
using System.ComponentModel.DataAnnotations;
#endregion

namespace Identity.WebApi.Models
{
    /// <summary>
    /// Create Role Model
    /// </summary>
    public class CreateRoleModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(256, ErrorMessage = "{0} must be at least {1} in length.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}