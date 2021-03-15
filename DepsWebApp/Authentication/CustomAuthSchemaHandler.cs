using DepsWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DepsWebApp.Authentication
{
    public class CustomAuthSchemaHandler : AuthenticationHandler<CustomAuthSchemaOptions>
    {
        private readonly UserStorageService _storage;
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization)) return AuthenticateResult.NoResult();
            if(!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }
            var base64Credentials = authHeaderValue.ToString().Split(':');
            if(! await _storage.TryGetUser((base64Credentials[0], base64Credentials[1])))
            {
                return AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(new AuthenticationTicket(
                new System.Security.Claims.ClaimsPrincipal(new UserIdentity(new Random().Next(1000), "user")),
                CustomAuthSchema.Name));
        }

        public CustomAuthSchemaHandler(
            IOptionsMonitor<CustomAuthSchemaOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            UserStorageService storage)
            : base(options,loggerFactory,encoder, clock)
        {
            _storage = storage;
        }
    }
}
