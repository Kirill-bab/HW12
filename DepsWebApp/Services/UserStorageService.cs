using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepsWebApp.Models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DepsWebApp.Services
{
    public class UserStorageService : IUserStorageService
    {
        private ILogger<UserStorageService> _logger;
        private UserStorageContext _dbContext;

        public UserStorageService(UserStorageContext dbContext, ILogger<UserStorageService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> TryGetUser(string encodedCredentials)
        {
            var credentials = (await Base64Decode(encodedCredentials)).Split(':');
            var login = credentials[0];
            var password = credentials[1];
            var match = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (match != default) return true;
            return false;
        }

        public async Task<bool> TryAddUserAsync(RegisterValidationModel model)
        {
            return await Task.Run(async () =>
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                var user = new User(model.Login, model.Password);
            if (await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login) is { })
                {
                    return false;
                }
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return true;
            });
        }

        private Task<string> Base64Decode(string data)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(data);
            return Task.FromResult(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }
    }
}
