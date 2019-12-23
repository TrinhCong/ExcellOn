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
using Smooth.IoC.Repository.UnitOfWork.Extensions;

namespace ExcellOn.Repositories
{

    public interface IDepartmentRepository : IRepository<Department, int>
    {

        List<Department> GetAllDepartment();
    }
    public class DepartmentRepository : Repository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbFactory factory) : base(factory)
        {
        }

        public List<Department> GetAllDepartment()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Department> items = session.Find<Department>(stm => stm.OrderBy($"{Sql.Table<Department>()}.{nameof(Department.name)}")).ToList();
                return items;
            }
        }
        public bool IsDepartmentExist(Department entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<Department>("Select * from departments where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<Department>("Select * from departments where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }
    }

}