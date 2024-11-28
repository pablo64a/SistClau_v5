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
    public partial class Form_Habilitacion_Reproceso : Form
    {
        public Form_Habilitacion_Reproceso()
        {
            InitializeComponent();        
        }

        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);
        Ingresos ingreso = new Ingresos();

        private static Form_Habilitacion_Reproceso m_FormDefInstance;
        public static Form_Habilitacion_Reproceso DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Habilitacion_Reproceso();
                else
                    m_FormDefInstance.BringToFront();

                return m_FormDefInstance;
            }
            set
            {
                m_FormDefInstance = value;
            }
        }

        public string fecha="";

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fecha == "")
                MessageBox.Show("Debe seleccionar una fecha para la ejecucion del reproceso", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                if (!cbcc.Checked && !cbct.Checked && !cbpc.Checked && !cbpr.Checked)
                    MessageBox.Show("Debe seleccionar por lo menos una actividad para que se realice el reproceso", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    if (DateTime.Parse(fecha) <= DateTime.Now)
                    {
                        DialogResult result = MessageBox.Show("Seguro que quiere adicionar la fecha " + fecha + " como fecha de reproceso para las opciones seleccionadas?", "Salir", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            ingresar_reproceso(fecha);
                        }
                        else
                            MessageBox.Show("No se adicionó el dia seleccionado.");
                    }
                    else
                    {
                        MessageBox.Show("La fecha de reproceso no debe ser mayor a la actual.");
                    }
                }
            }
        }

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
             fecha = e.Start.ToString("yyyy-MM-dd");  
        }


        private string codigo()
        {
            string cod = "";
            if (cbcc.Checked)
                cod += "0";
            else
                cod += "1";
            if (cbct.Checked)
                cod += "0";
            else
                cod += "1";
            if (cbpc.Checked)
                cod += "0";
            else
                cod += "1";
            if (cbpr.Checked)
                cod += "0";
            else
                cod += "1";
            return cod;
        }

        private void ingresar_reproceso(string f)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ingresar_reproceso", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter fe = new SqlParameter("@fecha", SqlDbType.VarChar, 10);
                fe.Direction = ParameterDirection.Input;
                fe.Value = f;
                cmd.Parameters.Add(fe);
                SqlParameter cd = new SqlParameter("@codigo", SqlDbType.VarChar, 5);
                cd.Direction = ParameterDirection.Input;
                cd.Value = codigo();
                //MessageBox.Show(codigo());
                cmd.Parameters.Add(cd);

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("HRP-00", Program.usuario, 0);

                MessageBox.Show("Se habilitó la fecha " + f + " para que se realice el reproceso.");
            }
            catch (SqlException ex)
            {
                ingreso.logs("HRP-01", Program.usuario, 1);
                MessageBox.Show("Se produjo un error al habilitar la fecha para reproceso.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }
    }
}
