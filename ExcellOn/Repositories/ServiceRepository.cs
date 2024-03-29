﻿using System;
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
        Service GetItem(int key);
        IEnumerable<Service> GetItems(string condition = "(1=1)");
    }

    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(IDbFactory factory) : base(factory)
        {

        }



        public Service GetItem(int key)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                return GetItems($"{Sql.Table<Service>()}.{nameof(Service.id)}={key}").FirstOrDefault();
            }
        }

        public IEnumerable<Service> GetItems(string condition = "(1=1)")
        {
            using (var session = Factory.Create<IAppSession>())
            {
                var items = session.Find<Service>(stm => stm.Where($"{condition}").Include<CategoryService>().OrderBy($"{Sql.Table<Service>()}.{nameof(Service.name)}"));
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
                    var existItems = session.Query<Service>("Select * from services where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<Service>("Select * from services where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }

    }

}