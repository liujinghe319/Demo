using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjhTools.Data
{
    public class MySqlDBHelper : DbHelperBase
    {
        // Fields
        private string _sConn;

        // Methods
        public MySqlDBHelper(string sConn)
        {
            this._sConn = sConn;
        }

        public override string ConnectionString()
        {
            if (base.m_connectionstring == null)
            {
                base.m_connectionstring = this._sConn;
            }
            return base.m_connectionstring;
        }

        public override IDbProvider Provider()
        {
            if (base.m_provider == null)
            {
                object lockHelper = base.lockHelper;
                lock (lockHelper)
                {
                    base.m_provider = DalFactory.DataBaseTypeProvider;
                }
            }
            return base.m_provider;
        }
    }


}