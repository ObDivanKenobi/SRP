﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeOk.Models
{
    public class QueryResult
    {
        public bool Succeed { get; private set; }
        public List<string> Errors { get; private set; }

        public QueryResult()
        {
            Succeed = false;
            Errors = new List<string>();
        }

        public QueryResult(bool succeed, List<string> errors)
        {
            Succeed = succeed;
            Errors = errors;
        }
    }

    public class UserStorage
    {
        public string Path { get; private set; }

        public UserStorage(string path)
        {
            Path = path;
        }

        public QueryResult Save(User user)
        {
            if (user == null)
                return new QueryResult(true, new List<string>());

            bool succeed = true;
            List<string> errors = new List<string>();

            string data="";
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(Path, FileMode.OpenOrCreate)))
                {
                    data = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (IOException e)
            {
                succeed = false;
                errors.Add("Failed to open file");
            }

            if (!succeed)
                return new QueryResult(succeed, errors);

            if (data.Contains($"{user.Username}"))
            {
                succeed = false;
                errors.Add("This username is already in use");
                return new QueryResult(succeed, errors);
            }


            try
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(Path, FileMode.Append)))
                    writer.WriteLine($"{user.Username}:{user.PasswordHash}");
            }
            catch (IOException)
            {
                succeed = false;
                errors.Add("Failed to write to file");
            }

            return new QueryResult(succeed, errors);
        }

        public User FindUserByLogin(string username, string passwordHash)
        {
            User result = null;

            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(Path, FileMode.OpenOrCreate)))
                {
                    string data = reader.ReadToEnd();
                    if (data.Contains($"{username}:{passwordHash}"))
                        result = new User(username, passwordHash);
                }
            }
            catch{}

            return result;            
        }
    }
}
