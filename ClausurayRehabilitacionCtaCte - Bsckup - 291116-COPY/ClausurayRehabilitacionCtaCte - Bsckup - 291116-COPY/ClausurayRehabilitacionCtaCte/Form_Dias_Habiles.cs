using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;


namespace ClausurayRehabilitacionCtaCte
{
    public partial class Form_dias : Form
    {
        public Form_dias()
        {
            InitializeComponent();
            cargar_dias();
        }

        private static Form_dias m_FormDefInstance;
        public static Form_dias DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_dias();
                else
                    m_FormDefInstance.BringToFront();

                return m_FormDefInstance;
            }
            set
            {
                m_FormDefInstance = value;
            }
        }

        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);
        Ingresos ingreso = new Ingresos();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cargar_dias()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_cargar_dias", cone);
                cmd.CommandType = CommandType.StoredProcedure;

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt.Rows[0][0].ToString() == "0")
                    cb1.Checked = true;
                else
                    cb1.Checked = false;

                if (dt.Rows[1][0].ToString() == "0")
                    cb2.Checked = true;
                else
                    cb2.Checked = false;
                if (dt.Rows[2][0].ToString() == "0")
                    cb3.Checked = true;
                else
                    cb3.Checked = false;
                if (dt.Rows[3][0].ToString() == "0")
                    cb4.Checked = true;
                else
                    cb4.Checked = false;
                if (dt.Rows[4][0].ToString() == "0")
                    cb5.Checked = true;
                else
                    cb5.Checked = false;
                if (dt.Rows[5][0].ToString() == "0")
                    cb6.Checked = true;
                else
                    cb6.Checked = false;
                if (dt.Rows[6][0].ToString() == "0")
                    cb7.Checked = true;
                else
                    cb7.Checked = false;             


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error al cargar los dias laborales.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            dias_modificados();
            this.Close();
        }

        private void dias_modificados()
        {
            string dias="";
            if (cb1.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb2.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb3.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb4.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb5.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb6.Checked)
                dias += "0";
            else
                dias += "1";
            if (cb7.Checked)
                dias += "0";
            else
                dias += "1";

            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_dias", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter diasstr = new SqlParameter("@dias", SqlDbType.VarChar);
                diasstr.Direction = ParameterDirection.Input;
                diasstr.Value = dias;
                cmd.Parameters.Add(diasstr);

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GDH-00",Program.usuario,0);

                MessageBox.Show("Se modficicaron los dias exitosamente.");

            }
            catch (SqlException ex)
            {
                ingreso.logs("GDH-01", Program.usuario, 1);
                MessageBox.Show("Error al actualizar los días laborales.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

    }
}
