#region Using Directives
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;
#endregion

namespace Identity.WebApi.Providers
{
    /// <summary>
    /// JSON Web Token Format
    /// </summary>
    public class Jwt 
        : ISecureDataFormat<AuthenticationTicket>
    {
        #region Private Fields

        private readonly string _issuer = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Jwt"/> class.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        public Jwt(string issuer)
        {
            _issuer = issuer;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Protects the specified authentication ticket.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">data</exception>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var audienceId = ConfigurationManager.AppSettings["audience_id"];
            var key = ConfigurationManager.AppSettings["audience_secret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(key);
            var signingKey = new HmacSigningCredentials(keyByteArray);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            var token = new JwtSecurityToken(_issuer, audienceId,
                data.Identity.Claims,
                issued.Value.UtcDateTime,
                expires.Value.UtcDateTime,
                signingKey);

            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        /// <summary>
        /// Generate authentication ticket from protected text.
        /// </summary>
        /// <param name="protectedText">The protected text.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}