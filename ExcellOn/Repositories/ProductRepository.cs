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


    }
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(IDbFactory factory) : base(factory)
        {
        }


    }

}