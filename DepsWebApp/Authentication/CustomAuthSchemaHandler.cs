﻿using DepsWebApp.Services;
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
        private readonly IUserStorageService _storage;
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization)) return AuthenticateResult.NoResult();
            if(!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeaderValue))
            {
                return AuthenticateResult.NoResult();
            }
            var base64Credentials = authHeaderValue.ToString().Split(' ')[1];
            if (!await _storage.TryGetUser(base64Credentials))
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
            IUserStorageService storage)
            : base(options,loggerFactory,encoder, clock)
        {
            _storage = storage;
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
