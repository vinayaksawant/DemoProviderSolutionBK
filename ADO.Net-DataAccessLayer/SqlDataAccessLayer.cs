using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ADODataAccess
{
    public class SqlDataAccessLayer
    {
        private readonly string connectionString;

        public SqlDataAccessLayer(string _connectionString)
        {
            this.connectionString = _connectionString;
        }
        /// <summary>
        /// GetConnection creates a new connection of SqlConnection type and return it after opening.
        /// </summary>
        /// <returns>SqlConnection</returns>
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        public void CloseConnection()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        /// <summary>
        /// GetCommand creates a new command of SqlCommand according to specified parameters.
        /// </summary>
        public DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }
        /// <summary>
        ///ExecuteReader initializes connection, command and executes ExecuteReader method of command object.
        ///Provides a way of reading a forward-only stream of rows from a SQL Server database.
        ///We have explicitly omitted using block for connection as we need to return DataTable with open connection state.
        ///Now question raises that how will we handle connection close open,
        ///for this we have created DataReader with "CommandBehavior.CloseConnection", which means, connection will be closed as related DataReader is closed.
        ///Please refer to MSDN for more details about SqlCommand.ExecuteReader and DataTable
        /// </summary>
        public DataTable GetData(string commandText, List<DbParameter>? parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    if (cmd.CommandType == CommandType.Text == true)
                    {
                        dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                    }

                    if (cmd.CommandType == CommandType.StoredProcedure == true)
                    {
                        DbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        dt.Load(dr);
                    }
                    return dt;
                }
            }
            catch (Exception)
            {
                //LogException("Failed to GetData for " + procedureName, ex, parameters);
                throw;
            }
        }
        /// <summary>
        /// ExecuteNonQuery initializes connection,
        /// command and executes ExecuteNonQuery method of command object.
        /// Although the ExecuteNonQuery returns no rows, any output parameters or return values mapped to parameters are populated with data.
        /// For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
        /// Please refer to MSDN for more details about SqlCommand.ExecuteNonQuery.
        /// </summary>
        public int ExecuteNonQuery(string commandText, List<DbParameter>? parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int result = -1;
            try
            {
                using (SqlConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText, commandType);
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
        /// <summary>
        /// ExecuteScalar initializes connection, command and executes  ExecuteScalar method of command object.
        /// Executes the query, and returns the first column of the first row in the result set returned by the query.
        /// Additional columns or rows are ignored.  Please refer to MSDN for more details about SqlCommand.ExecuteScalar.
        /// </summary>
        public object ExecuteScalar(string commandText, List<SqlParameter>? parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            object? result = null;

            try
            {
                using (DbConnection con = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(con, commandText, commandType);

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
        /// <summary>
        /// GetParameter creates a new parameter of SqlParameter and initialize it with provided value.
        /// </summary>
        public SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }
        /// <summary>
        /// GetParameterOut creates a new parameter of SqlParameter type with parameter direct set to Output type.
        /// </summary>
        public SqlParameter GetParameterOut(string parameter, SqlDbType type, object? value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }
        /// <summary>
        ///ExecuteReader initializes connection, command and executes GetDataByAdapter method of command object.
        ///Provides a way of reading a forward-only stream of rows from a SQL Server database.
        ///We have explicitly omitted using block for connection as we need to return DataTable with open connection state.
        ///Now question raises that how will we handle connection close open,
        ///for this we have created SqlDataAdapter.
        ///Please refer to MSDN for more details about SqlCommand.ExecuteReader and SqlDataAdapter
        /// </summary>
        public DataTable GetDataByAdapter(string commandText, List<DbParameter>? parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DataTable dt = new DataTable();
            try
            {
                DbConnection connection = this.GetConnection();
                {
                    DbCommand cmd = this.GetCommand(connection, commandText, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    SqlDataAdapter dr = new SqlDataAdapter((SqlCommand)cmd);
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