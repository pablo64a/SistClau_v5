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
using System.DirectoryServices.AccountManagement;

namespace ClausurayRehabilitacionCtaCte
{
    public partial class Form_Ingreso : Form
    {
        public Form_Ingreso()
        {
            InitializeComponent();
            Mayus();
            timer1_caps.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (t1.Text.Trim() == "" || t2.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar los valores de matrícula o contraseña");
            }
            else
            {
                if (ValidateUserPass(t1.Text, t2.Text))
                {
                    acceso(t1.Text);
                }
                else
                {
                    MessageBox.Show("Error en el usuario o Contraseña : " + t1.Text, "Autentificación Incorrecta");
                }
            }
        }


        //validacion de usuario  administrador
        public Boolean validacion_administrador(string matricula)
        {
            Boolean estado = false;
            String rol = "";
            String query = "SELECT [ROL] FROM [TBLUSUARIOS] WHERE USUARIO = '" + matricula + "';";
            cone.Open();
            SqlCommand cmd = new SqlCommand(query, cone);
            rol = (String)cmd.ExecuteScalar();
            cone.Close();

            if (rol == "Administrador")
            {
                estado = true;
                Program.Administrador = true;
            }
            else
            {
                estado = false;
            }

            return estado;
        }
        //validar el usuario con rol de operador
        public Boolean validacion_rolOpe(string matricula)
        {
            Boolean estado = false;
            String rol = "";
            String query = "SELECT [ROL] FROM [TBLUSUARIOS] WHERE USUARIO = '" + matricula + "';";
            cone.Open();
            SqlCommand cmd = new SqlCommand(query, cone);
            rol = (String)cmd.ExecuteScalar();
            cone.Close();

            if (rol == "Operador")
            {
                estado = true;
                Program.Administrador = false;
            }
            else
            {
                estado = false;
            }

            return estado;
        }







        Ingresos ingreso = new Ingresos();
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);
        public Boolean caps;

