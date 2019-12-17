using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace LjhTools.Data
{
    public class SplitPages
    {
        // Fields
        public static readonly string sPre = string.Empty;

        // Methods
        public static IDataReader GetListPages_CusTomSearch(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, out int totalRecords)
        {
            if (string.IsNullOrEmpty(strFields))
            {
                strFields = "*";
            }
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@Tables", SqlDbType.VarChar, 0x3e8), new SqlParameter("@PrimaryKey", SqlDbType.VarChar, 10), new SqlParameter("@Sort", SqlDbType.VarChar, 200), new SqlParameter("@CurrentPage", SqlDbType.Int, 4), new SqlParameter("@PageSize", SqlDbType.Int, 4), new SqlParameter("@Fields", SqlDbType.VarChar, 0x3e8), new SqlParameter("@Filter", SqlDbType.VarChar, 0x3e8) };
            commandParameters[0].Value = sTableName;
            commandParameters[1].Value = KeyField;
            commandParameters[2].Value = OrderBy.Trim();
            commandParameters[3].Value = PageIndex;
            commandParameters[4].Value = PageSize;
            commandParameters[5].Value = strFields;
            commandParameters[6].Value = strWhere.Trim();
            IDataReader reader = DB.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}SplitPages", sPre), commandParameters);
            string commandText = string.Format("select count(*)  from {0}  {1} ", sTableName, string.IsNullOrEmpty(strWhere) ? "" : (" Where " + strWhere));
            object objA = DB.ExecuteScalar(CommandType.Text, commandText);
            totalRecords = 1;
            if (!object.Equals(objA, null))
            {
                totalRecords = int.Parse(objA.ToString());
            }
            return reader;
        }

        public static IDataReader GetListPages_SP(string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, out int totalRecords)
        {
            return GetListPages_SP(sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, out totalRecords, sPre);
        }

        public static IDataReader GetListPages_SP(string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, out int totalRecords, string TablePrefix)
        {
            return GetListPages_SPPT(null, sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, out totalRecords, TablePrefix);
        }

        public static IDataReader GetListPages_SP(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, out int totalRecords, string TablePrefix)
        {
            return GetListPages_SPPT(DB, sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, out totalRecords, TablePrefix);
        }

        private static IDataReader GetListPages_SPPT(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, out int totalRecords, string TablePrefix)
        {
            DbHelperBase base2 = DB;
            sTableName = TablePrefix + sTableName;
            if (string.IsNullOrEmpty(strFields))
            {
                strFields = "*";
            }
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("@Tables", SqlDbType.VarChar, 0x3e8), new SqlParameter("@PrimaryKey", SqlDbType.VarChar, 10), new SqlParameter("@Sort", SqlDbType.VarChar, 200), new SqlParameter("@CurrentPage", SqlDbType.Int, 4), new SqlParameter("@PageSize", SqlDbType.Int, 4), new SqlParameter("@Fields", SqlDbType.VarChar, 0x3e8), new SqlParameter("@Filter", SqlDbType.VarChar, 0x3e8) };
            commandParameters[0].Value = sTableName;
            commandParameters[1].Value = KeyField;
            commandParameters[2].Value = OrderBy.Trim();
            commandParameters[3].Value = PageIndex;
            commandParameters[4].Value = PageSize;
            commandParameters[5].Value = strFields;
            commandParameters[6].Value = strWhere.Trim();
            IDataReader reader = base2.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}SplitPages", TablePrefix), commandParameters);
            string commandText = string.Format("select count(*)  from {0}  {1} ", sTableName, string.IsNullOrEmpty(strWhere) ? "" : (" Where " + strWhere));
            object objA = base2.ExecuteScalar(CommandType.Text, commandText);
            totalRecords = 1;
            if (!object.Equals(objA, null))
            {
                totalRecords = int.Parse(objA.ToString());
            }
            return reader;
        }

        public static string GetListPagesSql2005(string sTableName, int PageIndex, int PageSize, string Fileds, string strWhere, string oderby)
        {
            if (PageIndex > 0)
            {
                PageIndex--;
            }
            int num = PageIndex * PageSize;
            int num2 = num + PageSize;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select {0},RankID ", Fileds);
            builder.Append(" FROM  ");
            builder.Append(string.Format(" (select {0},ROW_NUMBER() OVER(ORDER BY {1} ) AS RankID   ", Fileds, oderby));
            builder.Append(string.Format(" FROM {0}", sTableName));
            if (strWhere.Trim() != "")
            {
                builder.Append(" WHERE ");
                builder.Append(strWhere);
            }
            builder.Append(" ) AS NewRowNumber   WHERE RankID>");
            builder.Append(num);
            builder.Append(" AND RankID<=  ");
            builder.Append(num2);
            return builder.ToString();
        }

        public static IDataReader GetListPagesSql2005(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string Fileds, string strWhere, string oderby, out int RecordCount)
        {
            string commandText = string.Format("select count(*)  from {0}  {1} ", sTableName, string.IsNullOrEmpty(strWhere) ? "" : (" Where " + strWhere));
            object objA = DB.ExecuteScalar(CommandType.Text, commandText);
            RecordCount = 1;
            if (!object.Equals(objA, null))
            {
                RecordCount = int.Parse(objA.ToString());
            }
            string str2 = GetListPagesSql2005(sTableName, PageIndex, PageSize, Fileds, strWhere, oderby);
            return DB.ExecuteReader(CommandType.Text, str2);
        }

        public static string GetSplitPagesMySql(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, string TablePrefix)
        {
            return GetSplitPagesMySql(DB, sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, TablePrefix, true);
        }

        public static string GetSplitPagesMySql(DbHelperBase DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, string TablePrefix, bool isint)
        {
            sTableName = TablePrefix + sTableName;
            string commandText = string.Empty;
            string str2 = string.Empty;
            if (PageIndex > 0)
            {
                PageIndex--;
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                str2 = " ORDER BY " + OrderBy;
            }
            else if (!string.IsNullOrEmpty(KeyField))
            {
                str2 = " ORDER BY " + KeyField + " desc";
            }
            int num = PageIndex * PageSize;
            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = " where " + strWhere;
            }
            object[] objArray1 = new object[] { "select ", KeyField, " from ", sTableName, " ", strWhere, str2, " limit ", num, ",", PageSize, ";" };
            commandText = string.Concat(objArray1);
            StringBuilder builder = new StringBuilder();
            if (isint)
            {
                using (IDataReader reader = DB.ExecuteReader(CommandType.Text, commandText))
                {
                    while (reader.Read())
                    {
                        builder.Append(reader.GetString(0));
                        builder.Append(",");
                    }
                }
            }
            else
            {
                using (IDataReader reader2 = DB.ExecuteReader(CommandType.Text, commandText))
                {
                    while (reader2.Read())
                    {
                        builder.Append("'");
                        builder.Append(reader2.GetString(0));
                        builder.Append("'");
                        builder.Append(",");
                    }
                }
            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            else
            {
                builder.Append("0");
            }
            if (string.IsNullOrEmpty(strFields))
            {
                strFields = "*";
            }
            object[] objArray2 = new object[] { "select ", strFields, " from ", sTableName, " where ", KeyField, " in (", builder, ")", str2 };
            return string.Concat(objArray2);
        }

        public static string GetSplitPagesMySql(DbHelperBase DB, string sTableName1, string sTableName2, int PageSize, int PageIndex, string strFields, string sTable1Key, string sTable2Key, string OrderBy, string strWhere, string TablePrefix)
        {
            sTableName1 = TablePrefix + sTableName1;
            sTableName2 = TablePrefix + sTableName2;
            string commandText = string.Empty;
            string str2 = string.Empty;
            if (PageIndex > 0)
            {
                PageIndex--;
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                str2 = " ORDER BY " + OrderBy;
            }
            int num = PageIndex * PageSize;
            object[] args = new object[] { sTableName1, sTable1Key, sTableName2, sTable2Key };
            string str3 = string.Format(" where {0}.{1}={2}.{3}", args);
            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = str3 + " and " + strWhere;
            }
            else
            {
                strWhere = str3;
            }
            object[] objArray2 = new object[] { "select ", sTableName1, ".", sTable1Key, " from ", sTableName1, ",", sTableName2, " ", strWhere, str2, " limit ", num, ",", PageSize, ";" };
            commandText = string.Concat(objArray2);
            StringBuilder builder = new StringBuilder();
            using (IDataReader reader = DB.ExecuteReader(CommandType.Text, commandText))
            {
                while (reader.Read())
                {
                    builder.Append(reader.GetString(0));
                    builder.Append(",");
                }
            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            else
            {
                builder.Append("0");
            }
            if (string.IsNullOrEmpty(strFields))
            {
                strFields = "*";
            }
            else
            {
                strFields = string.Format(strFields, sTableName1, sTableName2);
            }
            object[] objArray3 = new object[] {
            "select ", strFields, " from ", sTableName1, ",", sTableName2, " where ", sTableName1, ".", sTable1Key, "=", sTableName2, ".", sTable2Key, " and ", sTableName1,
            ".", sTable1Key, " in (", builder, ")", str2
         };
            return string.Concat(objArray3);
        }

        public static string GetSplitPagesSql(string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere)
        {
            return GetSplitPagesSql(sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, sPre);
        }

        public static string GetSplitPagesSql(string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, string TablePrefix)
        {
            return GetSplitPagesSqlPT(sTableName, PageSize, PageIndex, strFields, KeyField, OrderBy, strWhere, TablePrefix);
        }

        //public static string GetSplitPagesSqlite(SQLiteDBHelper DB, string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, string TablePrefix, bool isint)
        //{
        //    sTableName = TablePrefix + sTableName;
        //    string cmdText = string.Empty;
        //    string str2 = string.Empty;
        //    if (PageIndex > 0)
        //    {
        //        PageIndex--;
        //    }
        //    if (!string.IsNullOrEmpty(OrderBy))
        //    {
        //        str2 = " ORDER BY " + OrderBy;
        //    }
        //    else if (!string.IsNullOrEmpty(KeyField))
        //    {
        //        str2 = " ORDER BY " + KeyField + " desc";
        //    }
        //    int num = PageIndex * PageSize;
        //    if (!string.IsNullOrEmpty(strWhere))
        //    {
        //        strWhere = " where " + strWhere;
        //    }
        //    object[] objArray1 = new object[] { "select ", KeyField, " from ", sTableName, " ", strWhere, str2, " limit ", num, ",", PageSize, ";" };
        //    cmdText = string.Concat(objArray1);
        //    StringBuilder builder = new StringBuilder();
        //    if (isint)
        //    {
        //        using (SQLiteDataReader reader = DB.ExecuteReader(1, cmdText))
        //        {
        //            while (reader.Read())
        //            {
        //                builder.Append(reader.GetInt64(0));
        //                builder.Append(",");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        using (SQLiteDataReader reader2 = DB.ExecuteReader(1, cmdText))
        //        {
        //            while (reader2.Read())
        //            {
        //                builder.Append("'");
        //                builder.Append(reader2.GetString(0));
        //                builder.Append("'");
        //                builder.Append(",");
        //            }
        //        }
        //    }
        //    if (builder.Length > 1)
        //    {
        //        builder.Remove(builder.Length - 1, 1);
        //    }
        //    else
        //    {
        //        builder.Append("0");
        //    }
        //    if (string.IsNullOrEmpty(strFields))
        //    {
        //        strFields = "*";
        //    }
        //    object[] objArray2 = new object[] { "select ", strFields, " from ", sTableName, " where ", KeyField, " in (", builder, ")", str2 };
        //    return string.Concat(objArray2);
        //}

        //public static string GetSplitPagesSqlite(SQLiteDBHelper DB, string sTableName1, string sTableName2, int PageSize, int PageIndex, string strFields, string sTable1Key, string sTable2Key, string OrderBy, string strWhere, string TablePrefix)
        //{
        //    sTableName1 = TablePrefix + sTableName1;
        //    sTableName2 = TablePrefix + sTableName2;
        //    string cmdText = string.Empty;
        //    string str2 = string.Empty;
        //    if (PageIndex > 0)
        //    {
        //        PageIndex--;
        //    }
        //    if (!string.IsNullOrEmpty(OrderBy))
        //    {
        //        str2 = " ORDER BY " + OrderBy;
        //    }
        //    int num = PageIndex * PageSize;
        //    object[] args = new object[] { sTableName1, sTable1Key, sTableName2, sTable2Key };
        //    string str3 = string.Format(" where {0}.{1}={2}.{3}", args);
        //    if (!string.IsNullOrEmpty(strWhere))
        //    {
        //        strWhere = str3 + " and " + strWhere;
        //    }
        //    else
        //    {
        //        strWhere = str3;
        //    }
        //    object[] objArray2 = new object[] { "select ", sTableName1, ".", sTable1Key, " from ", sTableName1, ",", sTableName2, " ", strWhere, str2, " limit ", num, ",", PageSize, ";" };
        //    cmdText = string.Concat(objArray2);
        //    StringBuilder builder = new StringBuilder();
        //    using (SQLiteDataReader reader = DB.ExecuteReader(1, cmdText))
        //    {
        //        while (reader.Read())
        //        {
        //            builder.Append(reader.GetString(0));
        //            builder.Append(",");
        //        }
        //    }
        //    if (builder.Length > 1)
        //    {
        //        builder.Remove(builder.Length - 1, 1);
        //    }
        //    else
        //    {
        //        builder.Append("0");
        //    }
        //    if (string.IsNullOrEmpty(strFields))
        //    {
        //        strFields = "*";
        //    }
        //    else
        //    {
        //        strFields = string.Format(strFields, sTableName1, sTableName2);
        //    }
        //    object[] objArray3 = new object[] { 
        //        "select ", strFields, " from ", sTableName1, ",", sTableName2, " where ", sTableName1, ".", sTable1Key, "=", sTableName2, ".", sTable2Key, " and ", sTableName1, 
        //        ".", sTable1Key, " in (", builder, ")", str2
        //     };
        //    return string.Concat(objArray3);
        //}

        private static string GetSplitPagesSqlPT(string sTableName, int PageSize, int PageIndex, string strFields, string KeyField, string OrderBy, string strWhere, string TablePrefix)
        {
            sTableName = TablePrefix + sTableName;
            int num = (PageIndex - 1) * PageSize;
            string str = "";
            string str2 = "";
            string str3 = string.Format(" order by {0} desc", KeyField);
            if (!string.IsNullOrEmpty(strWhere))
            {
                str = " WHERE " + strWhere;
                str2 = " AND " + strWhere;
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                str3 = " ORDER BY " + OrderBy;
            }
            if (string.IsNullOrEmpty(strFields))
            {
                strFields = " * ";
            }
            if (PageIndex <= 1)
            {
                object[] objArray1 = new object[] { PageSize, strFields, sTableName, str, str3 };
                return string.Format("SELECT TOP {0} {1} FROM {2} {3} {4}", objArray1);
            }
            object[] args = new object[] { PageSize, strFields, sTableName, KeyField, num, str, str3, str2 };
            return string.Format("select top {0} {1} from {2} where {3} not in(select top {4} {3} from {2} {5} {6} ) {7} {6} ", args);
        }
    }




}