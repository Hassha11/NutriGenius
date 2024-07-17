using System.Data.SqlClient;

namespace Nutrigenius
{
    public class SQL
    {
        public string getSingleData(string strSQL, SqlConnection Conn)
        {
            string s = "";
            SqlCommand cmdgetSingleData = new SqlCommand();
            cmdgetSingleData.CommandText = strSQL;
            cmdgetSingleData.Connection = Conn;

            object result = cmdgetSingleData.ExecuteScalar();
            //if (cmdgetSingleData.ExecuteScalar().Equals(DBNull.Value))
            if (result == null)
            { s = ""; }
            else
            { s = cmdgetSingleData.ExecuteScalar().ToString(); }
            return s;

        }
    }
}
