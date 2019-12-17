using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace LjhTools.Data
{
    public interface IDbProvider
    {
        // Methods
        void DeriveParameters(IDbCommand cmd);
        string GetLastIdSql();
        DbProviderFactory Instance();
        bool IsBackupDatabase();
        bool IsCompactDatabase();
        bool IsDbOptimize();
        bool IsFullTextSearchEnabled();
        bool IsShrinkData();
        bool IsStoreProc();
        DbParameter MakeParam(string ParamName, DbType DbType, int Size);
    }
}