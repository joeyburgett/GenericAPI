#region Using Directives
using System.ComponentModel.DataAnnotations;
#endregion

namespace Identity.WebApi.Models
{
    /// <summary>
    /// Change Password Model
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets the current password.
        /// </summary>
        /// <value>
        /// The current password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters.", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}