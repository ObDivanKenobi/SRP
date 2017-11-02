using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeOk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeOk.Models.Tests
{
    [TestClass()]
    public class UserStorageTests
    {
        static string path = "test.txt";

        void ClearFile()
        {
            StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create));
            writer.Close();
        }

        [TestMethod()]
        public void Save_NewUser_Success()
        {
            string username = "testUser",
                   passwordHash = "testPassword".GetHashCode().ToString();

            UserStorage storage = new UserStorage(path);
            storage.Save(new User(username, passwordHash));

            StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
            string data = reader.ReadToEnd();
            reader.Close();
            ClearFile();

            Assert.IsTrue(data.Contains($"{username}:{passwordHash}"));
        }

        [TestMethod()]
        public void Save_ExistedUser_Fail()
        {
            string username = "testUser",
                   passwordHash = "testPassword".GetHashCode().ToString();
            User user = new User(username, passwordHash);

            UserStorage storage = new UserStorage(path);
            storage.Save(user);
            storage.Save(user);

            Assert.IsFalse(storage.Save(user).Succeed);
        }

        [TestMethod()]
        public void FindUserByLogin_ExistedUser_Success()
        {
            string username = "testUser",
                   passwordHash = "testPassword".GetHashCode().ToString();

            UserStorage storage = new UserStorage(path);
            storage.Save(new User(username, passwordHash));
            User foundUser = storage.FindUserByLogin(username, passwordHash);
            ClearFile();

            Assert.IsNotNull(foundUser);
        }

        [TestMethod()]
        public void FindUserByLogin_NotExistedUser_Fail()
        {
            string username = "testUser",
                   passwordHash = "testPassword".GetHashCode().ToString(),
                   otherUsername = "otherUser",
                   otherPasswordHash = "otherPassword".GetHashCode().ToString();
            User user = new User(username, passwordHash);
            UserStorage storage = new UserStorage(path);

            storage.Save(user);
            User foundUser = storage.FindUserByLogin(otherUsername, otherPasswordHash);
            ClearFile();

            Assert.IsNull(foundUser);
        }
    }
}