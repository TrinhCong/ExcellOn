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
using ExcellOn.ViewModels;

namespace ExcellOn.Repositories
{
    public interface IDepartmentRepository : IRepository<Department, int>
    {
        List<Department> GetAll();
        bool IsDepartmentExist(Department entity);
        List<DepartmentView> GetAllView();
    }



    public class DepartmentRepository : Repository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbFactory factory) : base(factory)
        {
        }

        public List<Department> GetAll()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Department> items = session.Query<Department>("Select * from departments").ToList();
                return items;
            }
        }

        public List<DepartmentView> GetAllView()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<DepartmentView> items = session.Query<DepartmentView>("select id,name,cat_id,(select cd.name from cat_departments cd where cd.id = d.cat_id) as catName, description from departments d;").ToList();
                return items;
            }
        }

        public bool IsDepartmentExist(Department entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Department> items = session.Query<Department>("select * from departments where name = '"+ entity .name+ "'").ToList();
                return items.Count > 0;
            }
        }
    }

}