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
    public partial class Form_Gestion_Feriados : Form
    {
        public Form_Gestion_Feriados()
        {
            InitializeComponent();
        }

        private static Form_Gestion_Feriados m_FormDefInstance;
        public static Form_Gestion_Feriados DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Gestion_Feriados();
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

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
           // textBox1.Text = e.Start.ToString("yyyy-MM-dd");
        }

        private void calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            lab_resp.Text = "";
            if (op == 2)
            {
                textBox1.Text = e.Start.ToString("yyyy-MM-dd"); 
            }
            else
                lab_feriado.Text = e.Start.ToString("yyyy-MM-dd");            
        }

        private void ingresar_dia(string dia)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ingresar_feriado", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter diaf = new SqlParameter("@dia", SqlDbType.VarChar,10);
                diaf.Direction = ParameterDirection.Input;
                diaf.Value = dia;                
                cmd.Parameters.Add(diaf);                

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GFE-10", Program.usuario, 0);

                lab_resp.Text = "Se adicionó la fecha " + dia + " como dia feriado.";

                lab_feriado.Text = "";
            }
            catch (SqlException ex)
            {
                ingreso.logs("GFE-11", Program.usuario, 1);
                if (ex.Number == 2627)
                    MessageBox.Show("La fecha ya se encuentra dentro de la tabla de feriados.");
                else
                    MessageBox.Show("Se produjo un error al ingresar la fecha.\n" + ex.Message);                 
            }
            finally
            {
                cone.Close();
            }
        }

        public int op;

        private void button1_Click(object sender, EventArgs e)
        {
            lab_resp.Text = "";
            if (lab_feriado.Text != "")
            {
                if (op == 1)
                {
                    DialogResult result = MessageBox.Show("Seguro que quiere adicionar la fecha " + lab_feriado.Text + " a la lista de feriados?", "Salir", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        ingresar_dia(lab_feriado.Text);
                    }
                    else
                        MessageBox.Show("No se adicionó el dia seleccionado.");
                }
                else
                {
                    if (op == 2)
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Seleccione una fecha del calendario por la cual se modificará la fecha seleccionada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Seguro que quiere modificar la fecha " + lab_feriado.Text + " por la fecha " + textBox1.Text + "?", "Salir", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                modificar_dia(lab_feriado.Text, textBox1.Text);
                            }
                            else
                                MessageBox.Show("No se modificó el dia seleccionado.");
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Seguro que quiere eliminar la fecha " + lab_feriado.Text + " de la lista de feriados?", "Salir", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            eliminar_dia(lab_feriado.Text);
                        }
                        else
                            MessageBox.Show("No se eliminó el dia seleccionado.");
                    }
                }
                
            }
            else
                MessageBox.Show("Seleccione una fecha.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            cargar();
        }

        private void modificar_dia(string dia, string dian)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_feriado", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter diaf = new SqlParameter("@dia", SqlDbType.VarChar, 10);
                diaf.Direction = ParameterDirection.Input;
                diaf.Value = dia;
                cmd.Parameters.Add(diaf);
                SqlParameter diaf2 = new SqlParameter("@dian", SqlDbType.VarChar, 10);
                diaf2.Direction = ParameterDirection.Input;
                diaf2.Value = dian;
                cmd.Parameters.Add(diaf2);

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GFE-30", Program.usuario, 0);

                lab_resp.Text = "Se modificó la fecha " + dia + " por la fecha "+dian;

                lab_feriado.Text = "";
            }
            catch (SqlException ex)
            {
                ingreso.logs("GUS-31", Program.usuario, 1       );
                if (ex.Number == 2627)
                    MessageBox.Show("La fecha ya se encuentra dentro de la tabla de feriados.");
                else
                    MessageBox.Show("Se produjo un error al actualizar los datos.\n" + ex.Message);           
            }
            finally
            {
                cone.Close();
            }
        }

        private void eliminar_dia(string dia)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_eliminar_feriado", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter diaf = new SqlParameter("@dia", SqlDbType.VarChar, 10);
                diaf.Direction = ParameterDirection.Input;
                diaf.Value = dia;
                cmd.Parameters.Add(diaf);                

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GFE-20", Program.usuario, 0);

                lab_resp.Text = "Se eliminó la fecha " + dia + " de la lista de feriados.";

                lab_feriado.Text = "";
            }
            catch (SqlException ex)
            {
                ingreso.logs("GFE-21", Program.usuario, 1);
                MessageBox.Show("Se produjo un error al eliminar la fecha.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void Form_feriados_Load(object sender, EventArgs e)
        {
            cargar();
            rb1.Checked = true;
        }

        public void cargar()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_cargar_feriados", cone);
                cmd.CommandType = CommandType.StoredProcedure;

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                dataGridView1.DataSource = dt;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error al cargar los días feriados.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void selectedCellsButton_Click(object sender, System.EventArgs e)
        {            
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string val = dataGridView1.CurrentCell.Value.ToString();
            val = val.Substring(6, 4) + "-" + val.Substring(3, 2) + "-" + val.Substring(0, 2);
            lab_feriado.Text = val;            
        }        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Selecione la fecha del calendario que se quiere añadir como feriado";
            label2.Visible = false;
            textBox1.Visible = false;
            button1.Text = "Añadir";
            op = 1;
            lab_resp.Text = "";
            calendar.Visible = true;
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Seleccione la fecha de la lista de feriados y luego selecione la nueva fecha";
            label2.Visible = true;
            textBox1.Visible = true;
            textBox1.Text = "";
            button1.Text = "Modificar";
            op = 2;
            lab_resp.Text = "";
            calendar.Visible = true;
        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Seleccione la fecha de la lista de feriados que desea eliminar";
            label2.Visible = false;
            textBox1.Visible = false;
            button1.Text = "Eliminar";
            op = 3;
            lab_resp.Text = "";
            calendar.Visible = false;
        }

        /*private void label1_Click(object sender, EventArgs e)
        {

        }*/

    }
}
