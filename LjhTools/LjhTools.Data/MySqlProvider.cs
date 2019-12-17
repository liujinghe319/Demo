using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LjhTools.Data
{
    public class MySqlProvider : IDbProvider
    {
        // Methods
        public void DeriveParameters(IDbCommand cmd)
        {
            if (cmd is SqlCommand)
            {
                MySqlCommandBuilder.DeriveParameters(cmd as MySqlCommand);
            }
        }

        public string GetLastIdSql()
        {
            return "SELECT SCOPE_IDENTITY()";
        }

        public DbProviderFactory Instance()
        {
            return MySqlClientFactory.Instance;
        }

        public bool IsBackupDatabase()
        {
            return true;
        }

        public bool IsCompactDatabase()
        {
            return true;
        }

        public bool IsDbOptimize()
        {
            return false;
        }

        public bool IsFullTextSearchEnabled()
        {
            return true;
        }

        public bool IsShrinkData()
        {
            return true;
        }

        public bool IsStoreProc()
        {
            return true;
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, int Size)
        {
            if (Size > 0)
            {
                return new MySqlParameter(ParamName, (MySqlDbType) DbType, Size);
            }
            return new MySqlParameter(ParamName, (MySqlDbType) DbType);
        }
    }
}