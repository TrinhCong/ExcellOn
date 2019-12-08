using Dapper.FastCrud;
using Smooth.IoC.UnitOfWork.Abstractions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ExcellOn.Repositories.Sessions
{
    public interface IAppSession : ISession
    {
       
    }

    public class AppSession : Session<SqlConnection>, IAppSession
    {
        public AppSession(IDbFactory session)
                : base(session, WebConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString)
        {
            OrmConfiguration.DefaultDialect = Dapper.FastCrud.SqlDialect.MsSql;
        }
    } 
}