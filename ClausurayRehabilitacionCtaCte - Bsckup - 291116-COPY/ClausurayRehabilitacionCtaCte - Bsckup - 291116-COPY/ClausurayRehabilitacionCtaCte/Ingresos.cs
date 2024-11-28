using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ClausurayRehabilitacionCtaCte
{
    class Ingresos
    {
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);

        public void logs(string cod, string usr, int exito)
        {

            SqlCommand cmin = new SqlCommand("sp_ingresos", cone);            
            cmin.CommandType = CommandType.StoredProcedure;
            cmin.Parameters.AddWithValue("cod", cod);
            cmin.Parameters.AddWithValue("usr", usr);
            cmin.Parameters.AddWithValue("exito", exito);
            try
            {
                cone.Open();
                cmin.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBoxTemporal.Show("asda\n"+ex.Message, "123", 12, true);
            }
            finally
            {
                cone.Close();
            }
        }
    }
}
