#region Using Directives
using System;
using System.Configuration;
using Identity.WebApi.Infrastructure;
using Identity.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
#endregion

namespace Identity.WebApi
{
    /// <summary>
    /// Owin Startup
    /// </summary>
    public class Startup
    {
        #region Public Methods

        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            configureOAuthTokenGeneration(app);
            configureOAuthTokenConsumption(app);

            configureWebApi(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        #endregion

        #region Private Methods

        private void configureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["token_issuer"];
            var id = ConfigurationManager.AppSettings["audience_id"];
            var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["audience_secret"]);

            // JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[]
                    {
                        id
                    },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                });
        }

        private void configureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var json = config.Formatters
                .OfType<JsonMediaTypeFormatter>()
                .First();

            json.SerializerSettings.ContractResolver = 
                new CamelCasePropertyNamesContractResolver();
        }

        private void configureOAuthTokenGeneration(IAppBuilder app)
        {
            // Single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {                
#if !DEBUG
                AllowInsecureHttp = false,
#else
                AllowInsecureHttp = true,
#endif
                TokenEndpointPath = new PathString(ConfigurationManager.AppSettings["token_endpoint"]),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthProvider(),
                AccessTokenFormat = new Jwt(ConfigurationManager.AppSettings["token_jws_access"])
            };

            // Generate OAuth v2 Access Token
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }

        #endregion
    }
}