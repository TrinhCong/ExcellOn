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
using System.Security.Cryptography;
using ExcellOn.Enums;
using ExcellOn.Helpers;


namespace ExcellOn.Repositories
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder, int> 
    {
        List<ServiceOrder> getAllService();
        List<ServiceOrder> getAllServiceNoCare();
        List<ServiceOrder> getAllServiceCare();
        ServiceOrder GetItem(int key);
        IEnumerable<ServiceOrder> GetItems(string condition = "(1=1)");

    }
    public class ServiceOrderRepository : Repository<ServiceOrder, int>, IServiceOrderRepository
    {
        public ServiceOrderRepository(IDbFactory factory) : base(factory)
        {

        }
        public ServiceOrder GetItem(int key)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                return GetItems($"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.id)}={key}").FirstOrDefault();
            }
        }

        public IEnumerable<ServiceOrder> GetItems(string condition = "(1=1)")
        {
            using (var session = Factory.Create<IAppSession>())
            {
                var items = session.Find<ServiceOrder>(stm => stm.Where($"{condition}").Include<Service>().Include<CategoryPayType>().Include<Customer>().Include<Employee>().OrderBy($"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.registered_date)} DESC"));
                return items;
            }
        }

        public List<ServiceOrder> getAllService()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<ServiceOrder> items = session.Query<ServiceOrder>("select * from service_orders").ToList();
                return items;
            }
        }

        public List<ServiceOrder> getAllServiceCare(int x)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<ServiceOrder> items = session.Query<ServiceOrder>("select * from service_orders where employee_id = " + x).ToList();
                return items;
            }
        }

        public List<ServiceOrder> getAllServiceCare()
        {
            throw new NotImplementedException();
        }

        public List<ServiceOrder> getAllServiceNoCare()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                string condition = "employee_id is null or employee_id = 0";
                return session.Find<ServiceOrder>(stm => stm.Where($"{condition}").Include<Customer>().Include<Service>().Include<Employee>()).ToList();
            }
        }
    }
}