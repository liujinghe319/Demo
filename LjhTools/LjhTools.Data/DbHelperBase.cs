﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace LjhTools.Data
{
    public abstract class DbHelperBase
    {
        // Fields
        protected object lockHelper = new object();
        protected string m_connectionstring = null;
        private DbProviderFactory m_factory = null;
        private Hashtable m_paramcache = Hashtable.Synchronized(new Hashtable());
        protected IDbProvider m_provider = null;
        private static int m_querycount = 0;
        private static string m_querydetail = "";

        // Methods
        protected DbHelperBase()
        {
        }

        private void AssignParameterValues(DbParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters != null) && (dataRow != null))
            {
                int num = 0;
                foreach (DbParameter parameter in commandParameters)
                {
                    if ((parameter.ParameterName == null) || (parameter.ParameterName.Length <= 1))
                    {
                        throw new Exception(string.Format("请提供参数{0}一个有效的名称{1}.", num, parameter.ParameterName));
                    }
                    if (dataRow.Table.Columns.IndexOf(parameter.ParameterName.Substring(1)) != -1)
                    {
                        parameter.Value = dataRow[parameter.ParameterName.Substring(1)];
                    }
                    num++;
                }
            }
        }

        private void AssignParameterValues(DbParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters != null) && (parameterValues != null))
            {
                if (commandParameters.Length != parameterValues.Length)
                {
                    throw new ArgumentException("参数值个数与参数不匹配.");
                }
                int index = 0;
                int length = commandParameters.Length;
                while (index < length)
                {
                    if (parameterValues[index] is IDbDataParameter)
                    {
                        IDbDataParameter parameter = (IDbDataParameter)parameterValues[index];
                        if (parameter.Value == null)
                        {
                            commandParameters[index].Value = DBNull.Value;
                        }
                        else
                        {
                            commandParameters[index].Value = parameter.Value;
                        }
                    }
                    else if (parameterValues[index] == null)
                    {
                        commandParameters[index].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[index].Value = parameterValues[index];
                    }
                    index++;
                }
            }
        }

        private void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                foreach (DbParameter parameter in commandParameters)
                {
                    if (parameter != null)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }
        }

        public void CacheParameterSet(string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((commandText == null) || (commandText.Length == 0))
            {
                throw new ArgumentNullException("commandText");
            }
            string str = this.ConnectionString() + ":" + commandText;
            this.m_paramcache[str] = commandParameters;
        }

        private DbParameter[] CloneParameters(DbParameter[] originalParameters)
        {
            DbParameter[] parameterArray = new DbParameter[originalParameters.Length];
            int index = 0;
            int length = originalParameters.Length;
            while (index < length)
            {
                parameterArray[index] = (DbParameter)((ICloneable)originalParameters[index]).Clone();
                index++;
            }
            return parameterArray;
        }

        public abstract string ConnectionString();
        public DbCommand CreateCommand(DbConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            DbCommand command = this.Factory.CreateCommand();
            command.CommandText = spName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                for (int i = 0; i < sourceColumns.Length; i++)
                {
                    spParameterSet[i].SourceColumn = sourceColumns[i];
                }
                this.AttachParameters(command, spParameterSet);
            }
            return command;
        }

        private DbParameter[] DiscoverSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            this.Provider().DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }
            DbParameter[] array = new DbParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(array, 0);
            foreach (DbParameter parameter in array)
            {
                parameter.Value = DBNull.Value;
            }
            return array;
        }

        public void ExecuteCommandWithSplitter(string commandText)
        {
            this.ExecuteCommandWithSplitter(commandText, "\r\nGO\r\n");
        }

        public void ExecuteCommandWithSplitter(string commandText, string splitter)
        {
            int num2;
            int startIndex = 0;
        Label_0003:
            num2 = commandText.IndexOf(splitter, startIndex);
            int length = ((num2 > startIndex) ? num2 : commandText.Length) - startIndex;
            string str = commandText.Substring(startIndex, length);
            if (str.Trim().Length > 0)
            {
                try
                {
                    this.ExecuteNonQuery(CommandType.Text, str);
                }
                catch
                {
                }
            }
            if (num2 != -1)
            {
                startIndex = num2 + splitter.Length;
                if (startIndex < commandText.Length)
                {
                    goto Label_0003;
                }
            }
        }

        public DataSet ExecuteDataset(string commandText)
        {
            return this.ExecuteDataset(CommandType.Text, commandText, null);
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(commandType, commandText, null);
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                return this.ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(connection, commandType, commandText, null);
        }

        public DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return this.ExecuteDataset(transaction, commandType, commandText, null);
        }

        public DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        public DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                m_querycount++;
                command.Parameters.Clear();
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                return dataSet;
            }
        }

        public DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                command.Parameters.Clear();
                return dataSet;
            }
        }

        public DataSet ExecuteDataset(DataSet ds, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.m_connectionstring;
                connection.Open();
                return this.ExecuteDataset(ds, "TableName", connection, commandType, commandText, commandParameters);
            }
        }

        public DataSet ExecuteDataset(DataSet ds, string TableName, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.m_connectionstring;
                connection.Open();
                return this.ExecuteDataset(ds, TableName, connection, commandType, commandText, commandParameters);
            }
        }

        public DataSet ExecuteDataset(DataSet ds, string TableName, DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                adapter.Fill(ds, TableName);
                m_querycount++;
                command.Parameters.Clear();
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                return ds;
            }
        }

        public DataSet ExecuteDatasetTypedParams(string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteDataset(CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteDataset(CommandType.StoredProcedure, spName);
        }

        public DataSet ExecuteDatasetTypedParams(DbConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public DataSet ExecuteDatasetTypedParams(DbTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText, null);
        }

        public int ExecuteNonQuery(out int id, string commandText)
        {
            return this.ExecuteNonQuery(out id, CommandType.Text, commandText, (DbParameter[])null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(commandType, commandText, null);
        }

        public int ExecuteNonQuery(out int id, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(out id, commandType, commandText, (DbParameter[])null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                return this.ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public int ExecuteNonQuery(out int id, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                return this.ExecuteNonQuery(out id, connection, commandType, commandText, commandParameters);
            }
        }

        public int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return num;
        }

        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            if(command.Connection==null)
            {
                command.Connection = transaction.Connection;
            }
            this.PrepareCommand(command, command.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }

        public int ExecuteNonQueryWidthTransaction(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                DbTransaction trans = connection.BeginTransaction();
                int num=this.ExecuteNonQuery(trans, commandType, commandText, commandParameters);
                if (num <= 0)
                {
                    trans.Rollback();
                }
                trans.Commit();
                return num;
            }
        }
        public int ExecuteNonQuery(out int id, DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(out id, connection, commandType, commandText, null);
        }

        public int ExecuteNonQuery(out int id, DbTransaction transaction, CommandType commandType, string commandText)
        {
            return this.ExecuteNonQuery(out id, transaction, commandType, commandText, null);
        }

        public int ExecuteNonQuery(out int id, DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (this.Provider().GetLastIdSql().Trim() == "")
            {
                throw new ArgumentNullException("GetLastIdSql is \"\"");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = this.Provider().GetLastIdSql();
            DateTime now = DateTime.Now;
            id = int.Parse(command.ExecuteScalar().ToString());
            DateTime dtEnd = DateTime.Now;
            m_querydetail = m_querydetail + this.GetQueryDetail(command.CommandText, now, dtEnd, commandParameters);
            m_querycount++;
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return num;
        }

        public int ExecuteNonQuery(out int id, DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = this.Provider().GetLastIdSql();
            id = int.Parse(command.ExecuteScalar().ToString());
            return num;
        }

        public int ExecuteNonQueryTypedParams(string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteNonQuery(CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteNonQuery(CommandType.StoredProcedure, spName);
        }

        public int ExecuteNonQueryTypedParams(DbConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public int ExecuteNonQueryTypedParams(DbTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return this.ExecuteReader(commandType, commandText, null);
        }

        public DbDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                object[] objArray1 = new object[] { CommandType.StoredProcedure, spName, spParameterSet };
                return this.ExecuteReader(this.ConnectionString(), objArray1);
            }
            object[] objArray2 = new object[] { CommandType.StoredProcedure, spName };
            return this.ExecuteReader(this.ConnectionString(), objArray2);
        }

        public DbDataReader ExecuteReader(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            DbDataReader reader;
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            DbConnection connection = null;
            try
            {
                connection = this.Factory.CreateConnection();
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                reader = this.ExecuteReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.Internal);
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }
                throw;
            }
            return reader;
        }

        public DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteReader(connection, commandType, commandText, null);
        }

        public DbDataReader ExecuteReader(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return this.ExecuteReader(transaction, commandType, commandText, null);
        }

        public DbDataReader ExecuteReader(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        public DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            return this.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        private DbDataReader ExecuteReader(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, DbConnectionOwnership connectionOwnership)
        {
            DbDataReader reader2;
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            bool mustCloseConnection = false;
            DbCommand command = this.Factory.CreateCommand();
            try
            {
                DbDataReader reader;
                this.PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
                DateTime now = DateTime.Now;
                if (connectionOwnership == DbConnectionOwnership.External)
                {
                    reader = command.ExecuteReader();
                }
                else
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                DateTime dtEnd = DateTime.Now;
                m_querydetail = m_querydetail + this.GetQueryDetail(command.CommandText, now, dtEnd, commandParameters);
                m_querycount++;
                bool flag3 = true;
                foreach (DbParameter parameter in command.Parameters)
                {
                    if (parameter.Direction != ParameterDirection.Input)
                    {
                        flag3 = false;
                    }
                }
                if (flag3)
                {
                    command.Parameters.Clear();
                }
                reader2 = reader;
            }
            catch (Exception exception)
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }
                throw new Exception(exception.Message + ":" + exception.StackTrace);
            }
            return reader2;
        }

        public DbDataReader ExecuteReaderTypedParams(string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                object[] objArray1 = new object[] { CommandType.StoredProcedure, spName, spParameterSet };
                return this.ExecuteReader(this.ConnectionString(), objArray1);
            }
            object[] parameterValues = new object[] { CommandType.StoredProcedure, spName };
            return this.ExecuteReader(this.ConnectionString(), parameterValues);
        }

        public DbDataReader ExecuteReaderTypedParams(DbConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public DbDataReader ExecuteReaderTypedParams(DbTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return this.ExecuteScalar(commandType, commandText, null);
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                return this.ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        public object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText)
        {
            return this.ExecuteScalar(connection, commandType, commandText, null);
        }

        public object ExecuteScalar(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return this.ExecuteScalar(transaction, commandType, commandText, null);
        }

        public object ExecuteScalar(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                return this.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            object obj2 = command.ExecuteScalar();
            command.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }
            return obj2;
        }

        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            DateTime now = DateTime.Now;
            object obj2 = command.ExecuteScalar();
            DateTime dtEnd = DateTime.Now;
            m_querydetail = m_querydetail + this.GetQueryDetail(command.CommandText, now, dtEnd, commandParameters);
            m_querycount++;
            command.Parameters.Clear();
            return obj2;
        }

        public string ExecuteScalarToStr(CommandType commandType, string commandText)
        {
            object obj2 = this.ExecuteScalar(commandType, commandText);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public string ExecuteScalarToStr(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            object obj2 = this.ExecuteScalar(commandType, commandText, commandParameters);
            if (obj2 == null)
            {
                return "";
            }
            return obj2.ToString();
        }

        public object ExecuteScalarTypedParams(string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteScalar(CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteScalar(CommandType.StoredProcedure, spName);
        }

        public object ExecuteScalarTypedParams(DbConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public object ExecuteScalarTypedParams(DbTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((dataRow != null) && (dataRow.ItemArray.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, dataRow);
                return this.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
            }
            return this.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public bool Exists(string strSql)
        {
            int num;
            object objA = this.ExecuteScalar(CommandType.Text, strSql);
            if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(objA.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public bool Exists(string strSql, params DbParameter[] parameterValues)
        {
            int num;
            object objA = this.ExecuteScalar(CommandType.Text, strSql, parameterValues);
            if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(objA.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                this.FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        public void FillDataset(string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                this.FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        public void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                connection.Open();
                this.FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        public void FillDataset(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            this.FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        public void FillDataset(DbConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                this.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
            {
                this.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        public void FillDataset(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            this.FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        public void FillDataset(DbTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if ((transaction != null) && (transaction.Connection == null))
            {
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                DbParameter[] spParameterSet = this.GetSpParameterSet(transaction.Connection, spName);
                this.AssignParameterValues(spParameterSet, parameterValues);
                this.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
            }
            else
            {
                this.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        public void FillDataset(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            this.FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        public void FillDataset(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            this.FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        private void FillDataset(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            DbCommand command = this.Factory.CreateCommand();
            bool mustCloseConnection = false;
            this.PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                adapter.SelectCommand = command;
                if ((tableNames != null) && (tableNames.Length > 0))
                {
                    string sourceTable = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        if ((tableNames[i] == null) || (tableNames[i].Length == 0))
                        {
                            throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        }
                        adapter.TableMappings.Add(sourceTable, tableNames[i]);
                        sourceTable = sourceTable + ((i + 1)).ToString();
                    }
                }
                adapter.Fill(dataSet);
                command.Parameters.Clear();
            }
            if (mustCloseConnection)
            {
                connection.Close();
            }
        }

        public List<string> GetAllTableNames()
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.m_connectionstring;
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                for (int i = 0; i < schema.Rows.Count; i++)
                {
                    if (schema.Rows[i].ItemArray[3].ToString() == "BASE TABLE")
                    {
                        string item = schema.Rows[i][2].ToString();
                        if (!item.ToLower().Equals("dtproperties"))
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public DbParameter[] GetCachedParameterSet(string commandText)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((commandText == null) || (commandText.Length == 0))
            {
                throw new ArgumentNullException("commandText");
            }
            string str = this.ConnectionString() + ":" + commandText;
            DbParameter[] originalParameters = this.m_paramcache[str] as DbParameter[];
            if (originalParameters == null)
            {
                return null;
            }
            return this.CloneParameters(originalParameters);
        }

        public int GetMaxID(string FieldName, string TableName)
        {
            return this.GetMaxID(FieldName, TableName, "");
        }

        public int GetMaxID(string FieldName, string TableName, string sPre)
        {
            string[] textArray1 = new string[] { "select max(", FieldName, ") from ", sPre, TableName };
            string commandText = string.Concat(textArray1);
            object obj2 = this.ExecuteScalar(CommandType.Text, commandText);
            if ((obj2 == null) || string.IsNullOrEmpty(obj2.ToString()))
            {
                return 1;
            }
            return int.Parse(obj2.ToString());
        }

        private string GetQueryDetail(string commandText, DateTime dtStart, DateTime dtEnd, DbParameter[] cmdParams)
        {
            string str = "<tr style=\"background: rgb(255, 255, 255) none repeat scroll 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if ((cmdParams != null) && (cmdParams.Length > 0))
            {
                foreach (DbParameter parameter in cmdParams)
                {
                    if (parameter != null)
                    {
                        str2 = str2 + "<td>" + parameter.ParameterName + "</td>";
                        str3 = str3 + "<td>" + parameter.DbType.ToString() + "</td>";
                        if (!object.Equals(parameter.Value, null))
                        {
                            str4 = str4 + "<td>" + parameter.Value.ToString() + "</td>";
                        }
                    }
                }
                object[] args = new object[] { str, str2, str3, str4 };
                str5 = string.Format("<table width=\"100%\" cellspacing=\"1\" cellpadding=\"0\" style=\"background: rgb(255, 255, 255) none repeat scroll 0%; margin-top: 5px; font-size: 12px; display: block; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">{0}{1}</tr>{0}{2}</tr>{0}{3}</tr></table>", args);
            }
            return string.Format("<center><div style=\"border: 1px solid black; margin: 2px; padding: 1em; text-align: left; width: 96%; clear: both;\"><div style=\"font-size: 12px; float: right; width: 100px; margin-bottom: 5px;\"><b>TIME:</b> {0}</div><span style=\"font-size: 12px;\">{1}{2}</span></div><br /></center>", dtEnd.Subtract(dtStart).TotalMilliseconds / 1000.0, commandText, str5);
        }

        public DbParameter[] GetSpParameterSet(string spName)
        {
            return this.GetSpParameterSet(spName, false);
        }

        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName)
        {
            return this.GetSpParameterSet(connection, spName, false);
        }

        public DbParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter)
        {
            if (string.IsNullOrEmpty(this.ConnectionString()))
            {
                throw new ArgumentNullException("ConnectionString()");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            using (DbConnection connection = this.Factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString();
                return this.GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            using (DbConnection connection2 = (DbConnection)((ICloneable)connection).Clone())
            {
                return this.GetSpParameterSetInternal(connection2, spName, includeReturnValueParameter);
            }
        }

        private DbParameter[] GetSpParameterSetInternal(DbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if ((spName == null) || (spName.Length == 0))
            {
                throw new ArgumentNullException("spName");
            }
            string str = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            DbParameter[] originalParameters = this.m_paramcache[str] as DbParameter[];
            if (originalParameters == null)
            {
                DbParameter[] parameterArray2 = this.DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                this.m_paramcache[str] = parameterArray2;
                originalParameters = parameterArray2;
            }
            return this.CloneParameters(originalParameters);
        }

        public DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value)
        {
            return this.MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public DbParameter MakeOutParam(string ParamName, DbType DbType, int Size)
        {
            return this.MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, int Size, ParameterDirection Direction, object Value)
        {
            DbParameter parameter = this.Provider().MakeParam(ParamName, DbType, Size);
            parameter.Direction = Direction;
            if ((Direction != ParameterDirection.Output) || (Value != null))
            {
                parameter.Value = Value;
            }
            return parameter;
        }

        private void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if ((commandText == null) || (commandText.Length == 0))
            {
                throw new ArgumentNullException("commandText");
            }
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                }
                command.Transaction = transaction;
            }            
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                this.AttachParameters(command, commandParameters);
            }
        }

        public abstract IDbProvider Provider();
        public void ResetDbProvider()
        {
            this.m_connectionstring = null;
            this.m_factory = null;
            this.m_provider = null;
        }

        public void UpdateDataset(DbCommand insertCommand, DbCommand deleteCommand, DbCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null)
            {
                throw new ArgumentNullException("insertCommand");
            }
            if (deleteCommand == null)
            {
                throw new ArgumentNullException("deleteCommand");
            }
            if (updateCommand == null)
            {
                throw new ArgumentNullException("updateCommand");
            }
            if ((tableName == null) || (tableName.Length == 0))
            {
                throw new ArgumentNullException("tableName");
            }
            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                adapter.UpdateCommand = updateCommand;
                adapter.InsertCommand = insertCommand;
                adapter.DeleteCommand = deleteCommand;
                adapter.Update(dataSet, tableName);
                dataSet.AcceptChanges();
            }
        }

        // Properties
        public DbProviderFactory Factory
        {
            get
            {
                if (this.m_factory == null)
                {
                    this.m_factory = this.Provider().Instance();
                }
                return this.m_factory;
            }
        }

        public static int QueryCount
        {
            get
            {
                return m_querycount;
            }
            set
            {
                m_querycount = value;
            }
        }

        public static string QueryDetail
        {
            get
            {
                return m_querydetail;
            }
            set
            {
                m_querydetail = value;
            }
        }

        // Nested Types
        private enum DbConnectionOwnership
        {
            Internal,
            External
        }
    }
}