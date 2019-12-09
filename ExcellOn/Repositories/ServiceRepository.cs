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


    }
    public class ServiceRepository : Repository<Service, int>, IServiceRepository
    {
        public ServiceRepository(IDbFactory factory) : base(factory)
        {
        }


    }

}