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

    public interface IServiceRepository : IRepository<Service, int>
    {

        List<Service> GetAllService();
        List<Service> GetServiceByCategoryId(int CategoryId);
    }
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(IDbFactory factory) : base(factory)
        {
        }

        public List<Service> GetAllService()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Service> items = session.Find<Service>(stm=>stm.Include<CategoryService>().OrderBy($"{Sql.Table<Service>()}.{nameof(Service.name)}")).ToList();
                return items;
            }
        }

        public List<Service> GetServiceByCategoryId(int CategoryId)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Service> list = session.Query<Service>("Select * from services where cat_id=" + CategoryId).ToList();
                return list;
            }
        }

        public bool IsServiceExist(Service entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<List<Service>>("Select * from services where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<List<Service>>("Select * from services where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }
    }

}