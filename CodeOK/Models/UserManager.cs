using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeOk.Models
{
    public class UserManager
    {
        Validator PasswordValidator { get; set; } = new Validator() { AllowNonAlphanumeric = false, RequireLength = true, RequiredLength = 10 };
        Validator UsernameValidator { get; set; } = new Validator() { AllowNonAlphanumeric = false, RequireUppercase = true, RequireLength = true, RequiredLength = 5 };
        PasswordHasher PasswordHasher { get; set; } = new PasswordHasher();
        UserStorage UserStorage { get; set; } = new UserStorage(ConfigurationManager.AppSettings["UserStorage"]);

        public bool Authenticate(string username, string password)
        {
            return UserStorage.FindUserByLogin(username, PasswordHasher.GetHash(password)) != null;
        }

        public QueryResult AddUser(string username, string password)
        {
            List<string> errors = new List<string>();
            bool isValid = UsernameValidator.IsValid(username);
            if (!isValid)
                errors.Add("Имя пользователя должно состоять не менее чем из 10 символов, состоять только из латинских букв и цифр, а так же включать буквы в верхнем регистре.");

            if(!PasswordValidator.IsValid(password))
            {
                isValid &= false;
                errors.Add("Пароль должен состоять не менее чем из 10 символов, а так же состоять только из латинских букв и цифр.");
            }

            if (!isValid)
                return new QueryResult(isValid, errors);

            return UserStorage.Save(new User(username, PasswordHasher.GetHash(password)));
        }
    }
}
