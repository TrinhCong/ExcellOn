using Dapper;
using Dapper.FastCrud;
using ExcellOn.Models;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.Repository.UnitOfWork;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExcellOn.Repositories
{
    public interface ICategoryRepository<TEntity> : IRepository<TEntity, int> where TEntity : Category
    {

        List<CategoryProduct> GetAllProductCategories();
        List<CategoryService> GetAllServiceCategories();
    }

    public class CategoryRepository<TEntity> : Repository<TEntity, int>, ICategoryRepository<TEntity> where TEntity : Category
    {
        public CategoryRepository(IDbFactory factory) : base(factory)
        {

        }

        /*===== CategoryService =====*/
        public List<CategoryService> GetAllServiceCategories()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<CategoryService> items = session.Query<CategoryService>("Select * from cat_services").ToList();
                return items;
            }
        }
        public bool IsCategoryServiceExist(CategoryService entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<CategoryService>("Select * from cat_services where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<CategoryService>("Select * from cat_services where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }




        /*===== CategoryProduct =====*/

        public List<CategoryProduct> GetAllProductCategories()
        {
            using (var session = Factory.Create<IAppSession>())
            {
                List<CategoryProduct> items = session.Query<CategoryProduct>("select * from cat_products").ToList();
                return items;
            }
        }

        public bool IsCategoryProductExist(CategoryProduct entity)
        {
            using (var session = Factory.Create<IAppSession>())
            {
                if (entity.id == 0)
                {
                    var existItems = session.Query<CategoryProduct>("Select * from cat_products where name='" + entity.name + "'");
                    return existItems.Count() > 0;
                }
                else
                {
                    var existItems = session.Query<CategoryProduct>("Select * from cat_products where name='" + entity.name + "' AND id<>" + entity.id);
                    return existItems.Count() > 0;
                }
            }
        }

    }
}