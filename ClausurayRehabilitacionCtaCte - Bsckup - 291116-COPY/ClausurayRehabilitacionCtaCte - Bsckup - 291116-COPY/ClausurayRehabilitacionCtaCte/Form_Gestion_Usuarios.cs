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
    public partial class Form_Gestion_Usuarios : Form
    {
        public Form_Gestion_Usuarios()
        {
            InitializeComponent();
        }

        private static Form_Gestion_Usuarios m_FormDefInstance;
        public static Form_Gestion_Usuarios DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Gestion_Usuarios();
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
       
        private void btnin_Click(object sender, EventArgs e)
        {
            if (rop1.Checked)
                insertar_usuario();
            else
            {
                if (rop2.Checked)
                    eliminar_usuario();
                else                    
                    editar_usuario();                
            }           
            cargar_usuarios();            
        }

        private void eliminar_usuario()
        {
            if (lbluser.Text != "")
            {
                DialogResult result = MessageBox.Show("Seguro que quiere eliminar al usuario " + lbluser.Text + " de los registros del sistema?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_eliminar_usuario", cone);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        SqlParameter usuario1 = new SqlParameter("@usuario", SqlDbType.VarChar);
                        usuario1.Direction = ParameterDirection.Input;
                        usuario1.Value = lbluser.Text;
                        cmd.Parameters.Add(usuario1);

                        cone.Open();
                        cmd.ExecuteNonQuery();

                        ingreso.logs("GUS-20", Program.usuario, 0);

                        MessageBox.Show("Usuario eliminado con éxito.");

                        limpiar();
                    }
                    catch (SqlException ex)
                    {
                        ingreso.logs("GUS-21", Program.usuario, 1);
                        MessageBox.Show("Error al intentar eliminar el usuario." + ex.Message);
                    }
                    finally
                    {
                        cone.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No se eliminó al usuario");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar el usuario a eliminar");
            }
        }

        private void editar_usuario()
        {
            if (lbluser.Text != "")
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Debe introducir un nombre");
                    textBox2.Focus();
                }
                else
                {
                    //if (valida_pass(textBox5.Text))
                    //{
                    //if (textBox5.Text.Equals(textBox51.Text, StringComparison.Ordinal))
                    //{
                    if (valida_correo(textBox6.Text))
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("sp_modificar_usuario_modif", cone);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            SqlParameter usuario1 = new SqlParameter("@usuario", SqlDbType.VarChar);
                            usuario1.Direction = ParameterDirection.Input;
                            usuario1.Value = lbluser.Text;
                            cmd.Parameters.Add(usuario1);
                            SqlParameter nombre = new SqlParameter("@nombre", SqlDbType.VarChar);
                            nombre.Direction = ParameterDirection.Input;
                            nombre.Value = textBox2.Text;
                            cmd.Parameters.Add(nombre);
                            SqlParameter paterno = new SqlParameter("@paterno", SqlDbType.VarChar);
                            paterno.Direction = ParameterDirection.Input;
                            paterno.Value = textBox3.Text;
                            cmd.Parameters.Add(paterno);
                            SqlParameter materno = new SqlParameter("@materno", SqlDbType.VarChar);
                            materno.Direction = ParameterDirection.Input;
                            materno.Value = textBox4.Text;
                            cmd.Parameters.Add(materno);
                            //SqlParameter clave = new SqlParameter("@clave", SqlDbType.NVarChar);
                            //clave.Direction = ParameterDirection.Input;
                            //clave.Value = textBox5.Text;
                            //cmd.Parameters.Add(clave);
                            SqlParameter niv = new SqlParameter("@nivel", SqlDbType.NVarChar);
                            niv.Direction = ParameterDirection.Input;
                            niv.Value = nivel;
                            cmd.Parameters.Add(niv);
                            SqlParameter correo = new SqlParameter("@correo", SqlDbType.VarChar);
                            correo.Direction = ParameterDirection.Input;
                            correo.Value = textBox6.Text;
                            cmd.Parameters.Add(correo);
                            SqlParameter rol = new SqlParameter("@rol", SqlDbType.VarChar);
                            rol.Direction = ParameterDirection.Input;
                            rol.Value = comboBox1.Text;
                            cmd.Parameters.Add(rol);

                            DialogResult result = MessageBox.Show("Seguro que quiere modificar los datos del usuario " + lbluser.Text + " de los registros del sistema?", "Salir", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                cone.Open();
                                cmd.ExecuteNonQuery();
                                ingreso.logs("GUS-30", Program.usuario, 0);
                                MessageBox.Show("Datos del usuario actualizados.");
                            }

                            limpiar();

                        }
                        catch (SqlException ei)
                        {
                            ingreso.logs("GUS-31", Program.usuario, 1);
                            if (ei.Number == 2627)
                                MessageBox.Show("El usuario ya existe.");
                            else
                                MessageBox.Show("Error al ingresar el registro.\n" + ei.Message);
                        }
                        finally
                        {
                            cone.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe colocar un correo valido");
                        textBox6.Focus();
                    }
                    //}
                    //else
                    //{
                    // MessageBox.Show("Las contraseñas deben ser iguales");
                    //textBox51.Focus();
                }
                //}
                //else
                //{
                //  MessageBox.Show("Contraseña no valida\n\nLa contraseña debe tener los siguientes requisitos minimos:\n* 8 caracteres de longitud.\n* 1 letra minúscula.\n* 1 letramayúscula.\n* 1 numero.\n* 1 caracter especial.");
                // textBox5.Focus();
                //}
                //}
            }
            else
                MessageBox.Show("Debe seleccionar un usuario.");
    
        }

        private void insertar_usuario()
        {
            textBox1.Visible = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Debe introducir un usuario");
                textBox1.Focus();
            }
            else
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Debe introducir un nombre");
                    textBox2.Focus();
                }
                else
                {
                    //if (valida_pass(textBox5.Text))
                    //{
                    //if (textBox5.Text.Equals(textBox51.Text,StringComparison.Ordinal))
                    //{
                    if (valida_correo(textBox6.Text))
                    {
                        ingresa_usuario();
                    }
                    else
                    {
                        MessageBox.Show("Debe colocar un correo valido");
                        textBox6.Focus();
                    }
                    //}
                    //else
                    //{
                    // MessageBox.Show("Las contraseñas deben ser iguales");
                    //textBox51.Focus();
                    //}
                    //}
                    //else
                    //{
                    //MessageBox.Show("Contraseña no valida\n\nLa contraseña debe tener los siguientes requisitos minimos:\n* 8 caracteres de longitud.\n* 1 letra minúscula.\n* 1 letramayúscula.\n* 1 numero.\n* 1 caracter especial.");
                    //textBox5.Focus();
                    //}
                }
            }
        }

        public string nivel;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                nivel = "1";
            if (comboBox1.SelectedIndex == 1)
                nivel = "2";
            if (comboBox1.SelectedIndex == 2)
                nivel = "3";
            if (comboBox1.SelectedIndex == 3)
                nivel = "0";
        }

        private void ingresa_usuario()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ingresar_usuario_modif", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter usuario1 = new SqlParameter("@usuario", SqlDbType.VarChar);
                usuario1.Direction = ParameterDirection.Input;
                usuario1.Value = textBox1.Text;
                cmd.Parameters.Add(usuario1);
                SqlParameter nombre = new SqlParameter("@nombre", SqlDbType.VarChar);
                nombre.Direction = ParameterDirection.Input;
                nombre.Value = textBox2.Text;
                cmd.Parameters.Add(nombre);
                SqlParameter paterno = new SqlParameter("@paterno", SqlDbType.VarChar);
                paterno.Direction = ParameterDirection.Input;
                paterno.Value = textBox3.Text;
                cmd.Parameters.Add(paterno);
                SqlParameter materno = new SqlParameter("@materno", SqlDbType.VarChar);
                materno.Direction = ParameterDirection.Input;
                materno.Value = textBox4.Text;
                cmd.Parameters.Add(materno);
                //SqlParameter clave = new SqlParameter("@clave", SqlDbType.NVarChar);
                //clave.Direction = ParameterDirection.Input;
                //clave.Value = textBox5.Text;
                //cmd.Parameters.Add(clave);
                SqlParameter niv = new SqlParameter("@nivel", SqlDbType.NVarChar);
                niv.Direction = ParameterDirection.Input;
                niv.Value = nivel;
                cmd.Parameters.Add(niv);
                SqlParameter correo = new SqlParameter("@correo", SqlDbType.VarChar);
                correo.Direction = ParameterDirection.Input;
                correo.Value = textBox6.Text;
                cmd.Parameters.Add(correo);
                SqlParameter rol = new SqlParameter("@rol", SqlDbType.VarChar);
                rol.Direction = ParameterDirection.Input;
                rol.Value = comboBox1.Text;
                cmd.Parameters.Add(rol);

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GUS-10",Program.usuario,0);

                MessageBox.Show("Usuario ingresado con éxito.");

                MessageBox.Show("Debe ingresar los correos de reportes que recibirá el usuario.");

                Program.operacion = 0;
                Program.val_glob = textBox1.Text;
                Form_Gestion_Destinatarios.DefInstance.Show(); 

                limpiar();

            }
            catch (SqlException ei)
            {
                ingreso.logs("GUS-11", Program.usuario, 1);
                if (ei.Number == 2627)
                    MessageBox.Show("El usuario ya existe.");
                else
                    MessageBox.Show("Error al ingresar el registro.\n" + ei.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void Form_Agregar_Usuario_Load(object sender, EventArgs e)
        {
            //textBox5.MaxLength = 50;
            //textBox5.PasswordChar = '*';
            //textBox51.MaxLength = 50;
            //textBox51.PasswordChar = '*';
            rop1.Checked = true;

                      
        }

        public Boolean valida_correo(string correo)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (regex.IsMatch(correo))
                return true;
            else
                return false;
        }


        public Boolean valida_pass(string pass)
        {
            Regex regex = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

            if (regex.IsMatch(pass))
                return true;                
            else
                return false;                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void limpiar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            //textBox5.Text = "";
            //textBox51.Text = "";
            textBox6.Text = "";
            comboBox1.SelectedIndex = 2;
            cbusuarios.SelectedIndex = -1;
        }

        DataTable dtu = new DataTable();
        DataTable dtd = new DataTable();

        private void cargar_usuarios()
        {
            dtu.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand("[sp_cargar_usuarios]", cone);
                cmd.CommandType = CommandType.StoredProcedure;

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dtu);

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void rop1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible = false;
            lbluser.Visible = false;
            btndinam.Text = "Añadir";
            textBox1.Visible = true;
            if (rop1.Checked)
                limpiar();
        }

        private void rop2_CheckedChanged(object sender, EventArgs e)
        {
            cargar_usuarios();
            panel1.Visible = true;
            lbluser.Visible = true;
            textBox1.Visible = false;
            btndinam.Text = "Eliminar";
            if (rop2.Checked)
                limpiar();            
        }

        private void rop3_CheckedChanged(object sender, EventArgs e)
        {
            cargar_usuarios();
            panel1.Visible = true;
            lbluser.Visible = true;
            textBox1.Visible = false;
            btndinam.Text = "Guardar";
            if (rop3.Checked)
                limpiar();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
            {
                cbusuarios.DisplayMember = "NOMB";
                cbusuarios.ValueMember = "USUARIO";
                cbusuarios.DataSource = dtu;
            }
            else
            {
                cbusuarios.DisplayMember = "USUARIO";
                cbusuarios.ValueMember = "USUARIO";
                cbusuarios.DataSource = dtu;
            }
        }

        private void cbusuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbusuarios.SelectedIndex != -1)
            {
                cargar_datos(cbusuarios.SelectedValue.ToString());
                lbluser.Text = cbusuarios.SelectedValue.ToString();
            }
        }

        private void cargar_datos(string usr)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_cargar_datos_usuario_modif", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter user = cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 20);
                user.Direction = ParameterDirection.Input;
                user.Value = usr;

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                textBox1.Text = (string)ds.Tables[0].Rows[0][0].ToString(); //usuario
                textBox2.Text = (string)ds.Tables[0].Rows[0][1].ToString(); //nombre
                textBox3.Text = (string)ds.Tables[0].Rows[0][2].ToString(); //Paterno
                textBox4.Text = (string)ds.Tables[0].Rows[0][3].ToString(); //Materno
                //textBox5.Text = (string)ds.Tables[0].Rows[0][4].ToString(); //Nivel
                //textBox51.Text = (string)ds.Tables[0].Rows[0][4].ToString(); //Correo
                textBox6.Text = (string)ds.Tables[0].Rows[0][5].ToString();

                string niv = (string)ds.Tables[0].Rows[0][5].ToString();
                if (niv == "1")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    if (niv == "2")
                        comboBox1.SelectedIndex = 1;
                    else
                    {
                        if (niv == "3")
                            comboBox1.SelectedIndex = 2;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al cargar los datos del usuario " + usr + ".\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (!(char.IsLetterOrDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros o letras.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
