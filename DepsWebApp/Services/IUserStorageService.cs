using DepsWebApp.Models;
using System.Threading.Tasks;

namespace DepsWebApp.Services
{
    public interface IUserStorageService
    {
        public Task<bool> TryGetUser(string encodedCredentials);
        public Task<bool> TryAddUserAsync(RegisterValidationModel model);
    }
}