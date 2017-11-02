using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeSmells.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeSmells.Models.Tests
{
    [TestClass()]
    public class UserTests
    {
        string path = @"TestData\tmp.dat";

        [TestMethod()]
        public void Authenticate_LoginDataOk_Success()
        {
            string login = "TestUser",
                   password = "TestPassword";
            User user = new User(login, password);
            User.Storage = path;

            user.Save();

            Assert.IsTrue(user.Authenticate());

            ClearFile();
        }

        [TestMethod()]
        public void Authenticate_WrongLogin_Fail()
        {
            string login = "TestUser",
                   password = "TestPassword",
                   wrongLogin = "WrongLogin";

            User user = new User(login, password);
            User.Storage = path;

            user.Save();

            user = new User(wrongLogin, password);
            Assert.IsFalse(user.Authenticate());

            ClearFile();
        }

        [TestMethod()]
        public void Authenticate_WrongPasswoord_Fail()
        {
            string login = "TestUser",
                   password = "TestPassword",
                   wrongPassword = "WrongPassword";

            User user = new User(login, password);
            User.Storage = path;

            user.Save();

            user = new User(login, wrongPassword);
            Assert.IsFalse(user.Authenticate());

            ClearFile();
        }

        [TestMethod()]
        public void Save_UserDoiesNotExist_Success()
        {
            string login = "TestUser",
                   password = "TestPassword";
            User user = new User(login, password);
            User.Storage = path;

            user.Save();

            StreamReader reader = new StreamReader(File.Open(path, FileMode.Open));
            string data = reader.ReadToEnd();
            Assert.IsTrue(data.Contains($"{user.Username}:{user.PasswordHash}"));
            reader.Close();

            ClearFile();
        }

        [TestMethod()]
        public void Save_UserAlreadyExists_Fail()
        {
            string login = "TestUser",
                   password = "TestPassword";
            User user = new User(login, password);
            User.Storage = path;

            user.Save();

            Assert.IsFalse(user.Save());

            ClearFile();
        }

        void ClearFile()
        {
            StreamReader reader = new StreamReader(File.Open(path, FileMode.Create));
            reader.Close();
        }
    }
}