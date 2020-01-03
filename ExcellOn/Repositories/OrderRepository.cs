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
    public interface IOrderRepository : IRepository<Order, int>
    {
        //IEnumerable<Order> GetItems(string condition = "(1=1)");
        //Order GetItem(int key);
    }



    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(IDbFactory factory) : base(factory)
        {

        }

        //public Order GetItem(int key)
        //{
        //    using (var session = Factory.Create<IAppSession>())
        //    {
        //        return GetItems($"{Sql.Table<Order>()}.{nameof(Order.id)}={key}").FirstOrDefault();
        //    }
        //}

        //public IEnumerable<Order> GetItems(string condition = "(1=1)")
        //{
        //    using (var session = Factory.Create<IAppSession>())
        //    {
        //        var items = session.Find<Order>(stm => stm.Where($"{condition}").Include<CategoryOrder>().OrderBy($"{Sql.Table<Order>()}.{nameof(Order.name)}"));
        //        foreach (var item in items)
        //        {
        //            item.images = session.Find<OrderImage>(stm => stm.Where($"{nameof(OrderImage.order_id)}={item.id}"));
        //        }
        //        return items;
        //    }
        //}

    }

}