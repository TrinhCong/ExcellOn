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
    public interface IServiceRepository : IRepository<Service, int>
    {
        List<Service> GetAllServices();
    }



    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(IDbFactory factory) : base(factory)
        {

        }


        public List<Service> GetAllServices()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Service> items = session.Find<Service>(stm => stm
                                                                    .Include<CategoryService>(j => j.LeftOuterJoin())
                                                                    .OrderBy($"{Sql.Table<Service>()}.{nameof(Service.name)}")).ToList();
                foreach (var item in items)
                {
                    item.images = session.Find<ServiceImage>(stm => stm.Where($"{nameof(ServiceImage.service_id)}={item.id}"));
                }
                return items;
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