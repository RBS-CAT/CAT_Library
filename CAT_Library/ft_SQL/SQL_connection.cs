using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace CAT_Library.SQL_functions
{
    /// <summary>Library for direct connection to a SQL server and performing queries.</summary>
    public static class SQL_connection
    {
        /// <summary>
        /// Check if the connection is opened, if not the connection will be opened.
        /// </summary>
        /// <param name="con">A connection to a SQL Server database.</param>
        /// <returns>Connection status.</returns>
        public static SqlConnection Opencon(SqlConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        /// <summary>
        /// Check if the connection is closed, if not the connection will be closed.
        /// </summary>
        /// <param name="con">A connection to a SQL Server database.</param>
        /// <returns>Connection status.</returns>
        public static SqlConnection Closecon(SqlConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return con;
        }
    }
}
