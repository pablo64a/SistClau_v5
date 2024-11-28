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
    public partial class Form_Gestion_Destinatarios : Form
    {
        public Form_Gestion_Destinatarios()
        {
            InitializeComponent();
            
        }

        private static Form_Gestion_Destinatarios m_FormDefInstance;
        public static Form_Gestion_Destinatarios DefInstance
        {
            get
            {
                if (m_FormDefInstance == null || m_FormDefInstance.IsDisposed)
                    m_FormDefInstance = new Form_Gestion_Destinatarios();
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
        public string usuario_mod="";
        Ingresos ingreso = new Ingresos();

        private void Form_correos_reportes_Load(object sender, EventArgs e)
        {
            if (Program.operacion == 0)
            {
                panel1.Visible = false;
                lbldest1.Text = "ADICIÓN DE REPORTES QUE SE ENVIARÁN AL USUARIO";
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox6.Visible = true;               
            }
            else
            {
                lbldest1.Text = "MODIFICACIÓN DE  REPORTES A DESTINATARIOS";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = false;
                groupBox4.Visible = false;
                groupBox6.Visible = false;               
                cargar_usuarios();               
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbclau.Checked)
            {
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
                cbc1.Checked = false;
                cbc2.Checked = false;
                cbc3.Checked = false;
                cbc4.Checked = false;
                cbc5.Checked = false;
                cbc6.Checked = false;
                cbc7.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbrehab.Checked)
            {
                groupBox4.Visible = true;
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
                cbr1.Checked = false;
                cbr2.Checked = false;
                cbr3.Checked = false;
                cbr4.Checked = false;  
            }
            if (!cbrehab.Checked)
            {
                if (!cbpres.Checked)
                    groupBox4.Visible = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpres.Checked)
            {
                groupBox3.Visible = true;
                groupBox4.Visible = true;
            }
            else
            {
                groupBox3.Visible = false;
                cp1.Checked = false;
                cp2.Checked = false;
                cp3.Checked = false;
                cp4.Checked = false;
            }

            if (!cbpres.Checked)
            {
                if (!cbrehab.Checked)
                    groupBox4.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                cbcorreos.DisplayMember = "NOMB";
                cbcorreos.ValueMember = "USUARIO";
                cbcorreos.DataSource = dtu;
            }                      
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                cbcorreos.DisplayMember = "USUARIO";
                cbcorreos.ValueMember = "USUARIO";
                cbcorreos.DataSource = dtu;
            }
        }

        private void cargar_correos_usuario()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_usuarios_correos_cr", cone);     
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter user = new SqlParameter("@usuario", SqlDbType.VarChar);
                user.Direction = ParameterDirection.Input;
                user.Value = cbcorreos.SelectedValue.ToString();
                cmd.Parameters.Add(user);

                cone.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string c = dr.GetString(0);
                        string r = dr.GetString(1);
                        string p = dr.GetString(2);
                        string rp = dr.GetString(3);
                        string cir = dr.GetString(4);
                        string tab = dr.GetString(5);
                        string clau = dr.GetString(6);
                        string rehab = dr.GetString(7);

                        llenar_checks(c, r, p, rp,cir,tab,clau,rehab);                    
                    }
                }
                else                
                    MessageBox.Show("No se encontraron los correos del usuario " + cbcorreos.Text);                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error.\n"+ex.Message);
            }
            finally
            {
                cone.Close();
            }

        }

        
        DataTable dtu = new DataTable();

        private void cargar_usuarios()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_cargar_usuarios", cone);
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


        private void llenar_checks(string a, string b, string c, string d, string cir, string tab, string clau, string rehab)
        {
            if (Convert.ToInt32(a) == 0)
            {
                cbclau.Checked = true;
                cbc1.Checked = true;
                cbc2.Checked = true;
                cbc3.Checked = true;
                cbc4.Checked = true;
                cbc5.Checked = true;
                cbc6.Checked = true;
                cbc7.Checked = true;
            }
            else
            {
                if (a == "1111111")
                {
                    cbclau.Checked = false;
                    cbc1.Checked = false;
                    cbc2.Checked = false;
                    cbc3.Checked = false;
                    cbc4.Checked = false;
                    cbc5.Checked = false;
                    cbc6.Checked = false;
                    cbc7.Checked = false;
                }
                else
                {
                    cbclau.Checked = true;
                    if (Convert.ToString(a[0]) == "0")
                        cbc1.Checked = true;
                    else
                        cbc1.Checked = false;
                    if (Convert.ToString(a[1]) == "0")
                        cbc2.Checked = true;
                    else
                        cbc2.Checked = false;
                    if (Convert.ToString(a[2]) == "0")
                        cbc3.Checked = true;
                    else
                        cbc4.Checked = false;
                    if (Convert.ToString(a[3]) == "0")
                        cbc4.Checked = true;
                    else
                        cbc4.Checked = false;
                    if (Convert.ToString(a[4]) == "0")
                        cbc5.Checked = true;
                    else
                        cbc5.Checked = false;
                    if (Convert.ToString(a[5]) == "0")
                        cbc6.Checked = true;
                    else
                        cbc6.Checked = false;
                    if (Convert.ToString(a[6]) == "0")
                        cbc7.Checked = true;
                    else
                        cbc7.Checked = false;
                }
            }
            if (Convert.ToInt32(b) == 0)
            {
                cbrehab.Checked = true;
                cbr1.Checked = true;
                cbr2.Checked = true;
                cbr3.Checked = true;
                cbr4.Checked = true;
            }
            else
            {
                if (b == "1111")
                {
                    cbrehab.Checked = false;
                    cbr1.Checked = false;
                    cbr2.Checked = false;
                    cbr3.Checked = false;
                    cbr4.Checked = false;                    
                }
                else
                {
                    cbrehab.Checked = true;
                    if (Convert.ToString(b[0]) == "0")
                        cbr1.Checked = true;
                    else
                        cbr1.Checked = false;
                    if (Convert.ToString(b[1]) == "0")
                        cbr2.Checked = true;
                    else
                        cbr2.Checked = false;
                    if (Convert.ToString(b[2]) == "0")
                        cbr3.Checked = true;
                    else
                        cbr3.Checked = false;
                    if (Convert.ToString(b[3]) == "0")
                        cbr4.Checked = true;
                    else
                        cbr4.Checked = false;
                }
            }
            if (Convert.ToInt32(c) == 0)
            {
                cbpres.Checked = true;
                cp1.Checked = true;
                cp2.Checked = true;
                cp3.Checked = true;
                cp4.Checked = true;
            }
            else
            {
                if (c == "1111")
                {
                    cbpres.Checked = false;
                    cp1.Checked = false;
                    cp2.Checked = false;
                    cp3.Checked = false;
                    cp4.Checked = false;
                }
                else
                {
                    cbpres.Checked = true;
                    if (Convert.ToString(c[0]) == "0")
                        cp1.Checked = true;
                    else
                        cp1.Checked = false;
                    if (Convert.ToString(c[1]) == "0")
                        cp2.Checked = true;
                    else
                        cp2.Checked = false;
                    if (Convert.ToString(c[2]) == "0")
                        cp3.Checked = true;
                    else
                        cp3.Checked = false;
                    if (Convert.ToString(c[3]) == "0")
                        cp4.Checked = true;
                    else
                        cp4.Checked = false;
                }
            }
            if (Convert.ToString(d[0]) == "0")
            {
                groupBox4.Visible = true;
                cbrp.Checked = true;
            }
            else
                cbrp.Checked = false;

            if (Convert.ToInt32(cir) + Convert.ToInt32(tab) + Convert.ToInt32(clau) + Convert.ToInt32(rehab) == 0)
            {
                cb_detalles.Checked = true;
                cbcir1.Checked = true;
                cbcir2.Checked = true;
                cbtab1.Checked = true;
                cbtab2.Checked = true;
                cbcl1.Checked = true;
                cbcl2.Checked = true;
                cbre1.Checked = true;
                cbre2.Checked = true;
            }
            else
            {
                if (cir + tab + clau + rehab == "11111111")
                {
                    cb_detalles.Checked = false;
                    cbcir1.Checked = false;
                    cbcir2.Checked = false;
                    cbtab1.Checked = false;
                    cbtab2.Checked = false;
                    cbcl1.Checked = false;
                    cbcl2.Checked = false;
                    cbre1.Checked = false;
                    cbre2.Checked = false;
                }
                else
                {
                    cb_detalles.Checked = true;
                    if (Convert.ToString(cir[0]) == "0")
                        cbcir1.Checked = true;
                    else
                        cbcir1.Checked = false;
                    if (Convert.ToString(cir[1]) == "0")
                        cbcir2.Checked = true;
                    else
                        cbcir2.Checked = false;
                    if (Convert.ToString(tab[0]) == "0")
                        cbtab1.Checked = true;
                    else
                        cbtab1.Checked = false;
                    if (Convert.ToString(tab[1]) == "0")
                        cbtab2.Checked = true;
                    else
                        cbtab2.Checked = false;
                    if (Convert.ToString(clau[0]) == "0")
                        cbcl1.Checked = true;
                    else
                        cbcl1.Checked = false;
                    if (Convert.ToString(clau[1]) == "0")
                        cbcl2.Checked = true;
                    else
                        cbcl2.Checked = false;
                    if (Convert.ToString(rehab[0]) == "0")
                        cbre1.Checked = true;
                    else
                        cbre1.Checked = false;
                    if (Convert.ToString(rehab[1]) == "0")
                        cbre2.Checked = true;
                    else
                        cbre2.Checked = false;
                }
            }           
        }

    

        private void cbcir_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_detalles.Checked)
            {
                groupBox6.Visible = true;
            }
            else
            {
                groupBox6.Visible = false;
                cbcir1.Checked = false;
                cbcir2.Checked = false;
                cbtab1.Checked = false;
                cbtab2.Checked = false;
                cbcl1.Checked = false;
                cbcl2.Checked = false;
                cbre1.Checked = false;
                cbre2.Checked = false;
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            if (Program.operacion == 0)            
                MessageBox.Show("No se adiconó reportes que recibira el usuario. Si posteriormente desea adicionarlos lo puede hacer desde la operacion de modificacion de correos de los destinatarios.");            
            else
                MessageBox.Show("No se guardaron los cambios realizados");

            this.Close();
        }
        
        private string cargar_checks()
        {
            string correos = "";
            if (cbcir1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbcir2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbtab1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbtab2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc3.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc4.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc5.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc6.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbc7.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbr1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbr2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbr3.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbr4.Checked)
                correos += "0";
            else
                correos += "1";
            if (cp1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cp2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cp3.Checked)
                correos += "0";
            else
                correos += "1";
            if (cp4.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbrp.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbcl1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbcl2.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbre1.Checked)
                correos += "0";
            else
                correos += "1";
            if (cbre2.Checked)
                correos += "0";
            else
                correos += "1";

            return correos;
        }

        private void actualizar_dest(string user)
        {
            string val = cargar_checks();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_destinatarios", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter usuario1 = new SqlParameter("@usuario", SqlDbType.VarChar);
                usuario1.Direction = ParameterDirection.Input;
                usuario1.Value = user;
                SqlParameter valores = new SqlParameter("@valores", SqlDbType.VarChar);
                valores.Direction = ParameterDirection.Input;
                valores.Value = val;
                cmd.Parameters.Add(usuario1);
                cmd.Parameters.Add(valores);

                cone.Open();
                cmd.ExecuteNonQuery();

                ingreso.logs("GDS-00",Program.usuario,0);

                MessageBox.Show("Se modificaron los correos para el usuario "+user+".");

                limpiar();
            }
            catch (SqlException ex)
            {
                ingreso.logs("GDS-01", Program.usuario, 1);
                MessageBox.Show("Se produjo un error al actualizar los datos.\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void limpiar()
        {
            cbcorreos.Text = "";
            cb_detalles.Checked = false;
            cbcir1.Checked = false;
            cbcir2.Checked = false;            
            cbtab1.Checked = false;
            cbtab2.Checked = false;
            cbclau.Checked = false;
            cbc1.Checked = false;
            cbc2.Checked = false;
            cbc3.Checked = false;
            cbc4.Checked = false;
            cbc5.Checked = false;
            cbc6.Checked = false;
            cbc7.Checked = false;
            cbrehab.Checked = false;
            cbr1.Checked = false;
            cbr2.Checked = false;
            cbr3.Checked = false;
            cbr4.Checked = false;
            cbpres.Checked = false;
            cp1.Checked = false;
            cp2.Checked = false;
            cp3.Checked = false;
            cp4.Checked = false;
            cbrp.Checked = false;
            cbcl1.Checked = false;
            cbcl2.Checked = false;
            cbre1.Checked = false;
            cbre2.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e) //Ingresar nuevos datos o actualizar... depende de como se ingrese al form
        {
            if (Program.operacion == 0)
            {
                DialogResult result = MessageBox.Show("Confirma los reportes para el usuario " + Program.val_glob + "?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    actualizar_dest(Program.val_glob);
                    this.Close();
                }
                else
                    MessageBox.Show("No se añadieron reportes al usuario.");
                
            }
            else
            {
                if (usuario_mod == "")
                    MessageBox.Show("Debe Seleccionar un usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    DialogResult result = MessageBox.Show("Confirma los cambios para el usuario " + "?", "Salir", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)                    
                        actualizar_dest(usuario_mod);                    
                    else
                        MessageBox.Show("No se guardaron los cambios realizados");
                }
            }
        }

        private void cbcorreos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_correos_usuario();
            usuario_mod = cbcorreos.SelectedValue.ToString();            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cbcir2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbrp_CheckedChanged(object sender, EventArgs e)
        {

        }

 
    }
}
