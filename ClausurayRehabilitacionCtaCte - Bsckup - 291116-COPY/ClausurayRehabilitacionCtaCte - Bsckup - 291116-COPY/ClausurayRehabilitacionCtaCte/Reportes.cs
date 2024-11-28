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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ClausurayRehabilitacionCtaCte
{
    public partial class Reportes : Form
    {
        private int? tipo = null;
        public Reportes()
        {
            InitializeComponent();
        }

        public Reportes (int idtipo) : this()
        {
            this.tipo = idtipo;
        }

        public string fechaini, fechafin;
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);       
        
        private void Reportes_Load_1(object sender, EventArgs e)
        {            
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            cargar_reportes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbreportes.SelectedIndex >= 0)
            {
                codrep = cbreportes.SelectedValue.ToString();
                reporte = cbreportes.Text;

                DateTime dia_hoy = DateTime.Now;
                if (Int32.Parse(dateTimePicker2.Text.Replace("-", "")) > Int32.Parse(dia_hoy.ToString("yyyyMMdd")))
                    MessageBox.Show("La fecha fin no puede ser mayor a la fecha actual.");
                else
                {
                    fecha1 = dateTimePicker1.Text;
                    fecha2 = dateTimePicker2.Text;
                    if (Int32.Parse(dateTimePicker1.Text.Replace("-", "")) <= Int32.Parse(dateTimePicker2.Text.Replace("-", "")))                    
                        busca_rep(codrep, dateTimePicker1.Text.Replace("-", ""), dateTimePicker2.Text.Replace("-", ""));                                           
                    else
                        MessageBox.Show("La fecha de inicio debe ser menor o igual a la fecha final.");                    
                }
            }
            else
                MessageBox.Show("Debe seleccionar un reporte valido.");                       
        }

        private void limpiar_dgv()
        {
            dgvrep.Columns.Clear();
        }        

        private void busca_rep(string rep, string f1, string f2)
        {
           
            try
            {
                DataTable dt = new DataTable();
                //dt.Clear();
                string nom_sp = "sp_reportes_" + rep;
                SqlCommand cmd = new SqlCommand(nom_sp, cone);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter fe1 = cmd.Parameters.Add("@fecha1", SqlDbType.VarChar, 10);
                fe1.Direction = ParameterDirection.Input;
                fe1.Value = f1;
                SqlParameter fe2 = cmd.Parameters.Add("@fecha2", SqlDbType.VarChar, 10);
                fe2.Direction = ParameterDirection.Input;
                fe2.Value = f2;
                
                cone.Open();
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                dgvrep.DataSource = dt;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error al buscar el reporte. " + ex.Message);
            }
            finally
            {
                cone.Close();
            }
            
        }

        public string fecha1, fecha2, reporte, codrep;

        private void btn_excel_Click(object sender, EventArgs e)
        {
            ExportarEXCEL exp = new ExportarEXCEL();
            exp.ExportarDG(dgvrep, fecha1, fecha2, codrep);
        }

       

        private void btn_pdf_Click(object sender, EventArgs e)
        {
            ExportarPDF exp = new ExportarPDF();
            exp.ExportarDGV(dgvrep, reporte, fecha1, fecha2, codrep);            
        }

        DataTable tcr = new DataTable();
        DataTable trr = new DataTable();
        DataTable ttr = new DataTable();

        public void cargar_reportes()
        {
            try
            {
                SqlCommand cmdc = new SqlCommand("sp_cargar_reportes_clau", cone);
                cmdc.CommandType = CommandType.StoredProcedure;
                SqlCommand cmdr = new SqlCommand("sp_cargar_reportes_rehab", cone);
                cmdr.CommandType = CommandType.StoredProcedure;
                SqlCommand cmdt = new SqlCommand("sp_cargar_reportes_rpres", cone);
                cmdt.CommandType = CommandType.StoredProcedure;

                cone.Open();
                SqlDataAdapter dac = new SqlDataAdapter(cmdc);
                dac.Fill(tcr);
                SqlDataAdapter dar = new SqlDataAdapter(cmdr);
                dar.Fill(trr);
                SqlDataAdapter dat = new SqlDataAdapter(cmdt);
                dat.Fill(ttr);                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Se produjo un error alcargar los datos de los reportes." + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }

        private void btn_limpiar_Click(object sender, EventArgs e)
        {
            dgvrep.Columns.Clear();
        }

        private void rclau_CheckedChanged(object sender, EventArgs e)
        {
            if (rclau.Checked)
            {
                cbreportes.DisplayMember = "NOMBRE_REP";
                cbreportes.ValueMember = "ID_REP";
                cbreportes.DataSource = tcr;
            }
        }

        private void rrehab_CheckedChanged(object sender, EventArgs e)
        {
            if (rrehab.Checked)
            {
                cbreportes.DisplayMember = "NOMBRE_REP";
                cbreportes.ValueMember = "ID_REP";
                cbreportes.DataSource = trr;
            }
        }

        private void rpres_CheckedChanged(object sender, EventArgs e)
        {
            if (rpres.Checked)
            {
                cbreportes.DisplayMember = "NOMBRE_REP";
                cbreportes.ValueMember = "ID_REP";
                cbreportes.DataSource = ttr;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbreportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvrep.Columns.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


    }


    
}
