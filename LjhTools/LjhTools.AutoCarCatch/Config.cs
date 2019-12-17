using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LjhTools.AutoCarCatch
{
    public static class Config
    {
        public static string SavePath = ConfigurationManager.AppSettings["savepath"];
    }
}
