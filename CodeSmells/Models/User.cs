using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace CodeSmells.Models
{
    public class User
    {
        static Regex userInfoPattern = new Regex(@"^[\w-[\d_]]\w+[\w-[_]]$");
        static int requiredPasswordLength = 10;
        static int requiredUsernameLength = 5;
        
        public static string defaultStorage { get; private set; } = ConfigurationManager.AppSettings["UserStorage"];
        public static string Storage { get; set; } = defaultStorage;

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public User(string username, string password)
        {
            Username = username;
            PasswordHash = password.GetHashCode().ToString();
        }

        public bool Authenticate()
        {
            string path = Storage;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            StreamReader reader = new StreamReader(new FileStream(path, FileMode.OpenOrCreate));
            string data = reader.ReadToEnd();
            reader.Close();

            return data.Contains($"{Username}:{PasswordHash}");
        }

        public bool Save()
        {
            string path = Storage;//yh ConfigurationManager.AppSettings["UserStorage"];

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            StreamReader reader = new StreamReader(new FileStream(path, FileMode.OpenOrCreate));
            string data = reader.ReadToEnd();
            reader.Close();

            if (data.Contains($"{Username}"))
                return false;

            StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Append));
            writer.WriteLine($"{Username}:{PasswordHash}");
            writer.Close();

            return true;
        }

        public static bool IsValidUser(string username)
        {
            return username.Length >= requiredUsernameLength && userInfoPattern.IsMatch(username);
        }

        public static bool IsValidPassword(string password)
        {
            return password.Length >= requiredPasswordLength && userInfoPattern.IsMatch(password);
        }
    }
}
