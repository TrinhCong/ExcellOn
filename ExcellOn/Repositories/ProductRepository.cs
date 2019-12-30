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
    public interface IProductRepository : IRepository<Product, int>
    {
        IEnumerable<Product> GetItems(string condition = "(1=1)");
        Product GetItem(int key);
    }



    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(IDbFactory factory) : base(factory)
        {

        }

        public Product GetItem(int key)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                return GetItems($"{Sql.Table<Product>()}.{nameof(Product.id)}={key}").FirstOrDefault();
            }
        }

        public IEnumerable<Product> GetItems(string condition = "(1=1)")
        {
            using (var session = Factory.Create<IAppSession>())
            {
                var items = session.Find<Product>(stm => stm.Where($"{condition}").Include<CategoryProduct>().OrderBy($"{Sql.Table<Product>()}.{nameof(Product.name)}"));
                foreach (var item in items)
                {
                    item.images = session.Find<ProductImage>(stm => stm.Where($"{nameof(ProductImage.product_id)}={item.id}"));
                }
                return items;
            }
        }

        public bool IsProductExist(Product entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<Product>("Select * from products where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<Product>("Select * from products where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }

    }

}