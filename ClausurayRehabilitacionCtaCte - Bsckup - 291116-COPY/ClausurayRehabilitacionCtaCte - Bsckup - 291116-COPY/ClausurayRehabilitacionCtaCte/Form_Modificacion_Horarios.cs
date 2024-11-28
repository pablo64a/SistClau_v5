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
    public partial class Form_Modificacion_Horarios : Form
    {
        public Form_Modificacion_Horarios()
        {
            InitializeComponent();
            cargar_dias();
        }
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);
        Ingresos ingreso = new Ingresos();

        private static Form_Modificacion_Horarios m_FormDefInstance;
        public static Form_Modificacion_Horarios DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Modificacion_Horarios();
                else
                    m_FormDefInstance.BringToFront();

                return m_FormDefInstance;
            }
            set
            {
                m_FormDefInstance = value;
            }
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
                    radioButton1.Enabled = true;
                else
                    radioButton1.Enabled = false;
                if (dt.Rows[1][0].ToString() == "0")
                    radioButton2.Enabled = true;
                else
                    radioButton2.Enabled = false;
                if (dt.Rows[2][0].ToString() == "0")
                    radioButton3.Enabled = true;
                else
                    radioButton3.Enabled = false;
                if (dt.Rows[3][0].ToString() == "0")
                    radioButton4.Enabled = true;
                else
                    radioButton4.Enabled = false;
                if (dt.Rows[4][0].ToString() == "0")
                    radioButton5.Enabled = true;
                else
                    radioButton5.Enabled = false;
                if (dt.Rows[5][0].ToString() == "0")
                    radioButton6.Enabled = true;
                else
                    radioButton6.Enabled = false;
                if (dt.Rows[6][0].ToString() == "0")
                    radioButton7.Enabled = true;
                else
                    radioButton7.Enabled = false;


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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            t1.MaxLength = 2;
            t2.MaxLength = 2;
            t3.MaxLength = 2;
            t4.MaxLength = 2;
            t6.MaxLength = 2;
            t7.MaxLength = 2;
            t8.MaxLength = 2;
            t9.MaxLength = 2;
            t11.MaxLength = 2;
            t12.MaxLength = 2;
            t13.MaxLength = 2;
            t14.MaxLength = 2;
            t15.MaxLength = 2;
            t16.MaxLength = 2;

        }



        private void t1_TextChanged(object sender, EventArgs e)
        {
       
        }

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }





        private void t5_DragLeave(object sender, EventArgs e)
        {

        }

        public string hora(string a, string b)
        {
            string horafinal = a +":"+ b + ":00";
            return horafinal;
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Seguro que quiere modificar la tabla de horarios?", "Salir", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_modificar_horarios", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    SqlParameter h11 = new SqlParameter("@h11", SqlDbType.VarChar);
                    h11.Direction = ParameterDirection.Input;
                    h11.Value = hora(t1.Text,t2.Text);
                    cmd.Parameters.Add(h11);
                    SqlParameter h12 = new SqlParameter("@h12", SqlDbType.VarChar);
                    h12.Direction = ParameterDirection.Input;
                    h12.Value = hora(t3.Text, t4.Text);
                    cmd.Parameters.Add(h12);
                    SqlParameter r1 = new SqlParameter("@r1", SqlDbType.Int);
                    r1.Direction = ParameterDirection.Input;
                    r1.Value = t5.Text;
                    cmd.Parameters.Add(r1);
                    SqlParameter h21 = new SqlParameter("@h21", SqlDbType.VarChar);
                    h21.Direction = ParameterDirection.Input;
                    h21.Value = hora(t6.Text, t7.Text);
                    cmd.Parameters.Add(h21);
                    SqlParameter h22 = new SqlParameter("@h22", SqlDbType.VarChar);
                    h22.Direction = ParameterDirection.Input;
                    h22.Value = hora(t8.Text, t9.Text);
                    cmd.Parameters.Add(h22);
                    SqlParameter r2 = new SqlParameter("@r2", SqlDbType.Int);
                    r2.Direction = ParameterDirection.Input;
                    r2.Value = t10.Text;
                    cmd.Parameters.Add(r2);
                    SqlParameter h31 = new SqlParameter("@h31", SqlDbType.VarChar);
                    h31.Direction = ParameterDirection.Input;
                    h31.Value = hora(t11.Text, t12.Text);
                    cmd.Parameters.Add(h31);
                    SqlParameter h32 = new SqlParameter("@h32", SqlDbType.VarChar);
                    h32.Direction = ParameterDirection.Input;
                    h32.Value = hora(t13.Text, t14.Text);
                    cmd.Parameters.Add(h32);
                    SqlParameter h33 = new SqlParameter("@h33", SqlDbType.VarChar);
                    h33.Direction = ParameterDirection.Input;
                    h33.Value = hora(t15.Text, t16.Text);
                    cmd.Parameters.Add(h33);
                    SqlParameter id = new SqlParameter("@id_dia", SqlDbType.Int);
                    id.Direction = ParameterDirection.Input;
                    id.Value = id_dia;
                    cmd.Parameters.Add(id);

                    cone.Open();
                    cmd.ExecuteNonQuery();

                    ingreso.logs("GHO-00",Program.usuario,0);

                    MessageBox.Show("Se modifico exitosamente los horarios del día " + id_dia.ToString());
                }
                catch (SqlException ei)
                {
                    ingreso.logs("GHO-01", Program.usuario, 1);
                    switch (ei.Number)
                    {
                        case 241:
                            MessageBox.Show("Debe introducir una hora valida. Descripción del error: " + ei.Message + "......");
                            break;
                        default:
                            MessageBox.Show("Se produjo un error al actualizar la tabla de horarios. Descripción: " + ei.Message);
                            break;
                    }
                }
                finally
                {
                    cone.Close();
                }
            }
            else
                MessageBox.Show("No se modificó la tabla de horarios.");
        }       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form_dias.DefInstance.Show();
            button3.Visible = true;
        }

        private void cargar_horas(int a)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_cargar_horas_dia", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter id = new SqlParameter("@id", SqlDbType.VarChar);
                id.Direction = ParameterDirection.Input;
                id.Value = a;
                cmd.Parameters.Add(id);

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);                

                if (dt.Rows[0][0].ToString() != null)
                {
                    string h1 = dt.Rows[0][0].ToString();
                    t1.Text = h1.Substring(0, 2);
                    t2.Text = h1.Substring(3, 2);
                }
                else
                {
                    t1.Text = "";
                    t2.Text = "";
                }

                if (dt.Rows[0][1].ToString() != null)
                {
                    string h2 = dt.Rows[0][1].ToString();
                    t3.Text = h2.Substring(0, 2);
                    t4.Text = h2.Substring(3, 2);
                }
                else
                {
                    t3.Text = "";
                    t4.Text = "";
                }

                if (dt.Rows[0][3].ToString() != null)
                {
                    string r = dt.Rows[0][3].ToString();
                    t5.Text = r;
                }
                else
                {
                    t5.Text = "";
                }               

                if (dt.Rows[1][0].ToString() != null)
                {
                    string h1 = dt.Rows[1][0].ToString();
                    t6.Text = h1.Substring(0, 2);
                    t7.Text = h1.Substring(3, 2);                    
                }
                else
                {
                    t6.Text = "";
                    t7.Text = "";
                }

                if (dt.Rows[1][1].ToString() != null)
                {
                    string h2 = dt.Rows[1][1].ToString();
                    t8.Text = h2.Substring(0, 2);
                    t9.Text = h2.Substring(3, 2);
                }
                else
                {
                    t8.Text = "";
                    t9.Text = "";
                }

                if (dt.Rows[1][3].ToString() != null)
                {
                    string r = dt.Rows[1][3].ToString();
                    t10.Text = r;               
                }
                else
                {
                    t10.Text = "";
                }

                if (dt.Rows[2][0].ToString() != null)
                {
                    string h1 = dt.Rows[2][0].ToString();
                    t11.Text = h1.Substring(0, 2);
                    t12.Text = h1.Substring(3, 2);
                }
                else
                {
                    t11.Text = "";
                    t12.Text = "";
                }

                if (dt.Rows[2][1].ToString() != null)
                {
                    string h2 = dt.Rows[2][1].ToString();
                    t13.Text = h2.Substring(0, 2);
                    t14.Text = h2.Substring(3, 2);
                }
                else
                {
                    t13.Text = "";
                    t14.Text = "";
                }

                if (dt.Rows[2][0].ToString() != null)
                {
                    string h3 = dt.Rows[2][2].ToString();
                    t15.Text = h3.Substring(0, 2);
                    t16.Text = h3.Substring(3, 2);
                }
                else
                {
                    t15.Text = "";
                    t16.Text = "";
                }

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

        public int id_dia;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 1;
            if (radioButton1.Checked)
                cargar_horas(1);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 6;
            if (radioButton6.Checked)
                cargar_horas(6);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 2;
            if (radioButton2.Checked)
                cargar_horas(2);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 3;
            if (radioButton3.Checked)
                cargar_horas(3);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 4;
            if (radioButton4.Checked)
                cargar_horas(4);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 5;
            if (radioButton5.Checked)
                cargar_horas(5);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            id_dia = 7;
            if (radioButton7.Checked)
                cargar_horas(7);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cargar_dias();
            button3.Visible = false;
        }

        private void t3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }        

        private void t4_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void t16_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void Form_Modificacion_Horarios_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }



    }
}
