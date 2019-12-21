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
        List<Product> GetAllProducts();
    }



    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(IDbFactory factory) : base(factory)
        {

        }
        

        public List<Product> GetAllProducts()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<Product> items = session.Find<Product>(stm => stm
                                                                    .Include<CategoryProduct>(j=>j.LeftOuterJoin())
                                                                    .OrderBy($"{Sql.Table<Product>()}.{nameof(Product.name)}")).ToList();
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
                    var existItems = session.Query<List<Product>>("Select * from products where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<List<Product>>("Select * from products where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }

    }

}