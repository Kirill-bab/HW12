using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DepsWebApp.Models
{
    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public User(string login, string password)
        {
            if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            Login = login;
            Password = password;
            Id = Guid.NewGuid();
        }
    }
}