        public void Mayus()
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                label4.Visible = true;
                label5.Visible = true;
                caps = true;
            }
            else
            {
                label4.Visible = false;
                label5.Visible = false;
                caps = false;
            }
        }

        private void acceso(string userName)
        {
                try
                {
                    if (validacion(userName))
                    {
                        /*************************** Cambiando login *************************************/
                        SqlCommand cmd = new SqlCommand("sp_login_modif", cone);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        SqlParameter usuario = new SqlParameter("@usuario", SqlDbType.VarChar);
                        usuario.Direction = ParameterDirection.Input;
                        usuario.Value = t1.Text;
                        cmd.Parameters.Add(usuario);
                        //SqlParameter clave = new SqlParameter("@clave", SqlDbType.VarChar);
                        //clave.Direction = ParameterDirection.Input;
                        //clave.Value = t2.Text;
                        //cmd.Parameters.Add(clave);
                        SqlParameter res = new SqlParameter("@resultado", SqlDbType.VarChar, 1);
                        res.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(res);
                        SqlParameter nomb = new SqlParameter("@nombre", SqlDbType.VarChar, 160);
                        nomb.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(nomb);

                        cone.Open();
                        cmd.ExecuteNonQuery();


                        string nivel = (string)cmd.Parameters["@resultado"].Value;
                        string nombre = (string)cmd.Parameters["@nombre"].Value;


                        //if(ValidateUserPass())



                        if (nivel == "9")
                        {
                            MessageBox.Show("Usuario o Clave incorrecto... Intente otra vez.");
                            ingreso.logs("ING-02", t1.Text, 1);
                        }
                        else
                        {
                            if (nivel == "2" )
                            {
                                Program.Administrador = false;
                                ingreso.logs("ING-00", t1.Text, 0);
                                Program.nivel = Int32.Parse(nivel);
                                Program.usuario = t1.Text;
                                MessageBox.Show("Bienvenido al sistema " + nombre);
                                FormPrincipal fp = new FormPrincipal();
                                this.Hide();        
                                fp.ShowDialog();
                                this.Close();
                                
                        }
                            else
                            {
                                ingreso.logs("ING-03", t1.Text, 0);
                                MessageBox.Show("No tiene permisos de Operador contactese con el Administrador ");
                            }
                        }
                    }
                    else
                    {
                        //poner aca la Excepcion
                        MessageBox.Show("Usuario no Autorizado, Contactese con el administrador : " + t1.Text, "Autentificación Incorrecta");

                    }

                }
                catch (SqlException ex)
                {
                    ingreso.logs("ING-03", t1.Text, 1);
                    MessageBox.Show("Error al intentar ingresar.\n" + ex.Message);
                }
                finally
                {
                    cone.Close();
                }
           }

        private void acceso1(string userName)
        {
            bool verificador = false;
            try
            {
                if (validacion(userName))
                {
                    /*************************** Cambiando login *************************************/
                    SqlCommand cmd = new SqlCommand("sp_login_modif", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    SqlParameter usuario = new SqlParameter("@usuario", SqlDbType.VarChar);
                    usuario.Direction = ParameterDirection.Input;
                    usuario.Value = t1.Text;
                    cmd.Parameters.Add(usuario);
                    //SqlParameter clave = new SqlParameter("@clave", SqlDbType.VarChar);
                    //clave.Direction = ParameterDirection.Input;
                    //clave.Value = t2.Text;
                    //cmd.Parameters.Add(clave);
                    SqlParameter res = new SqlParameter("@resultado", SqlDbType.VarChar, 1);
                    res.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(res);
                    SqlParameter nomb = new SqlParameter("@nombre", SqlDbType.VarChar, 160);
                    nomb.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(nomb);

                    cone.Open();
                    cmd.ExecuteNonQuery();


                    string nivel = (string)cmd.Parameters["@resultado"].Value;
                    string nombre = (string)cmd.Parameters["@nombre"].Value;


                    if (nivel == "9")
                    {
                        MessageBox.Show("Usuario o Clave incorrecto... Intente otra vez.");
                        ingreso.logs("ING-02", t1.Text, 0);
                    }
                    else
                    {
                        if (nivel == "1")
                        {
                            Program.Administrador = true;
                            ingreso.logs("ING-00", t1.Text, 0);
                            Program.nivel = Int32.Parse(nivel);
                            Program.usuario = t1.Text;
                            //MessageBox.Show("Bienvenido al sistema " + nombre);

                            //FormPrincipal fp = new FormPrincipal();
                            //this.Hide();
                            //fp.ShowDialog();
                            //this.Close();
                            verificador = true;
                        }
                        else
                        {   
                            ingreso.logs("ING-02", t1.Text, 1);
                            //MessageBox.Show("No tiene Permisos de Administrador: ");
                        }

                        if (nivel == "2")
                        {
                            Program.Administrador = false;
                            ingreso.logs("ING-00", t1.Text, 0);
                            Program.nivel = Int32.Parse(nivel);
                            Program.usuario = t1.Text;
                            //MessageBox.Show("Bienvenido al sistema " + nombre);
                            
                            //FormPrincipal fp = new FormPrincipal();
                            //this.Hide();
                            //fp.ShowDialog();
                            //this.Close();
                            verificador = true;
                        }
                        else
                        {
                            ingreso.logs("ING-03", t1.Text, 1);
                            //MessageBox.Show("No tiene permisos de Operador contactese con el Administrador ");
                        }

                        if (verificador)
                        {
                            MessageBox.Show("Bienvenido al sistema " + nombre);
                            FormPrincipal fp = new FormPrincipal();
                            this.Hide();
                            fp.ShowDialog();
                            this.Close();
                           
                        }
                        else
                        {
                            MessageBox.Show("No tiene Permisos, contactese con el Administrador... ");
                        }

                    }
                }
                else
                {
                    //poner aca la Excepcion
                    MessageBox.Show("Usuario no Autorizado, Contactese con el administrador : " + t1.Text, "Autentificación Incorrecta");

                }

            }
            catch (SqlException ex)
            {
                ingreso.logs("ING-03", t1.Text, 1);
                MessageBox.Show("Error al intentar ingresar.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }


        //Verificación del usuario en la BD en la tabla de USUARIO
        public Boolean validacion(string userName)
        {

            Boolean estado = false;
            string userName1;
            userName1 = userName.Replace("BTBNET\\", "").ToUpper();
            String query = "SELECT usuario from [TBLUSUARIOS] where usuario = '" + userName + "';";
            cone.Open();
            SqlCommand cmd = new SqlCommand(query, cone);
            userName = (string)cmd.ExecuteScalar();
            cone.Close();
            //userName = userName.Replace("BTBNET\\", "").ToUpper();
            //userName1 = userName;
            if (userName != userName1)
            {
                estado = false;
            }
            else
            {
                estado = true;
            }

            return estado;
        }

        //validar al usuario con credenciales de Dominio
        private bool ValidateUserPass(string userName, string userPass)
        {
            var domainContext = new PrincipalContext(ContextType.Domain);
            return domainContext.ValidateCredentials(userName, userPass);

        }

        private void Form_Ingreso_Load(object sender, EventArgs e)
        {
            t2.MaxLength = 50;
            t2.PasswordChar = '*';
            t1.Focus();         
        }

      

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetterOrDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros o letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void timer1_caps_Tick(object sender, EventArgs e)
        {
            Mayus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (t1.Text.Trim() == "" || t2.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar los valores de matrícula o contraseña");
            }
            else
            {
                if (ValidateUserPass(t1.Text, t2.Text))
                {
                    acceso1(t1.Text);
                    //acceso(t1.Text);
                }
                else
                {
                    //acceso(t1.Text);
                    MessageBox.Show("Error en el usuario o Contraseña : " + t1.Text, "Autentificación Incorrecta");
                }

                ////if (ValidateUserPass(t1.Text, t2.Text))
                //{
                //    //acceso(t1.Text);
                //}
                ////else
                //{
                //  //  MessageBox.Show("Error en el usuario o Contraseña : " + t1.Text, "Autentificación Incorrecta");
                //}


            }

        }

      

      

    }
}
