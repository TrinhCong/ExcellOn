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
using ExcellOn.Helpers;

namespace ExcellOn.Repositories
{

    public interface ICustomerRepository : IRepository<Customer, int>
    {
        IEnumerable<Customer> GetCustomers(string condition = "(1=1)");
        Customer GetCustomerById(int key);
        Customer GetCustomerByName(string customerName);
        Customer Login(Customer entity);
        Customer Register(Customer entity);
        bool IsExist(Customer entity);
        IEnumerable<Customer> GetItems(string condition = "(1=1)");
        Customer GetItem(int key);
    }
    public class CustomerRepository : Repository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory factory) : base(factory)
        {
        }
        public Customer GetItem(int key)
        {
            return GetItems($"{Sql.Table<Customer>()}.{nameof(Customer.id)}={key}").FirstOrDefault();
        }
        public IEnumerable<Customer>  GetItems(string condition="(1=1)")
        {
            using(var session = Factory.Create<IAppSession>())
            {
                condition = string.IsNullOrEmpty(condition) ? "(1=1)" : condition;
                return session.Find<Customer>(stm => stm.Where($"{condition}").OrderBy($"{Sql.Table<Customer>()}.{nameof(Customer.display_name)}"));
            }
        }
        public Customer GetCustomerById(int key)
        {
            return GetCustomers($"{Sql.Table<Customer>()}.{nameof(Customer.id)}={key}").FirstOrDefault();
        }
        public Customer GetCustomerByName(string customerName)
        {
            return GetCustomers($"{nameof(Customer.user_name)}='{customerName}'").FirstOrDefault();
        }
        public IEnumerable<Customer> GetCustomers(string condition="(1=1)")
        {
            using(var session = Factory.Create<IAppSession>())
            {
                return session.Find<Customer>(stm => stm.Where($"{condition}"));
            }
        }
        public bool IsExist(Customer entity)
        {
            var existItem = GetCustomerByName(entity.user_name.Trim().ToLower());
            return existItem != null;
        }
        public Customer Login(Customer entity)
        {
            var customer = GetCustomerByName(entity.user_name.ToLower());
            if (customer != null && AuthHelper.CheckPass(entity.password, customer.hash_password))
            {
                return customer;
            }
            return null;
        }
        public Customer Register(Customer entity)
        {
            using(var session = Factory.Create<IAppSession>())
            {
                try
                {
                    entity.hash_password =AuthHelper.EncryptPassword(entity.password);
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
    }

}