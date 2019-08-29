using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Cig.YanFa.Account.Repository.ContentOptions;
using Microsoft.Extensions.Options;

namespace Cig.YanFa.Account.Repository
{
   
    public class AcDBContext : DapperDBContext
    {
        public AcDBContext(IOptions<AcContextOptions> optionsAccessor) : base(optionsAccessor)
        {
        }
       
        private IDbConnection _connection;
        protected override IDbConnection CreateConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            return _connection;
        }
    }
}
