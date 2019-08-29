using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using Cig.YanFa.Account.Repository.ContentOptions;
using Dapper;
using Microsoft.Extensions.Options;

namespace Cig.YanFa.Account.Repository
{
    public class CrmDBContext : DapperDBContext
    {
        public CrmDBContext(IOptions<CRMContextOptions> optionsAccessor) : base(optionsAccessor)
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
