using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CAT_Library.SQL_functions;

namespace CAT_Library.ft_SQL
{
    /// <summary>
    /// Functions about SQL.
    /// CRUD Method.
    /// </summary>
    public class ft_Query
    {
        static string conStr = @"Data Source=EU1RAPW313;Initial Catalog=CATDepartment;Integrated Security=False;User ID=web_sql;Password=Ottobre1985";
        SqlConnection con = new SqlConnection(conStr);
        
        /// <summary>
        /// Function that will search for the URL based on OpCo and Production Type (PROD or NONPROD) to access the Lease page.
        /// </summary>
        /// <param name="OpCo">OpCo acronym.</param>
        /// <param name="Type">Production type.</param>
        /// <returns></returns>
        public string LeaserUrl(string OpCo, string Type)
        {
            string url = "";
            SqlCommand cmd = new SqlCommand("SearchURL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Opco", OpCo);
            cmd.Parameters.AddWithValue("@Type", Type);
            SQL_connection.Opencon(con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() && url == "")
            {
                url = reader["URL"] as string;
            }
            SQL_connection.Closecon(con);
            return url;
        }
    }
}
