using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Identity.WebApi.Validators
{
    public class CustomPasswordValidator
        : PasswordValidator
    {
        /// <summary>
        /// Ensures that the string is of the required length and meets the configured requirements
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(string item)
        {
            var result = await base.ValidateAsync(item);

            // Add checks for enforcing valid passwords; i.e. sequences or "password"

            return result;
        }
    }
}