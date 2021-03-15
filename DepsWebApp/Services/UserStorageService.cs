using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepsWebApp.Models;
using System.Collections.Concurrent;

namespace DepsWebApp.Services
{
    public class UserStorageService
    {
        private readonly ConcurrentBag<RegisterValidationModel> _users = new ConcurrentBag<RegisterValidationModel>();

        public async Task<bool> TryGetUser(string encodedCredentials)
        {
            var credentials = (await Base64Decode(encodedCredentials)).Split(':');
            var login = credentials[0];
            var password = credentials[1];
            if (_users.Where(u => u.Login == login && u.Password == password).ToList().Count != 0) return true;
            return false;
        }

        public async Task<bool> TryAddUserAsync(RegisterValidationModel user)
        {
            return await Task.Run(() =>
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                if (_users.Where(u => u.Login == user.Login && u.Password == user.Password)
                .ToList().Count != 0)
                {
                    return Task.FromResult(false);
                }
                _users.Add(user);
                return Task.FromResult(true);
            });
        }

        private Task<string> Base64Decode(string data)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(data);
            return Task.FromResult(System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
        }
    }
}
