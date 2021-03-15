using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DepsWebApp.Authentication
{
    /// <summary>
    /// User Identity
    /// </summary>
    public class UserIdentity : ClaimsIdentity
    {
        public int UserId { get; set; }
        public string Role { get; set; }

        public UserIdentity(int userId, string role) 
            : base(CreateClaimsIdentity(userId, role), CustomAuthSchema.Type)
        {
            UserId = userId;
            Role = role;
        }

        private static IEnumerable<Claim> CreateClaimsIdentity(
            int userId,
            string role)
        {
            var claimsIdentity = new List<Claim>
            {
                new Claim(DefaultNameClaimType, userId.ToString())
            };

            if(role != null)
            {
                claimsIdentity.Add(new Claim(DefaultRoleClaimType, role));
            }

            return claimsIdentity;
        }
    }
}
