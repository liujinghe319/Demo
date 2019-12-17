using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LjhTools.Data
{
    public class DalFactory
    {
        // Properties
        public static IDbProvider DataBaseTypeProvider
        {
            get
            {
                return new MySqlProvider();
            }
        }
    } 

}