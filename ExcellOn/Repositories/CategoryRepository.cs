using Dapper;
using Dapper.FastCrud;
using ExcellOn.Models;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExcellOn.Repositories
{
    public interface ICategoryRepository<TEntity> : IRepository<TEntity, int> where TEntity : Category
    {

        List<CategoryProduct> GetAllProductCategories();
    }

    public class CategoryRepository<TEntity> : Repository<TEntity, int>, ICategoryRepository<TEntity> where TEntity : Category
    {
        public CategoryRepository(IDbFactory factory) : base(factory)
        {

        }


        public List<CategoryProduct> GetAllProductCategories()
        {
            using (var session = Factory.Create<IAppSession>())
            {
               List<CategoryProduct> items= session.Query<CategoryProduct>("select * from cat_products").ToList();
                return items;
            }
        }

        public bool IsCategoryProductExist(CategoryProduct entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                   var existItems= session.Query<List<CategoryProduct>>("Select * from cat_products where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<List<CategoryProduct>>("Select * from cat_products where name='" + entity.name + "' AND id<>"+entity.id);
                    return existItems.Count() > 0;
                }
            }
        }
    }
}