using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;

namespace ADODataAccess
{
   public class OdbcDataAccessLayer
    {
        private readonly string connectionString;

        public OdbcDataAccessLayer(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        private OdbcConnection GetConnection()
        {
            OdbcConnection connection = new OdbcConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        protected void CloseConnection()
        {
            OdbcConnection connection = new OdbcConnection(this.connectionString);
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText)
        {
            OdbcCommand command = new OdbcCommand(commandText, connection as OdbcConnection);
            return command;
        }

        protected DataTable GetData(string commandText, List<DbParameter>? parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OdbcConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));

                    return dt;
                }
            }
            catch (Exception)
            {
                //LogException("Failed to GetData for " + procedureName, ex, parameters);
                throw;
            }
        }

        protected int ExecuteNonQuery(string commandText, List<DbParameter>? parameters)
        {
            int result = -1;
            try
            {
                using (OdbcConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
                throw;
            }
            return result;
        }

        protected object ExecuteScalar(string commandText, List<OdbcParameter>? parameters)
        {
            object? result = null;

            try
            {
                using (DbConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return result;
        }

        protected OdbcParameter GetParameter(string parameter, object value)
        {
            OdbcParameter parameterObject = new OdbcParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }

        protected DataTable GetDataByAdapter(string commandText, List<DbParameter>? parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                DbConnection connection = this.GetConnection();
                {
                    DbCommand cmd = this.GetCommand(connection, commandText);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    OdbcDataAdapter dr = new OdbcDataAdapter((OdbcCommand)cmd);
                    dr.Fill(dt);
                }
            }
            catch (Exception)
            {
                //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);
                throw;
            }

            return dt;
        }
    }
}
