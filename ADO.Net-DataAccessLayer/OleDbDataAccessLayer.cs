using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace ADODataAccess
{
    public class OleDbDataAccessLayer
    {
        private readonly string connectionString;

        public OleDbDataAccessLayer(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        private OleDbConnection GetConnection()
        {
            OleDbConnection connection = new OleDbConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        protected void CloseConnection()
        {
            OleDbConnection connection = new OleDbConnection(this.connectionString);
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText)
        {
            OleDbCommand command = new OleDbCommand(commandText, connection as OleDbConnection);
            return command;
        }

        protected DataTable GetData(string commandText, List<DbParameter>? parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection con = this.GetConnection())
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
                using (OleDbConnection con = this.GetConnection())
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

        protected object ExecuteScalar(string commandText, List<OleDbParameter>? parameters)
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

        protected OleDbParameter GetParameter(string parameter, object value)
        {
            OleDbParameter parameterObject = new OleDbParameter(parameter, value != null ? value : DBNull.Value);
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
                    OleDbDataAdapter dr = new OleDbDataAdapter((OleDbCommand)cmd);
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