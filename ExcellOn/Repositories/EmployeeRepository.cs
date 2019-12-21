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



namespace ExcellOn.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
        List<Employee> GetAllEmployee();
    }



    public class EmployeeRepository : Repository<Employee, int>, IEmployeeRepository
    {
        public EmployeeRepository(IDbFactory factory) : base(factory)
        {
        }
        public List<Employee> GetAllEmployee()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Employee> items = session.Query<Employee>("Select * from employees").ToList();
                return items;
            }
        }
        public bool IsEmployeeExist(Employee entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<List<Employee>>("Select * from employees where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<List<Employee>>("Select * from employees where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }
    }

}