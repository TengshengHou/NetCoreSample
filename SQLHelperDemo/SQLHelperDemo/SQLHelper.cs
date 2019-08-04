using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SQLHelperDemo
{

    public class SQLHelper
    {
        IConfiguration _configuration;
        private string _connectionString;
        public SQLHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public int ExecuteNonQuery(SqlConnection conn, string cmdText, SqlParameter[] cmdParms, SqlTransaction trans)

        {
            return ExecuteNonQuery(conn, CommandType.Text, cmdText, cmdParms, trans);
        }

        public int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, SqlTransaction trans)
        {
            SqlCommand cmd = conn.CreateCommand();
            int val = 0;
            using (cmd)
            {
                PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParms);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            return val;
        }

        public SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return rdr;
        }


        public DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            System.Data.DataTable dt = new DataTable();
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        #region private
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            //cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(cmd, commandParameters);
            }
        }
        private void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }
        #endregion

        #region  Default connection
        public DataTable ExecuteDataTable(string cmdText, SqlParameter[] cmdParms)
        {
            DataTable dataTable;
            using (var sqlConnection = GetDefaultSqlConnection())
            {
                dataTable = this.ExecuteDataTable(sqlConnection, CommandType.Text, cmdText, cmdParms);
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string cmdText, SqlParameter[] cmdParms)
        {
            int rowsAffected;
            using (var sqlConnection = GetDefaultSqlConnection())
            {
                rowsAffected = ExecuteNonQuery(sqlConnection, cmdText, cmdParms, null);
            }
            return rowsAffected;
        }

        public object ExecuteScalar(string cmdText, SqlParameter[] cmdParms)
        {
            object retObj;
            using (var sqlConnection = GetDefaultSqlConnection())
            {

                retObj = ExecuteScalar(cmdText, cmdParms);
            }
            return retObj;
        }
        #endregion

        public SqlConnection GetDefaultSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }

}
