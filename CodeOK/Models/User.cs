using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeOk.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
