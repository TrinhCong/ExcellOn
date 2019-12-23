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

    public interface IEmployeeRepository : IRepository<Employee, int>
    {
        Employee GetEmployeeById(int key);
        Employee GetEmployeeByName(string employeeName);
        Employee Login(Employee entity);
        Employee Register(Employee entity);
        bool IsExist(Employee entity);
        IEnumerable<Employee> GetItems(string condition = "(1=1)");
        Employee GetItem(int key);
    }
    public class EmployeeRepository : Repository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(IDbFactory factory) : base(factory)
        {

        }
        public Employee GetItem(int key)
        {
            return GetItems($"{Sql.Table<Employee>()}.{nameof(Employee.id)}={key}").FirstOrDefault();
        }
        public IEnumerable<Employee> GetItems(string condition = "(1=1)")
        {
            using (var session = Factory.Create<IAppSession>())
            {
                condition = string.IsNullOrEmpty(condition) ? "(1=1)" : condition;
                return session.Find<Employee>(stm => stm.Where($"{condition}").Include<UserRole>().OrderBy($"{Sql.Table<Employee>()}.{nameof(Employee.display_name)}"));
            }
        }
        public Employee GetEmployeeById(int key)
        {
            return GetItems($"{Sql.Table<Employee>()}.{nameof(Employee.id)}={key}").FirstOrDefault();
        }
        public Employee GetEmployeeByName(string employeeName)
        {
            return GetItems($"{Sql.Table<Employee>()}.{nameof(Employee.user_name)}='{employeeName}'").FirstOrDefault();
        }
        public bool IsExist(Employee entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = GetItems($"{Sql.Table<Employee>()}.{nameof(Employee.user_name)}='{entity.user_name.Trim()}'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = GetItems($"{Sql.Table<Employee>()}.{nameof(Employee.user_name)}='{entity.user_name.Trim()}' AND {Sql.Table<Employee>()}.{nameof(Employee.id)}<>{entity.id}");
                    return existItems.Count() > 0;
                }
            }
        }
        public Employee Login(Employee entity)
        {
            var employee = GetEmployeeByName(entity.user_name.ToLower());
            if (employee != null && AuthHelper.CheckPass(entity.password, employee.hash_password))
            {
                return employee;
            }
            return null;
        }
        public Employee Register(Employee entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                try
                {
                    entity.hash_password = AuthHelper.EncryptPassword(entity.password);
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