using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ClausurayRehabilitacionCtaCte
{
    public partial class Form_Cambiar_Clave : Form
    {
        public Form_Cambiar_Clave()
        {
            InitializeComponent();
            textBox5.MaxLength = 50;
            textBox5.PasswordChar = '*';
            textBox1.MaxLength = 50;
            textBox1.PasswordChar = '*';
            textBox2.MaxLength = 50;
            textBox2.PasswordChar = '*';
        }

        private static Form_Cambiar_Clave m_FormDefInstance;
        public static Form_Cambiar_Clave DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Cambiar_Clave();
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
        public string usr = Program.usuario;

        public Boolean valida_pass(string pass)
        {
            Regex regex = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

            if (regex.IsMatch(pass))
                return true;
            else
                return false;
        }

        private void Form_Cambiar_Clave_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void cambiar_pass()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_cambiar_clave", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter usuario = new SqlParameter("@usuario", SqlDbType.VarChar);
                usuario.Direction = ParameterDirection.Input;
                usuario.Value = usr;
                cmd.Parameters.Add(usuario);
                SqlParameter clave = new SqlParameter("@clave", SqlDbType.VarChar);
                clave.Direction = ParameterDirection.Input;
                clave.Value = textBox5.Text;
                cmd.Parameters.Add(clave);
                SqlParameter nclave = new SqlParameter("@clave2", SqlDbType.VarChar);
                nclave.Direction = ParameterDirection.Input;
                nclave.Value = textBox1.Text;
                cmd.Parameters.Add(nclave);
                SqlParameter resp = new SqlParameter("@res", SqlDbType.Int);
                resp.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resp);

                cone.Open();
                cmd.ExecuteNonQuery();

                int res = (Int32)cmd.Parameters["@res"].Value;               

                if (res == 1)
                {
                    MessageBox.Show("Contraseña antigua incorrecta.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    ingreso.logs("PSW-01", usr, 1);
                    textBox5.Focus();
                }
                else
                {
                    MessageBox.Show("Contraseña cambiada.");
                    ingreso.logs("PSW-00", usr, 0);
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                ingreso.logs("PSW-01", usr, 1);
                MessageBox.Show("Error al intentar cambiar la contraseña del usuario.\n\n"+ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                cone.Close();
            }
        }

        private void btndinam_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")            
                MessageBox.Show("Ingrese la antigua contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);            
            else
            {
                if (valida_pass(textBox1.Text))
                {
                    if (textBox1.Text.Equals(textBox2.Text, StringComparison.Ordinal))
                    {
                        cambiar_pass();                                 
                    }
                    else
                        MessageBox.Show("Las contraseñas deben ser iguales", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Contraseña no valida\n\nLa contraseña debe tener los siguientes requisitos minimos:\n* 8 caracteres de longitud.\n* 1 letra minúscula.\n* 1 letramayúscula.\n* 1 numero.\n* 1 caracter especial.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }      
    }
}


