using Dapper;
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


    }

    public class CategoryRepository<TEntity> : Repository<TEntity, int>, ICategoryRepository<TEntity> where TEntity : Category
    {
        public CategoryRepository(IDbFactory factory) : base(factory)
        {
        }

    }
}