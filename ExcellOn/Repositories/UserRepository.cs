using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork.Interfaces;
using ExcellOn.Models;
using Dapper;
using Dapper.FastCrud;
using System.Security.Cryptography;
using ExcellOn.Enums;

namespace ExcellOn.Repositories
{

    public interface IUserRepository : IRepository<User, int>
    {
        IEnumerable<User> GetUsers(string condition = "(1=1)");
        User GetUserById(int key);
        User GetUserByName(string userName);
        User Login(User entity);
        User Register(User entity);
        bool IsExist(User entity);
        IEnumerable<User> GetItems(string condition = "(1=1)");
        User GetItem(int key);
        string EncryptPassword(string password);
    }
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        public UserRepository(IDbFactory factory) : base(factory)
        {
        }
        public User GetItem(int key)
        {
            return GetItems($"{Sql.Table<User>()}.{nameof(User.id)}={key}").FirstOrDefault();
        }
        public IEnumerable<User>  GetItems(string condition="(1=1)")
        {
            using(var session = Factory.Create<IAppSession>())
            {
                condition = string.IsNullOrEmpty(condition) ? "(1=1)" : condition;
                return session.Find<User>(stm => stm.Where($"{condition}").Include<UserRole>().OrderBy($"{Sql.Table<User>()}.{nameof(User.display_name)}"));
            }
        }
        public User GetUserById(int key)
        {
            return GetUsers($"{Sql.Table<User>()}.{nameof(User.id)}={key}").FirstOrDefault();
        }
        public User GetUserByName(string userName)
        {
            return GetUsers($"{nameof(User.user_name)}='{userName}'").FirstOrDefault();
        }
        public IEnumerable<User> GetUsers(string condition="(1=1)")
        {
            using(var session = Factory.Create<IAppSession>())
            {
                return session.Find<User>(stm => stm.Where($"{condition}").Include<UserRole>());
            }
        }
        public bool IsExist(User entity)
        {
            var existItem = GetUserByName(entity.user_name.Trim().ToLower());
            return existItem != null;
        }
        public User Login(User entity)
        {
            var user = GetUserByName(entity.user_name.ToLower());
            if (user != null && checkPass(entity.password, user.hash_password))
            {
                return user;
            }
            return null;
        }
        public User Register(User entity)
        {
            using(var session = Factory.Create<IAppSession>())
            {
                try
                {
                    entity.hash_password = EncryptPassword(entity.password);
                    entity.role_id = EnumRole.CLIENT;
                    entity.user_name = entity.user_name.ToLower();
                    session.Insert(entity);
                    if (entity.id > 0)
                        return entity;
                }
                catch (Exception e)
                {
                }
                return null;
            }
        }

        public string EncryptPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hassPassword = Convert.ToBase64String(hashBytes);
            return hassPassword;
        }
        private bool checkPass(string password, string hassPassword)
        {
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(hassPassword);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }

}