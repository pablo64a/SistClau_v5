/*
 
 * PPROPIEDAD INTELECTUAL DEL BANCO DE CRÉDITO DE BOLIVIA S.A. 
 * SISTEMA DESARROLLADO PARA LA DIVISIÓN DE AUDITORÍA
 * SISTEMA PARA EL CONTROL Y SEGUIMIENTO DE CLAUSURA Y REHABILITACIÓN DE CUENTAS CORRIENTES

*/

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
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using static iTextSharp.text.pdf.qrcode.Version;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using static iTextSharp.text.pdf.events.IndexEvents;
using System.IO;
using System.Data.OleDb;

namespace ClausurayRehabilitacionCtaCte
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
            nivel_usr = Program.nivel;
            //procedimiento para bloquear los accesos dependiendo el usuario que ingrese
            nivel_usr = 1;
            identificar_usuario();
           
        }

        public string desti_fallos, desti_exitos;

        public string dia_proceso,dia_actual,usuario,informe;
        public int sw_CC,sw_CR,sw_CT, sw_ap;

        public Int32 sw_journal, sw_TBLDBC, sw_TBLCTAPAS, sw_TBLRCO, sw_TBLCTAPAS_HIST, flag_error, contador;
        public int sw_pro3;

        public int nivel_usr;
        public int ultimo;

        public Boolean outlook = false;
      
        public string detalle1, detalle2, detalle3;

        public string reportes_proceso_3="";

        public string dia_de_proceso,hora_sist,hora_proceso1i,hora_proceso1f,hora1i,hora_proceso2i,hora_proceso2f,hora_proceso1r,hora_proceso2r,hora_proceso31,hora_proceso32,hora_proceso33;

        public string hora_fin;
        public int retardo_bd;

        public string hora_inicio_proc, hora_fin_proc;       
        Ingresos ingreso = new Ingresos();
        EnviarCorreo EnviaCorreo = new EnviarCorreo();
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);
        SqlConnection cone2 = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion2"].ConnectionString);
        //SqlConnection cone3 = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion3"].ConnectionString);
        SqlConnection cone4 = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString); 

        public string emisor = ConfigurationManager.AppSettings["emisor"];
        public string pass = ConfigurationManager.AppSettings["pass"];
        public string correo_fallos = ConfigurationManager.AppSettings["correo_fallos"];
        public string separador = ConfigurationManager.AppSettings["separador"];
        public string tabla_btra = ConfigurationManager.AppSettings["tabla_btra"];
       
        //Determina si el dia de entrada es un dia hábil
        public Boolean dia_habil(string fecha)
        {
            SqlCommand cmdp = new SqlCommand("sp_dia_habil",cone);                    
            cmdp.CommandType = CommandType.StoredProcedure;          
            cmdp.Parameters.Clear();
            SqlParameter fech = new SqlParameter("@fecha", SqlDbType.VarChar);
            fech.Direction = ParameterDirection.Input;
            fech.Value = fecha;
            cmdp.Parameters.Add(fech);           
            SqlParameter hab = cmdp.Parameters.Add("@habil", SqlDbType.Int);
            hab.Direction = ParameterDirection.Output;               
       
            cone.Open();
            cmdp.ExecuteNonQuery();                       
            int dia_hab = (Int32)cmdp.Parameters["@habil"].Value;
            cone.Close();

            if (dia_hab == 0)
                return true;
            else
                return false;
        }

        //Determinación de fecha de ejecución
        public String det_fecha_proceso(string horap,DateTime dia)
        {            
            string sdia = dia.ToString("yyyyMMdd");            
            TimeSpan horapro = TimeSpan.Parse(horap);            
            if (horapro <= TimeSpan.Parse(separador))
            {
                //nos devuelve la fecha del ultimo día habil en formato "yyyyMMdd"
                string dia_p = obtiene_ultimo(sdia); 
                //para devolverlo en formato "ddMMyy"
                return dia_p.Substring(6, 2) + dia_p.Substring(4, 2) + dia_p.Substring(2, 2);
            }
            else
            {               
                return dia.ToString("ddMMyy");
            }
        }
               

        //Proceso de carga de tablas del Banco (journal, TBLDBC, TBLCTAPAS y TBLRCO)

        

        //Proceso 3: Proceso de Clausura........................................
        public void proceso_clausura(int vuelta,string fecha_p,string fecha_e)
        {
            ingreso.logs("PCL-00", Program.usuario, 0);
            //obtiene los destinatarios para el proceso, reporte de ejecución fallida o exitosa
            obtiene_destinatarios(3);
            DateTime hora = DateTime.Now; //hora de inicio del procedimiento
            hora_inicio_proc = hora.ToString("HH:mm:ss");
            sw_pro3 = 1;

            int extra;
            string observ ="";
            detalle1 = "";           
            informe = "";

            if (vuelta == 1)
            {
                try
                {
                    ingreso.logs("PCL-01", Program.usuario, 0);
                    SqlCommand cmdp = new SqlCommand();                    
                    cmdp.CommandText = "sp_clausura_asfi_dia_1";
                    cmdp.CommandType = CommandType.StoredProcedure;
                    cmdp.Connection = cone;
                    cmdp.Parameters.Clear();
                    SqlParameter f1 = new SqlParameter("@fecha1", SqlDbType.VarChar);
                    f1.Direction = ParameterDirection.Input;
                    f1.Value = fecha_p;
                    cmdp.Parameters.Add(f1);
                    SqlParameter f2 = new SqlParameter("@fecha2", SqlDbType.VarChar);
                    f2.Direction = ParameterDirection.Input;
                    f2.Value = fecha_e;
                    cmdp.Parameters.Add(f2);
                    SqlParameter info = cmdp.Parameters.Add("@informe", SqlDbType.VarChar, 1000);
                    info.Direction = ParameterDirection.Output;
                    SqlParameter sw = cmdp.Parameters.Add("@estado", SqlDbType.Int);
                    sw.Direction = ParameterDirection.Output;
                    SqlParameter obs = cmdp.Parameters.Add("@observacion", SqlDbType.VarChar, 200);
                    obs.Direction = ParameterDirection.Output;
                    SqlParameter ext = cmdp.Parameters.Add("@extra", SqlDbType.Int);
                    ext.Direction = ParameterDirection.Output;

                    cone.Open();
                    cmdp.ExecuteNonQuery();
                    informe = (string)cmdp.Parameters["@informe"].Value;
                    sw_pro3 = (Int32)cmdp.Parameters["@estado"].Value;
                    observ = (string)cmdp.Parameters["@observacion"].Value;
                    extra = (Int32)cmdp.Parameters["@extra"].Value;
                }
                catch (SqlException ex)
                {                   
                    añadir_detalleproceso(fecha_e, fecha_p, 3, "CLAUSURA DE CUENTAS CUENTAS CORRIENTES", hora_inicio_proc, hora_fin_proc, sw_pro3, usuario, informe,"", 1, 1, 1, 1);
                    abrir_outlook();
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Proceso Clausura de Cuentas Corrientes", "Se produjo un error al intentar realizar el proceso de clausura de cuentas corrientes. Descripción del error: " + ex.Message, desti_fallos);
                    cerrar_outlook();
                    extra = 1;
                }
                finally
                {
                    cone.Close();
                }

                if (extra == 0)
                {
                    DataTable dt_ctas = new DataTable();
                    SqlCommand cmcta = new SqlCommand();
                    cmcta.CommandType = CommandType.StoredProcedure;
                    cmcta.CommandText = "sp_clausuras_obtener_ctas";
                    cmcta.Connection = cone;
                    cmcta.Parameters.Clear();
                    SqlParameter fe1 = new SqlParameter("@fecha", SqlDbType.VarChar);
                    fe1.Direction = ParameterDirection.Input;
                    fe1.Value = fecha_e;
                    cmcta.Parameters.Add(fe1);

                    SqlDataAdapter da = new SqlDataAdapter(cmcta);

                    da.Fill(dt_ctas);
                    cone.Close();
                    if (dt_ctas.Rows.Count > 0)
                        robot(3, dt_ctas, vuelta);
                }
                //MessageBox.Show(sw_pro3.ToString());
                if (sw_pro3 == 0)
                {
                    cuerpo_correo(3, fecha_e);
                    informe = informe + detalle1;
                    exportar_diario(3, vuelta, fecha_e);
                    enviar_correos(3, informe, "", "");                    
                }
                else
                {
                    abrir_outlook();
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Proceso de Clausura de Cuentas Corrientes", informe, desti_fallos);
                    cerrar_outlook();
                }

                DateTime hora2 = DateTime.Now; //hora final del procedimiento
                hora_fin_proc = hora2.ToString("HH:mm:ss");

                añadir_detalleproceso(dia_actual, dia_proceso, 3, "CLAUSURA DE CUENTAS CUENTAS CORRIENTES", hora_inicio_proc, hora_fin_proc, sw_pro3, usuario, informe, observ, extra, 1, 1, 1);                                      
            }
            else
            {
                Int32 sw_rbt = verifica_robot(3, dia_actual);
                
                if (sw_rbt == 0)
                {
                    DataTable dt_ctas = new DataTable();
                    SqlCommand cmcta = new SqlCommand();
                    cmcta.CommandType = CommandType.StoredProcedure;
                    cmcta.CommandText = "sp_clausuras_obtener_ctas";
                    cmcta.Connection = cone;
                    cmcta.Parameters.Clear();
                    SqlParameter fe1 = new SqlParameter("@fecha", SqlDbType.VarChar);
                    fe1.Direction = ParameterDirection.Input;
                    fe1.Value = fecha_e;
                    cmcta.Parameters.Add(fe1);

                    SqlDataAdapter da = new SqlDataAdapter(cmcta);

                    da.Fill(dt_ctas);
                    cone.Close();
                    if (dt_ctas.Rows.Count > 0)
                        robot(3, dt_ctas, vuelta);

                    cuerpo_correo(3, fecha_e);//Devuelve las variables detalle1,detalle2,detalle3 llenadas
                    informe = informe + detalle1;
                    exportar_diario(3, vuelta,fecha_e);
                    enviar_correos(3, informe, "", "");
                }            
            }
            ingreso.logs("PCL-21", Program.usuario, 0);
        }

        //Proceso 4 - Rehabilitación de cuentas corrientes

        public void proceso_rehabilitacion(int vuelta, string fecha_p, string fecha_e)
        {
            ingreso.logs("PRE-00", Program.usuario, 0);
            obtiene_destinatarios(4); //destinatarios en caso de fallos o si se ejecutara exitosamente
            DateTime hora = DateTime.Now; //hora de inicio del procedimiento
            hora_inicio_proc = hora.ToString("HH:mm:ss");
            sw_pro3 = 1;
            string observ = "";
            int extra = 1;
            informe = "";

            if (vuelta == 1)
            {
                try
                {
                    ingreso.logs("PRE-01", Program.usuario, 0);
                    SqlCommand cmdp = new SqlCommand();
                    cmdp.CommandText = "sp_rehabilitacion_asfi_dia_1";
                    cmdp.CommandType = CommandType.StoredProcedure;
                    cmdp.Connection = cone;
                    cmdp.Parameters.Clear();
                    SqlParameter f1 = new SqlParameter("@fecha1", SqlDbType.VarChar);
                    f1.Direction = ParameterDirection.Input;
                    f1.Value = fecha_p;
                    cmdp.Parameters.Add(f1);
                    SqlParameter f2 = new SqlParameter("@fecha2", SqlDbType.VarChar);
                    f2.Direction = ParameterDirection.Input;
                    f2.Value = fecha_e;
                    cmdp.Parameters.Add(f2);
                    SqlParameter info = cmdp.Parameters.Add("@informe", SqlDbType.VarChar, 1000);
                    info.Direction = ParameterDirection.Output;
                    SqlParameter sw = cmdp.Parameters.Add("@estado", SqlDbType.Int);
                    sw.Direction = ParameterDirection.Output;
                    SqlParameter obs = cmdp.Parameters.Add("@observacion", SqlDbType.VarChar, 200);
                    obs.Direction = ParameterDirection.Output;
                    SqlParameter ext = cmdp.Parameters.Add("@extra", SqlDbType.Int);
                    ext.Direction = ParameterDirection.Output;

                    cone.Open();
                    cmdp.ExecuteNonQuery();
                    informe = (string)cmdp.Parameters["@informe"].Value;
                    sw_pro3 = (Int32)cmdp.Parameters["@estado"].Value;
                    observ = (string)cmdp.Parameters["@observacion"].Value;
                    extra = (Int32)cmdp.Parameters["@extra"].Value;                    
                }
                catch (SqlException ex)
                {
                    añadir_detalleproceso(dia_actual, dia_proceso, 4, "REHABILITACIÓN DE CUENTAS CUENTAS CORRIENTES", hora_inicio_proc, hora_fin_proc, sw_pro3, usuario, informe, "", 1, 1, 1, 1);
                    //MessageBox.Show(informe);
                    abrir_outlook();
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Proceso de Rehabilitacion de Cuentas Corrientes", "Se produjo un error al intentar realizar el proceso de rehabilitación de cuentas corrientes. Descripción del error: " + ex.Message, desti_fallos);
                    cerrar_outlook();
                    extra = 1;
                }
                finally
                {
                    cone.Close();
                }

                if (extra == 0)
                {
                    DataTable dt_ctas = new DataTable();
                    SqlCommand cmcta = new SqlCommand();
                    cmcta.CommandType = CommandType.StoredProcedure;
                    cmcta.CommandText = "sp_rehabilitaciones_obtener_ctas";
                    cmcta.Connection = cone;
                    cmcta.Parameters.Clear();
                    SqlParameter fe1 = new SqlParameter("@fecha", SqlDbType.VarChar);
                    fe1.Direction = ParameterDirection.Input;
                    fe1.Value = fecha_e;
                    cmcta.Parameters.Add(fe1);

                    SqlDataAdapter da = new SqlDataAdapter(cmcta);

                    da.Fill(dt_ctas);
                    cone.Close();
                    if (dt_ctas.Rows.Count > 0)
                        robot(4, dt_ctas, vuelta);
                }
                if (sw_pro3 == 0)
                {
                    cuerpo_correo(4, fecha_p);//Devuelve las variables detalle1,detalle2,detalle3 llenadas
                    informe = informe + detalle1 + detalle2 + detalle3;
                    exportar_diario(4, vuelta,fecha_p);
                    enviar_correos(4, detalle1, detalle2, detalle3);
                }
                else
                {
                    //MessageBox.Show(informe);
                    abrir_outlook();
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Proceso de Rehabilitación de Cuentas Corrientes.", informe, desti_fallos);
                    cerrar_outlook();
                }
                DateTime hora2 = DateTime.Now; //hora final del procedimiento
                hora_fin_proc = hora2.ToString("HH:mm:ss");                              
                
                añadir_detalleproceso(dia_actual, dia_proceso, 4, "REHABILITACIÓN DE CUENTAS CUENTAS CORRIENTES", hora_inicio_proc, hora_fin_proc, sw_pro3, usuario, informe, observ, extra, 1, 1, 1);                
            }
            else
            {
                Int32 sw_rbt = verifica_robot(4, fecha_e);
               
                if (sw_rbt == 0)
                {
                    DataTable dt_ctas = new DataTable();
                    SqlCommand cmcta = new SqlCommand();
                    cmcta.CommandType = CommandType.StoredProcedure;
                    cmcta.CommandText = "sp_rehabilitaciones_obtener_ctas";
                    cmcta.Connection = cone;
                    cmcta.Parameters.Clear();
                    SqlParameter fe1 = new SqlParameter("@fecha", SqlDbType.VarChar);
                    fe1.Direction = ParameterDirection.Input;
                    fe1.Value = fecha_e;
                    cmcta.Parameters.Add(fe1);

                    dt_ctas.Clear();
                    SqlDataAdapter da = new SqlDataAdapter(cmcta);
                    
                    da.Fill(dt_ctas);
                    cone.Close();
                    if (dt_ctas.Rows.Count > 0)
                        robot(4, dt_ctas, vuelta);

                    cuerpo_correo(4, fecha_p);//Devuelve las variables detalle1,detalle2,detalle3 llenadas
                    informe = informe + detalle1 + detalle2 + detalle3;
                    exportar_diario(4, vuelta,fecha_p);
                    enviar_correos(4, detalle1, detalle2, detalle3);
                }
            }
            ingreso.logs("PRE-21", Program.usuario, 0);
        }

        private void btnfecha_Click(object sender, EventArgs e)
        {
            DateTime ayer = DateTime.Today.AddDays(-1);
            string fecha_proceso = ayer.ToString("ddMMyy");
            MessageBox.Show(" "+fecha_proceso);
        }
              
        private string obtiene_ultimo(string actual)
        {
            try
            {                
                SqlCommand cmdp = new SqlCommand("ultimo", cone);
                cmdp.CommandType = CommandType.StoredProcedure;
                cmdp.Parameters.Clear();
                SqlParameter fech = new SqlParameter("@fecha", SqlDbType.VarChar);
                fech.Direction = ParameterDirection.Input;
                fech.Value = actual;
                cmdp.Parameters.Add(fech);
                SqlParameter fec_pro = cmdp.Parameters.Add("@ultimo_dia", SqlDbType.VarChar, 10);
                fec_pro.Direction = ParameterDirection.Output;               
                cone.Open();
                cmdp.ExecuteNonQuery();
                string fp = (string)cmdp.Parameters["@ultimo_dia"].Value;                              
                return fp;
            }
            catch (SqlException es2)
            {                
                return "19990101";
            }
            finally
            {
                cone.Close();
            }

        }

        private string obtiene_proximo(string actual)
        {
            try
            {
                SqlCommand cmdp = new SqlCommand("proximo", cone);
                cmdp.CommandType = CommandType.StoredProcedure;
                cmdp.Parameters.Clear();
                SqlParameter fech = new SqlParameter("@fecha", SqlDbType.VarChar);
                fech.Direction = ParameterDirection.Input;
                fech.Value = actual;
                cmdp.Parameters.Add(fech);
                SqlParameter fec_pro = cmdp.Parameters.Add("@proximo_dia", SqlDbType.VarChar, 10);
                fec_pro.Direction = ParameterDirection.Output;
                cone.Open();
                cmdp.ExecuteNonQuery();
                string fp = (string)cmdp.Parameters["@proximo_dia"].Value;
                return fp;
            }
            catch (SqlException es2)
            {
                return "19990101";
            }
            finally
            {
                cone.Close();
            }

        }
       
        private void FormPrincipal_Load_1(object sender, EventArgs e)
        {
            Boolean servidor = true;
            //Validando los permisos 
            //Si es administrador mostramos el menú de administración:
            if (Program.Administrador)
            {
                //menu 1
                repro.Enabled = true;
                //menu 2
                report.Enabled = true;
                //menu 3
                admin.Enabled = true;
                //menu 4
                ejecuciónManualToolStripMenuItem.Enabled = true;

            }
            else
            { 

                //menu 1
                repro.Enabled = true;
                //menu 2
                report.Enabled = true;
                //menu 3
                admin.Enabled = false;
                //menu 4
                ejecuciónManualToolStripMenuItem.Enabled = true;

            }



            //Mostrando errores de Ejecucion 
            try
            {
                cone.Open();
            }
            catch (SqlException ex)
            {                
                abrir_outlook();
                EnviaCorreo.Enviar_SinArchivos("SCR: Error Crítico - No se pudo iniciar el Sistema CRCTACTE", "No se pudo acceder al servidor del sistema<br><br>Descripción del Error:<br>" + ex.Message, correo_fallos);
                cerrar_outlook();
                servidor = false;
            }
            finally
            {
                cone.Close();
            }

            String fecha_hoy = DateTime.Today.ToString("yyyyMMdd"); //fecha actual, fecha de ejecución

            if (servidor && dia_habil(fecha_hoy))
            {
                DateTime dia_hoy = DateTime.Now;
                dia_actual = dia_hoy.ToString("yyyyMMdd");
                string inf_error = "<br><br>Descripción tecnica:<br>";
                usuario = Environment.UserName;
                int salida = 1;
                try
                {
                    cone.Open();
                    //cone2.Open();
                    //cone3.Open();
                    //cone4.Open();
                }
                catch (SqlException ec)
                {
                    salida = 0;
                    switch (ec.Number)
                    {
                        case 4060:
                            inf_error = "Descripción:<br>No se encontró la Base de Datos." + inf_error + ec.Message;
                            break;
                        case 17142:
                            inf_error = "Descripción:<br>El Servidor se encuentra pausado." + inf_error + ec.Message;
                            break;
                        default:
                            inf_error += ec.Message;
                            break;
                    }

                    abrir_outlook();
                    EnviaCorreo.Enviar_SinArchivos("SCR: Error crítico al tratar de acceder a la Base de Datos", inf_error, correo_fallos);
                    cerrar_outlook();
                    System.Windows.Forms.Application.Exit();
                }
                finally
                {
                    cone.Close();
                    //cone2.Close();
                    // cone3.Close();
                    //cone4.Close();
                }

                if (salida == 1)
                {
                    cargar_principal(inf_error, dia_hoy);
                }
            }
            else
            {

                cargar_principal("", DateTime.Now); 
                //System.Windows.Forms.Application.Exit();
            }





        }

        private void actualiza_horarios(DateTime dia_hoy)
        {
            SqlCommand cmdh = new SqlCommand("sp_cargar_principal", cone);
            cmdh.CommandType = CommandType.StoredProcedure;
            SqlParameter h11 = cmdh.Parameters.Add("@h11", SqlDbType.VarChar, 8);
            h11.Direction = ParameterDirection.Output;
            SqlParameter h12 = cmdh.Parameters.Add("@h12", SqlDbType.VarChar, 8);
            h12.Direction = ParameterDirection.Output;
            SqlParameter r1 = cmdh.Parameters.Add("@r1", SqlDbType.VarChar, 3);
            r1.Direction = ParameterDirection.Output;
            SqlParameter h21 = cmdh.Parameters.Add("@h21", SqlDbType.VarChar, 8);
            h21.Direction = ParameterDirection.Output;
            SqlParameter h22 = cmdh.Parameters.Add("@h22", SqlDbType.VarChar, 8);
            h22.Direction = ParameterDirection.Output;
            SqlParameter r2 = cmdh.Parameters.Add("@r2", SqlDbType.VarChar, 3);
            r2.Direction = ParameterDirection.Output;
            SqlParameter h31 = cmdh.Parameters.Add("@h31", SqlDbType.VarChar, 8);
            h31.Direction = ParameterDirection.Output;
            SqlParameter h32 = cmdh.Parameters.Add("@h32", SqlDbType.VarChar, 8);
            h32.Direction = ParameterDirection.Output;
            SqlParameter h33 = cmdh.Parameters.Add("@h33", SqlDbType.VarChar, 8);
            h33.Direction = ParameterDirection.Output;


            try
            {
                cone.Close();
                cone.Open();
                cmdh.ExecuteNonQuery();
                hora_proceso1i = (string)cmdh.Parameters["@h11"].Value;
                hora_proceso1f = (string)cmdh.Parameters["@h12"].Value;
                hora_proceso1r = (string)cmdh.Parameters["@r1"].Value;
                hora_proceso2i = (string)cmdh.Parameters["@h21"].Value;
                hora_proceso2f = (string)cmdh.Parameters["@h22"].Value;
                hora_proceso2r = (string)cmdh.Parameters["@r2"].Value;
                hora_proceso31 = (string)cmdh.Parameters["@h31"].Value;
                hora_proceso32 = (string)cmdh.Parameters["@h32"].Value;
                hora_proceso33 = (string)cmdh.Parameters["@h33"].Value;
                cone.Close();

                lblhora1i.Text = hora_proceso1i;
                lblhora1f.Text = hora_proceso1f;
                lblhora2i.Text = hora_proceso2i;
                lblhora2f.Text = hora_proceso2f;

                lblr1.Text = hora_proceso1r + " Min.";
                lblr2.Text = hora_proceso2r + " Min.";
                lblhora31.Text = hora_proceso31;
                lblhora32.Text = hora_proceso32;
                lblhora33.Text = hora_proceso33;
                dia_de_proceso = det_fecha_proceso(dia_hoy.ToString("HH:mm"), DateTime.Today);
                lblfechaproceso.Text = dia_de_proceso.Substring(0, 2) + "-" + dia_de_proceso.Substring(2, 2) + "-20" + dia_de_proceso.Substring(4, 2);
                lblfechaact.Text = dia_hoy.ToString("dd-MM-yyyy");
                dia_proceso = "20" + dia_de_proceso.Substring(4, 2) + dia_de_proceso.Substring(2, 2) + dia_de_proceso.Substring(0, 2);
                timer1.Enabled = true;
                timer_hora.Enabled = true;
                //posibilidad de descomentar probar despues 

            }
            catch (SqlException e1)
            {

            }
        }
        
        private void cargar_principal(string inf_error,DateTime dia_hoy)
        {
            SqlCommand cmdh = new SqlCommand("sp_cargar_principal", cone);
            cmdh.CommandType = CommandType.StoredProcedure;
            SqlParameter h11 = cmdh.Parameters.Add("@h11", SqlDbType.VarChar, 8);
            h11.Direction = ParameterDirection.Output;
            SqlParameter h12 = cmdh.Parameters.Add("@h12", SqlDbType.VarChar, 8);
            h12.Direction = ParameterDirection.Output;
            SqlParameter r1 = cmdh.Parameters.Add("@r1", SqlDbType.VarChar, 3);
            r1.Direction = ParameterDirection.Output;
            SqlParameter h21 = cmdh.Parameters.Add("@h21", SqlDbType.VarChar, 8);
            h21.Direction = ParameterDirection.Output;
            SqlParameter h22 = cmdh.Parameters.Add("@h22", SqlDbType.VarChar, 8);
            h22.Direction = ParameterDirection.Output;
            SqlParameter r2 = cmdh.Parameters.Add("@r2", SqlDbType.VarChar, 3);
            r2.Direction = ParameterDirection.Output;
            SqlParameter h31 = cmdh.Parameters.Add("@h31", SqlDbType.VarChar, 8);
            h31.Direction = ParameterDirection.Output;
            SqlParameter h32 = cmdh.Parameters.Add("@h32", SqlDbType.VarChar, 8);
            h32.Direction = ParameterDirection.Output;
            SqlParameter h33 = cmdh.Parameters.Add("@h33", SqlDbType.VarChar, 8);
            h33.Direction = ParameterDirection.Output;


            try
            {
                cone.Open();
                cmdh.ExecuteNonQuery();
                hora_proceso1i = (string)cmdh.Parameters["@h11"].Value;
                hora_proceso1f = (string)cmdh.Parameters["@h12"].Value;
                hora_proceso1r = (string)cmdh.Parameters["@r1"].Value;
                hora_proceso2i = (string)cmdh.Parameters["@h21"].Value;
                hora_proceso2f = (string)cmdh.Parameters["@h22"].Value;
                hora_proceso2r = (string)cmdh.Parameters["@r2"].Value;
                hora_proceso31 = (string)cmdh.Parameters["@h31"].Value;
                hora_proceso32 = (string)cmdh.Parameters["@h32"].Value;
                hora_proceso33 = (string)cmdh.Parameters["@h33"].Value;              
                cone.Close();

                lblhora1i.Text = hora_proceso1i;
                lblhora1f.Text = hora_proceso1f;
                lblhora2i.Text = hora_proceso2i;
                lblhora2f.Text = hora_proceso2f;
               
                lblr1.Text = hora_proceso1r + " Min.";
                lblr2.Text = hora_proceso2r + " Min.";
                lblhora31.Text = hora_proceso31;
                lblhora32.Text = hora_proceso32;
                lblhora33.Text = hora_proceso33;               
                dia_de_proceso = det_fecha_proceso(dia_hoy.ToString("HH:mm"), DateTime.Today);               
                lblfechaproceso.Text = dia_de_proceso.Substring(0, 2) + "-" + dia_de_proceso.Substring(2, 2) + "-20" + dia_de_proceso.Substring(4, 2);
                lblfechaact.Text = dia_hoy.ToString("dd-MM-yyyy");
                dia_proceso = "20" + dia_de_proceso.Substring(4, 2) + dia_de_proceso.Substring(2, 2) + dia_de_proceso.Substring(0, 2);               
                timer1.Enabled = true;
                timer_hora.Enabled = true;
            }
            catch (SqlException e1)
            {
                switch (e1.Number)
                {
                    case 9002:
                        inf_error = "Descripción del error:<br>Registro de transacciones lleno." + inf_error + e1.Message;
                        break;
                    case 17142:
                        inf_error = "Descripción del error:<br>Servidor pausado. " + inf_error + e1.Message;
                        break;
                    default:
                        inf_error = "Descripción del error:<br>" + e1.Message;
                        break;
                }

                abrir_outlook();
                EnviaCorreo.Enviar_SinArchivos("SCR: Error al iniciar el sistema.",inf_error,correo_fallos);
                cerrar_outlook();

                //correo.correo_informe(inf_error, "Error al iniciar el Sistema de Clausura y Rehabilitacion de Cuentas Corrientes", correo_fallos);
            }
        }

        private void timer_hora_Tick(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            lblhorasist.Text = hora.ToString("HH:mm:ss");
            hora_sist = hora.ToString("HH:mm:ss");

            dia_actual = hora.ToString("yyyyMMdd");
            /*****************************************************************************/
            string hora_sistema_aux = hora.ToString("ss");
            actualiza_horarios(DateTime.Today);
            /*****************************************************************************/
            if (hora_sist == "12:00:00" && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd")) && hora_sistema_aux.Equals("PM"))
            {
                //Prueba1.Text = DateTime.Now.AddDays(-3).ToLongDateString();
                cargar_principal("<br><br>Descripción tecnica:<br>", DateTime.Today);

            } 

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimeSpan.Parse(hora_sist) == TimeSpan.Parse("00:00:00"))
            {
                cargar_principal("<br><br>Descripción tecnica:<br>", DateTime.Now);
            }
            if (hora_sist == hora_proceso1i && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd"))) // && hora_sist == hora_proceso1i))
            {
                string aux = dia_actual;
                carga_circulares(dia_de_proceso,false);
                //añadir_detalleproceso(dia_actual, dia_proceso, 1, "CARGA DE CIRCULARES", hora_inicio_proc, dia_actual, 1, usuario, informe, "", sw_CC, sw_CR, sw_CT, 1);

            }
            if (hora_sist == hora_proceso2i && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd")))
            {

                    carga_tablas(dia_proceso, dia_actual, false);
                
                //string aux = dia_actual;
                //añadir_detalleproceso(dia_actual, dia_proceso, 1, "CARGA DE CIRCULARES", hora_inicio_proc, dia_actual, 1, usuario, informe, "", sw_CC, sw_CR, sw_CT, 1);
            }
            if (hora_sist == hora_proceso31 && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd")))
            {
                proceso_clausura(1,dia_proceso,dia_actual);
                proceso_rehabilitacion(1,dia_proceso,dia_actual);
            }
            //if (hora_sist == hora_proceso32 && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd")))
            //{
            //    proceso_clausura(2,dia_proceso,dia_actual);
            //    proceso_rehabilitacion(2,dia_proceso,dia_actual);
            //}
            //if (hora_sist == hora_proceso33 && nivel_usr < 3 && dia_habil(DateTime.Today.ToString("yyyyMMdd")))
            //{
            //    proceso_clausura(3,dia_proceso,dia_actual);
            //    proceso_rehabilitacion(3,dia_proceso,dia_actual);
            //    //this.Close();
            //}            
        }

        public void reproceso(string fecha, string repros)
        {
            if (repros.Substring(0, 1) == "0")
                carga_circulares(fecha.Substring(6,2)+fecha.Substring(4,2)+fecha.Substring(2,2), true);
            if (repros.Substring(1, 1) == "0")
                carga_tablas(fecha,obtiene_proximo(fecha),true);
            if (repros.Substring(2, 1) == "0")
                proceso_clausura(1, fecha, obtiene_proximo(fecha));
            if (repros.Substring(3, 1) == "0")
                proceso_rehabilitacion(1, fecha, obtiene_proximo(fecha));
        }                            

        private void cargaDeTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Uta noooo");
            panel_rep.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel_rep.Visible = false;
        }

        private int verifica_reproceso(string re,string fecha)
        {
            int ok = 1;

            SqlCommand cmdp = new SqlCommand("sp_reproceso", cone);
            cmdp.CommandType = CommandType.StoredProcedure;
            cmdp.Parameters.Clear();
            SqlParameter p = new SqlParameter("@repros", SqlDbType.VarChar);
            p.Direction = ParameterDirection.Input;
            p.Value = re;
            cmdp.Parameters.Add(p);
            SqlParameter fe = new SqlParameter("@fecha", SqlDbType.VarChar);
            fe.Direction = ParameterDirection.Input;
            fe.Value = fecha;
            cmdp.Parameters.Add(fe);
            SqlParameter resp = new SqlParameter("@resultado", SqlDbType.Int);
            resp.Direction = ParameterDirection.Output;
            cmdp.Parameters.Add(resp);
            
            cone.Open();
           
            cmdp.ExecuteNonQuery();
            ok = (int)cmdp.Parameters["@resultado"].Value;
            cone.Close();

            return ok;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string repros = "";
            string fecha = "";
            if(cbcc.Checked)
                repros += "0";
            else
                repros += "1";
            if (cbct.Checked)
                repros += "0";
            else
                repros += "1";
            if (cbpc.Checked)
                repros += "0";
            else
                repros += "1";
            if (cbpr.Checked)
                repros += "0";
            else
                repros += "1";

            if (repros == "1111")
                MessageBox.Show("Debe seleccionar al menos una actividad para el reproceso", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                fecha = dateTimePicker1.Value.ToString("yyyyMMdd");
                if (fecha != "")
                {
                    int sw_rep = verifica_reproceso(repros,fecha);
                    if (sw_rep == 9)
                        MessageBox.Show("La fecha no esta habilitada para que se realice el reproceso.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    else
                    {
                        if (sw_rep == 1)
                            MessageBox.Show("Se ingresaron procesos que no se encuentran habilitados, verifique y vuelva a intentar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            try
                            {
                                reproceso(fecha, repros);                                
                            }
                            catch (SqlException err)
                            {
                                MessageBox.Show("No se realizó el reproceso. Descripción del error: " + err.Message);
                            }
                            finally
                            {
                                cone.Close();
                            }
                            panel_rep.Visible = false;
                        }
                    }                        
                }
                else
                    MessageBox.Show("Debe seleccionar la fecha de la que se realizará el reproceso","Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            proceso_clausura(3, dia_proceso, dia_actual);
            //proceso_rehabilitacion(1, dia_proceso, dia_actual);
        }

        private void mensualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reportes rep = new Reportes(1);
            rep.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            carga_circulares(dia_de_proceso,false);
        }

        public void carga_circulares(string dia, Boolean repro)
        {
            if (repro)
                ingreso.logs("RCC-00", Program.usuario, 0);
            else
                ingreso.logs("CRC-00", Program.usuario, 0);
            //obtiene la cadena de destinatarios para el proceso de carga de circulares
            obtiene_destinatarios(1);

            DateTime hora = DateTime.Now; //hora de inicio del procedimiento
            hora_inicio_proc = hora.ToString("HH:mm:ss");
            string CC, CR, CT, infclau = "", infrehab = "", infpres = "";
            //Nombre de las circulares diarias
            CC = "CC" + dia; //CCddMMyy
            CR = "CR" + dia; //CRddMMyy
            CT = "CT" + dia; //CTddMMyy
            //declaracion de variable de salida, estado del procedimiento 
            int sw_salida = 1, sw_procedimiento = 1, vuelta = 0;
            //asignacion de las variables sw de las circulares inicialmente como "No cargadas"
            sw_CC = 1; sw_CR = 1; sw_CT = 1;

            do
            {
                ingreso.logs("CRC-10", Program.usuario, 0);
                informe = "<center><b>Proceso de Carga de Circulares</b></center><HR>";
                informe += "<br>Fecha de Proceso: " + dia.Substring(0, 2) + "." + dia.Substring(2, 2) + "." + dia.Substring(4, 2);
                if (sw_CC == 0)
                    infclau = "<br>Se cargó la cricular de la fecha de proceso anteriormente";
                else
                {
                    if (existe_c(CC))
                        //carga_c es el procedimiento que permite cargar los registros de la circular enviada en formato txt a la Base de Datos
                        infclau = carga_c(CC, 1);
                    else
                        infclau = "<br>No se encontró la Circular de Clausura correspondiente a la fecha.";
                }
                if (sw_CR == 0)
                    infrehab = "<br>Se cargó la cricular de la fecha de proceso anteriormente";
                else
                {
                    if (existe_c(CR))
                        infrehab = carga_c(CR, 2);
                    else
                        infrehab = "<br>No se encontró la Circular de Rehabilitación correspondiente a la fecha.";
                }
                if (sw_CT == 0)
                    infpres = "<br>Se cargó la cricular de la fecha de proceso anteriormente";
                else
                {
                    if (existe_c(CT))
                        infpres = carga_c(CT, 3);
                    else
                        infpres = "<br>No se encontró la Circular de Rehabilitación por Prescripción correspondiente a la fecha.";
                }
                informe = informe + "<br><br><b>Circular de Clausura " + CC + ":</b>" + infclau + "<br><br><b>Circular de Rehabilitación " + CR + ":</b>" + infrehab + "<br><br><b>Circular de Rehabilitación por Prescripción " + CT + ":</b>" + infpres;


                if (infclau == "<br>No se encontró la Circular de Clausura correspondiente a la fecha.")
                {
                    sw_CC = 0;
                }

                if (infrehab == "<br>No se encontró la Circular de Rehabilitación correspondiente a la fecha.")
                {
                    sw_CR = 0;
                }

                if (infpres == "<br>No se encontró la Circular de Rehabilitación por Prescripción correspondiente a la fecha.")
                {
                    sw_CT = 0;
                }
                //Comprueba que se hayan cargado todas las circulares
                if (sw_CC == 0 && sw_CR == 0 && sw_CT == 0)
                {
                    //sw_ procedimiento = 0, marca la carga exitosa
                    sw_procedimiento = 0;
                    ingreso.logs("CRC-21", Program.usuario, 0);
                }
                else
                {
                    ingreso.logs("CRC-22", Program.usuario, 1);
                    if (repro)
                    {                        
                        sw_salida = 0;
                    }
                    else
                    {
                        //Si no se cargaron las circulares verifica que la hora actual sea menor a la final del proceso 
                        //string d1 = destinatarios1(1);
                        vuelta++;
                        reintento(1);

                        TimeSpan horafin_bd = TimeSpan.Parse(hora_fin);
                        DateTime horaf = DateTime.Now.AddMinutes(retardo_bd);
                        string horafin = horaf.ToString("HH:mm");
                        DateTime hora_par = DateTime.Now;
                        string hora_parcial = hora_par.ToString("HH:mm:ss");
                        TimeSpan horafin_sist = TimeSpan.Parse(horafin);
                        if (horafin_sist <= horafin_bd)
                        {
                            informe = informe + "<br><br>El proceso se realizo con fallas en la vuelta " + vuelta + " por tanto se volverá a intentar en " + retardo_bd + " minutos.<br><br><b>Estados temporales de las circulares:</b><br>Circular de Clausura = " + sw_CC + "<br>Circular de Rehabilitación = " + sw_CR + "<br>Circular de Rehabilitación por Prescripción = " + sw_CT + ".";
                            retardo_bd = retardo_bd * 60 * 1000 - 500;
                            añadir_detalleproceso(dia_actual, dia_proceso, 1, "CARGA DE CIRCULARES", hora_inicio_proc, hora_parcial, sw_procedimiento, usuario, informe, "", sw_CC, sw_CR, sw_CT, 1);
                            abrir_outlook();
                            EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Carga de Circulares de la ASFI", informe, desti_fallos);
                            cerrar_outlook();
                            //correo.correo_informe(informe, "Fallo en la Carga de Circulares de la ASFI", d1);                        
                            //MessageBox.Show("err...." + informe);
                            lblthread.Text = "Pantalla Congelada... esperando tiempo retardo del Procedimiento de Carga de Circulares.";
                            MessageBoxTemporal.Show(informe, "Error:", 0, true);
                            Thread.Sleep(retardo_bd);
                            lblthread.Text = "";
                        }
                        else
                        {
                            informe = informe + "<br><br>El tiempo límite para el proceso de Carga de Circulares fue superado por tanto se finalizó el proceso (con fallas durante su ejecución).";
                            sw_salida = 0;
                        }
                    }
                }

            } while (sw_salida == 1 && sw_procedimiento == 1);

            DateTime hora2 = DateTime.Now; //hora final del procedimiento
            hora_fin_proc = hora2.ToString("HH:mm:ss");
            string fechap = "20" + dia.Substring(4, 2) + dia.Substring(2, 2) + dia.Substring(0, 2);
            
            abrir_outlook();
            if (repro)
            {
                informe += "<br>Finalizó el Reproceso.<br><br><b>Estados finales:</b><br>Circular de Clausura = " + sw_CC + "<br>Circular de Rehabilitación = " + sw_CR + "<br>Circular de Rehabilitación por Prescripción = " + sw_CT + ".";
                añadir_detalleproceso(dia_actual, dia_proceso, 1, "REPROCESO - CARGA DE CIRCULARES", hora_inicio_proc, hora_fin_proc, sw_procedimiento, usuario, informe, "", sw_CC, sw_CR, sw_CT, 1);

                if (sw_procedimiento == 0)
                {
                    if (desti_exitos.Contains("@"))
                        EnviaCorreo.Enviar_SinArchivos("SCR: Reproceso Exitoso - Carga de Circulares de la ASFI" + dia, informe, desti_exitos);
                    else
                        MessageBox.Show("El reproceso de Carga de Circulares se ejecutó con exito.");
                     
                    //correo.correo_informe(informe, "Carga de Circulares de la ASFI exitosa", d1);
                    //MessageBox.Show("exitoso....." + informe);
                    ingreso.logs("CRC-51", Program.usuario, 0);
                }
                else
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Reproceso Fallido - Carga de Circulares de la ASFI " + dia, informe, desti_fallos);                   
                    ingreso.logs("CRC-52", Program.usuario, 0);
                }
                ingreso.logs("RCC-01", Program.usuario, 0);
                
            }
            else
            {
                informe += "<br>Finalizó el proceso.<br><br><b>Estados finales:</b><br>Circular de Clausura = " + sw_CC + "<br>Circular de Rehabilitación = " + sw_CR + "<br>Circular de Rehabilitación por Prescripción = " + sw_CT + ".";
                añadir_detalleproceso(dia_actual, dia_proceso, 1, "CARGA DE CIRCULARES", hora_inicio_proc, hora_fin_proc, sw_procedimiento, usuario, informe, "", sw_CC, sw_CR, sw_CT, 1);

                if (sw_procedimiento == 0)
                {
                    if (desti_exitos.Contains("@"))
                        EnviaCorreo.Enviar_SinArchivos("SCR: Exitoso - Carga de Circulares de la ASFI", informe, desti_exitos);
                    
                    //correo.correo_informe(informe, "Carga de Circulares de la ASFI exitosa", d1);
                    //MessageBox.Show("exitoso....." + informe);
                    ingreso.logs("CRC-41", Program.usuario, 0);
                }
                else
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Carga de Circulares de la ASFI", informe, desti_fallos);
                    //correo.correo_informe(informe, "Fallo en la Carga de Circulares de la ASFI", d1);
                    //MessageBox.Show("fallido....." + informe);
                    ingreso.logs("CRC-42", Program.usuario, 0);
                }
                ingreso.logs("CRC-30", Program.usuario, 0);
            }
            cerrar_outlook();

           
        }

        public void reintento (int num_proc)
        {
            SqlCommand cmdr = new SqlCommand("sp_reintento", cone);
            cmdr.CommandType = CommandType.StoredProcedure;
            SqlParameter numero = new SqlParameter("@num", SqlDbType.Int);
            numero.Direction = ParameterDirection.Input;
            numero.Value = num_proc;
            cmdr.Parameters.Add(numero);
            SqlParameter hora_bd = cmdr.Parameters.Add("@hora", SqlDbType.VarChar, 5);
            hora_bd.Direction = ParameterDirection.Output;
            SqlParameter ret_bd = cmdr.Parameters.Add("@retardo", SqlDbType.Int);
            ret_bd.Direction = ParameterDirection.Output;
            cone.Open();
            cmdr.ExecuteNonQuery();
            hora_fin = (string)cmdr.Parameters["@hora"].Value;
            retardo_bd = (int)cmdr.Parameters["@retardo"].Value;
            cone.Close();
        }

        public void añadir_detalleproceso(string d_act, string f_proc, int num_proc, string proc, string h_ini_proc, string h_fin_proc, int sw_proc, string usua, string inf, string obs, int c1, int c2, int c3, int c4)
        {
            SqlCommand cmin = new SqlCommand("[sp_ingresar_detalle]", cone);
            cmin.CommandType = CommandType.StoredProcedure;
            cmin.Parameters.AddWithValue("fecha_ejecucion", d_act);
            cmin.Parameters.AddWithValue("fecha_proceso", f_proc);
            cmin.Parameters.AddWithValue("num_proceso", num_proc);
            cmin.Parameters.AddWithValue("proceso", proc);
            cmin.Parameters.AddWithValue("hora_inicio", h_ini_proc);
            cmin.Parameters.AddWithValue("hora_fin", h_fin_proc);
            cmin.Parameters.AddWithValue("estado_final", sw_proc);
            cmin.Parameters.AddWithValue("usuario", usua);
            cmin.Parameters.AddWithValue("informe", inf);
            cmin.Parameters.AddWithValue("observacion", obs);
            cmin.Parameters.AddWithValue("campo1", c1);
            cmin.Parameters.AddWithValue("campo2", c2);
            cmin.Parameters.AddWithValue("campo3", c3);
            cmin.Parameters.AddWithValue("campo4", c4);

            try
            {
                cone.Open();
                cmin.ExecuteNonQuery();
                //MessageBox.Show("termino el proceso y se actualizo la tabla");
            }
            catch (SqlException exq)
            {
                informe += "<br><br>Se produjo un error al actualizar la tabla de procesos.<br><br>Descripción del error:<br>" + exq.Message;
            }
            finally
            {
                cone.Close();
            }
        }

        private void lblfechaproceso_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DSFDSF");
        }

        //Retorna falso o verdadero, s existe una circular
        public Boolean existe_c(string circular)
        {
            string nombre_circular = circular+".txt"; //Nombre que tiene la circular del día          
            //Obtenemos las rutas de las circulares del archivo de configuración
            string origen = ConfigurationManager.AppSettings["ruta_origen"];           
            string destino = ConfigurationManager.AppSettings["ruta_destino_temporal"];
            string origenarchivo = System.IO.Path.Combine(origen, nombre_circular);
            string destinoarchivo = System.IO.Path.Combine(destino, nombre_circular);
            //crea el directorio si no existe
            if (!System.IO.Directory.Exists(destino))
                System.IO.Directory.CreateDirectory(destino);
            //Verifica que exista la circular
            if (System.IO.File.Exists(origenarchivo))
            {
                if (System.IO.File.Exists(destinoarchivo))
                    System.IO.File.Delete(destinoarchivo);

                System.IO.File.Copy(origenarchivo, destinoarchivo); //Mueve la circular al destino temporal
                return true;
            }
            else
                return false;
        }

        private void repro_Click(object sender, EventArgs e)
        {

        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        public string   carga_c(string circular, int cod_c)
        {           
            string sp_carga;
            string fecha_circ = "20" + circular.Substring(6, 2) + circular.Substring(4, 2) + circular.Substring(2, 2);
           
            int sw_proc = 1;
            string inf_carga = "";


            if (cod_c == 1)            
                sp_carga = "sp_carga_cc";            
            else
            {
                if (cod_c == 2)
                    sp_carga = "sp_carga_cr";               
                else                                   
                    sp_carga = "sp_carga_ct";                
            }
            
            string destino = ConfigurationManager.AppSettings["ruta_destino_temporal"];    
            try
            {
                SqlCommand cmdloc = new SqlCommand();
                cmdloc.CommandText = "sp_carga_circular_local";
                cmdloc.CommandType = CommandType.StoredProcedure;
                cmdloc.Connection = cone4;
                cmdloc.Parameters.Clear();
                SqlParameter circu = new SqlParameter("@circular", SqlDbType.VarChar, 10);
                circu.Direction = ParameterDirection.Input;
                circu.Value = circular;
                cmdloc.Parameters.Add(circu);
                SqlParameter ruta = new SqlParameter("@ruta", SqlDbType.VarChar, 300);
                ruta.Direction = ParameterDirection.Input;
                ruta.Value = destino;
                cmdloc.Parameters.Add(ruta);

                cone4.Open();
                cmdloc.ExecuteNonQuery();
                sw_proc = 0;     
            }
            catch (SqlException exc)
            {
                MessageBox.Show(exc.Message);
                sw_proc = 1;
                inf_carga = "Ocurrieron fallas al cargar la circular " + circular + ".<br>Descripción del error: "+exc.Message;
                //EnviaCorreo.Enviar_SinArchivos("Error al Cargar la Circular de Clausura de la ASFI", "Se produjo un error al momento de cargar la circular " + circular + ".<br><br>Descripción del error: <br>" + exc.Message + ".\n Repare los errores y ejecute un reproceso.", desti_fallos);
            }
            finally
            { cone4.Close(); }


            if (sw_proc == 0)
            {
                try
                {
                    cone4.Open();
                    string Query = "select * from TBLTEMP0_" + circular.Substring(0, 2);
                    DataTable dt = new DataTable();
                    
                    
                    
                    SqlCommand cmd = new SqlCommand(Query, cone4);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);



                    da.Fill(dt);
                   
                    SqlCommand ins = new SqlCommand(sp_carga, cone);
                    ins.CommandType = CommandType.StoredProcedure;
                    SqlParameter fech = new SqlParameter("@fecha", SqlDbType.VarChar, 10);
                    fech.Direction = ParameterDirection.Input;
                    fech.Value = fecha_circ;
                    ins.Parameters.Add(fech);
                    SqlParameter tab = ins.Parameters.AddWithValue("@cx", dt);
                    tab.SqlDbType = SqlDbType.Structured;
                    SqlParameter cod = ins.Parameters.Add("@cod", SqlDbType.Int);
                    cod.Direction = ParameterDirection.Output;
                    SqlParameter inf = ins.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                    inf.Direction = ParameterDirection.Output;
                    cone.Open();
                    ins.ExecuteNonQuery();
                    sw_proc = (int)ins.Parameters["@cod"].Value;
                    inf_carga = (string)ins.Parameters["@informe"].Value;
                }
                catch (SqlException exc)
                {
                    sw_proc = 1;
                    inf_carga = "Ocurrieron fallas al cargar la circular " + circular + ".<br>Descripción del error: " + exc.Message;
                    //EnviaCorreo.Enviar_SinArchivos("Error al Cargar la Circular de Clausura.", "Se produjo un error al intentar los datos de la circular a la tabla temporal.<br><br>Descripción del error:<br>" + exc.Message, desti_fallos);
                }
                finally
                {
                    cone.Close();
                    cone4.Close();
                }

            }
            
            if (sw_proc == 0)
            {            
                string destino_final = ConfigurationManager.AppSettings["destino_final_procesados"];
                string destinofinalarchivo = System.IO.Path.Combine(destino_final, circular+".txt");
                if (System.IO.File.Exists(destinofinalarchivo))
                    System.IO.File.Delete(destinofinalarchivo);
                System.IO.File.Move(destino + "\\" + circular + ".txt", destinofinalarchivo);
                inf_carga += "<br>Se validó correctamente la circular, se copiaron sus registros y se movió la circular a la carpeta de procesados.<br>";               
            }
            else
            {
                string destino_final = ConfigurationManager.AppSettings["destino_final_rechazados"];
                string destinofinalarchivo = System.IO.Path.Combine(destino_final, circular+".txt");
                if (System.IO.File.Exists(destinofinalarchivo))
                    System.IO.File.Delete(destinofinalarchivo);
                System.IO.File.Move(destino + "\\" + circular + ".txt", destinofinalarchivo);
                inf_carga += "<br>No se validó la circular y por tanto se movió a la carpeta de rechazados.<br>";                
            }
            if (cod_c == 1)
            {
                sw_CC = sw_proc;
                ingreso.logs("CRC-01", Program.usuario, sw_proc);
            }
            else
            {
                if (cod_c == 2)
                {
                    sw_CR = sw_proc;
                    ingreso.logs("CRC-02", Program.usuario, sw_proc);
                }
                else
                {
                    sw_CT = sw_proc;
                    ingreso.logs("CRC-03", Program.usuario, sw_proc);
                }
            }
            return inf_carga;
        }
        
        public void carga_tablas(string fecha_1, string fecha_2, Boolean repro)
        {
            if (repro)
                ingreso.logs("RTB-00", Program.usuario, 0);
            else
                ingreso.logs("TBC-00", Program.usuario, 0);

            flag_error = 1;
            int sw_procedimiento = 1,cant;
            DateTime hora = DateTime.Now;
            hora_inicio_proc = hora.ToString("HH:mm:ss");
            string infor = "";
            //Obtiene la cadena de destiatarios para el proceso de carga de tablas del Banco
            obtiene_destinatarios(2);

            string vst_journal = ConfigurationManager.AppSettings["vista_journal"];
            string TBLCTAPAS = ConfigurationManager.AppSettings["vista_pasivas"];
            string TBLDBC = ConfigurationManager.AppSettings["vista_clientes"];
            string TBLRCO = ConfigurationManager.AppSettings["vista_relacion"];
            

            sw_journal = 1; sw_TBLCTAPAS = 1; sw_TBLDBC = 1; sw_TBLRCO = 1; sw_TBLCTAPAS_HIST = 1; informe = "";
                    
            int vuelta = 0;
            do
            {
                ingreso.logs("TBC-10", Program.usuario, 0);
                informe = "<br><center><b>Proceso de Carga de Tablas del Banco</b></center><hr>";
                informe += "<b>Fecha de Proceso: " + fecha_1.Substring(6, 2) + "." + fecha_1.Substring(4, 2) + "." + fecha_1.Substring(2, 2);
                informe += "<br>Fecha de Ejecución: " + fecha_2.Substring(6,2) + "." + fecha_2.Substring(4, 2) + "." + fecha_2.Substring(2, 2)+"</b>";
                informe += "<br><br><b>Reporte sobre la carga de la tabla Journalglobal:</b>";

                if (sw_journal == 0)
                    informe += "Se cargó la tabla anteriormente";
                else
                {
                    if (vuelta > 1 || repro)
                    {
                        SqlCommand cmdb = new SqlCommand("sp_borra_journal", cone);
                        cmdb.CommandType = CommandType.StoredProcedure;
                        SqlParameter fe1 = cmdb.Parameters.Add("@fecha1", SqlDbType.VarChar, 10);
                        fe1.Direction = ParameterDirection.Input;
                        fe1.Value = fecha_1;
                        SqlParameter fe2 = cmdb.Parameters.Add("@fecha2", SqlDbType.VarChar, 10);
                        fe2.Direction = ParameterDirection.Input;
                        fe2.Value = fecha_2;
                        cone.Open();
                        cmdb.ExecuteNonQuery();
                        cone.Close();
                    }



                    //dia_proceso "yyyyMMdd". dia_actual "yyyyMMdd"                    
                    //dia de proceso que lo definimos como constante para pruebas y la query debe reemplazar por la de arriba al momento de la implementacion
                    //dia_proceso = "20160711";
                    try
                    {
                        cone2.Open();
                        int flg_journal = 0;
                        string q_journal = "select count(1) from " + vst_journal + " where (SysDate between '" + fecha_1 + "' and '" + fecha_2 + "') and TxnType != '7' and TxnStatus in ('1','10')";
                        SqlCommand cmdj = new SqlCommand(q_journal, cone2);
                        cmdj.CommandTimeout = 111120000;
                        if ((Convert.ToInt32(cmdj.ExecuteScalar())) > 0)
                        {
                            flg_journal = 1;                            
                        }
                        cone2.Close();

                        if (flg_journal == 1)
                        {
                            //string Query = "select OwnerID,SysDate,AcctNbr,SysTime,CurrCode,UserID,WorkstationID,AnswerHub,General,Mnemonic,SucAge,Cajero from "+vst_journal+" where (SysDate between '" + dia_proceso + "' and '" + dia_actual + "') and Causal = 'CLS' and (Mnemonic = 'DEBL' or Mnemonic = 'BLOK') and TxnType != '7' and TxnStatus in ('1','10')";
                            string Query = "select OwnerID,SysDate,AcctNbr,SysTime,CurrCode,UserID,WorkstationID,AnswerHub,General,Mnemonic,SucAge,Cajero from " + vst_journal + " where (SysDate between '" + fecha_1 + "' and '" + fecha_2 + "') and Causal = 'CLS' and (Mnemonic = 'DEBL' or Mnemonic = 'BLOK') and TxnType != '7' and TxnStatus in ('1','10')";
                            cone2.Open();
                            DataTable dt = new DataTable();
                            SqlCommand cmd = new SqlCommand(Query, cone2);
                            cmd.CommandTimeout = 111120000;

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            cant = dt.Rows.Count;
                            if (cant > 0)
                            {
                                SqlCommand ins = new SqlCommand("sp_carga_journal", cone);
                                ins.CommandType = CommandType.StoredProcedure;
                                SqlParameter f1 = new SqlParameter("@fecha1", SqlDbType.VarChar, 10);
                                f1.Direction = ParameterDirection.Input;
                                if (repro)
                                {
                                    f1.Value = fecha_1;
                                }
                                else
                                {
                                    f1.Value = dia_proceso;
                                }                                
                                ins.Parameters.Add(f1);
                                SqlParameter f2 = new SqlParameter("@fecha2", SqlDbType.VarChar, 10);
                                f2.Direction = ParameterDirection.Input;

                                if (repro)
                                {
                                    f2.Value = fecha_2;
                                }
                                else
                                {
                                    f2.Value = dia_actual;
                                }
                                
                                ins.Parameters.Add(f2);
                                SqlParameter cant1 = new SqlParameter("@cant", SqlDbType.Int);
                                cant1.Direction = ParameterDirection.Input;
                                cant1.Value = cant;
                                ins.Parameters.Add(cant1);
                                SqlParameter tab = ins.Parameters.AddWithValue("@journal", dt);
                                tab.SqlDbType = SqlDbType.Structured;
                                SqlParameter cod = ins.Parameters.Add("@cod", SqlDbType.Int);
                                cod.Direction = ParameterDirection.Output;
                                SqlParameter inf = ins.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                                inf.Direction = ParameterDirection.Output;

                                cone.Open();
                                ins.CommandTimeout = 0;
                                ins.ExecuteNonQuery();
                                sw_journal = (int)ins.Parameters["@cod"].Value;
                                infor = (string)ins.Parameters["@informe"].Value;
                            }
                            else
                            {
                                sw_journal = 0;
                                infor = "<br>No se encontraron registros de Clausura o Rehabilitación en la tabla Journal del Warehouse de la fecha de proceso.";
                            }
                        }
                        else
                        {
                            sw_journal = 0;
                            infor = "<br>No se encontró ningún registros en la tabla Journal del Warehouse de la fecha de proceso.";
                        }
                    }
                    catch (SqlException ej)
                    {
                        sw_journal = 1;
                        infor = "<br>No se pudo cargar la tabla. Descripción del error:<br>" + ej.Message;
                    }
                    finally
                    {
                        cone.Close();
                        cone2.Close();
                    }

                    if (sw_journal == 1)
                        flag_error = 0;
                    informe += infor;
                }

                informe += "<br><br><b>Reporte sobre la carga de la tabla TBLCTAPAS:</b>";
                if (flag_error == 1)
                {
                    if (sw_TBLCTAPAS == 0)
                        informe += "Se cargó la tabla anteriormente";
                    else
                    {
                        try
                        {
                            ingreso.logs("TBC-12", Program.usuario, 0);
                            string Query = "select CP_FECEXTRACCION, CP_APLICATIVO,	CP_NROCUENTA, CP_CIC, CP_TIPCUENTA,	CP_NOMTIPCTA, CP_SITUACION, CP_NOMTIPSITUAC, CP_FECAPERTU, CP_FECCANCELAC, CP_FECULTMOV, CP_SLDACTUAL ";
                            Query = Query + "from " + TBLCTAPAS;
                            Query = Query + " where CP_APLICATIVO = 'CTE' and CP_SITUACION != '03'";

                            cone2.Open();
                            DataTable dt = new DataTable();
                            SqlCommand cmd = new SqlCommand(Query, cone2);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            cant = dt.Rows.Count;
                            contador = cant;

                            SqlCommand ins = new SqlCommand("sp_carga_tblctapas", cone);
                            ins.CommandType = CommandType.StoredProcedure;
                            SqlParameter cant1 = new SqlParameter("@cant", SqlDbType.Int);
                            cant1.Direction = ParameterDirection.Input;
                            cant1.Value = cant;
                            ins.Parameters.Add(cant1);
                            SqlParameter tab = ins.Parameters.AddWithValue("@TBLCTAPAS_RE", dt);
                            tab.SqlDbType = SqlDbType.Structured;
                            SqlParameter cod = ins.Parameters.Add("@cod", SqlDbType.Int);
                            cod.Direction = ParameterDirection.Output;
                            SqlParameter inf = ins.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                            inf.Direction = ParameterDirection.Output;
                          
                            cone.Open();
                            ins.CommandTimeout = 0;
                            ins.ExecuteNonQuery();
                            sw_TBLCTAPAS = (int)ins.Parameters["@cod"].Value;
                            infor = (string)ins.Parameters["@informe"].Value;

                            ingreso.logs("TBC-17", Program.usuario, 0);
                        }
                        catch (SqlException ej)
                        {
                            sw_TBLCTAPAS = 1;
                            infor = "<br>No se pudo cargar la tabla. Descripción del error:<br>" + ej.Message;
                            ingreso.logs("TBC-32", Program.usuario, 0);
                        }
                        finally
                        {
                            cone.Close();
                            cone2.Close();
                        }
                        if (sw_TBLCTAPAS == 1)
                            flag_error = 0;
                        informe += infor;
                    }
                }
                else
                    informe += "<br>No se pudo cargar la tabla debido a que se produjeron errores de carga anteriores.";

                informe += "<br><br><b>Reporte sobre la carga de la tabla TBLCTAPAS_HIST:</b>";
                if (flag_error == 1)
                {
                    if (sw_TBLCTAPAS_HIST == 0)
                        informe += "<br>Se cargó la tabla anteriormente";
                    else
                    {
                        try
                        {
                            ingreso.logs("TBC-15", Program.usuario, 0);
                            SqlCommand cmdt = new SqlCommand("sp_carga_tblctapas_hist", cone);
                            cmdt.CommandType = CommandType.StoredProcedure;
                            SqlParameter cant1 = cmdt.Parameters.Add("@cant", SqlDbType.Int);
                            cant1.Direction = ParameterDirection.Input;
                            cant1.Value = contador;
                            SqlParameter cod = cmdt.Parameters.Add("@cod", SqlDbType.Int);
                            cod.Direction = ParameterDirection.Output;
                            SqlParameter inf = cmdt.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                            inf.Direction = ParameterDirection.Output;                            
                            
                            cone.Open();
                            cmdt.CommandTimeout = 0;
                            cmdt.ExecuteNonQuery();
                            sw_TBLCTAPAS_HIST = (int)cmdt.Parameters["@cod"].Value;
                            infor = (string)cmdt.Parameters["@informe"].Value;

                            ingreso.logs("TBC-20", Program.usuario, 0);
                        }
                        catch (SqlException ej)
                        {
                            sw_TBLCTAPAS_HIST = 1;
                            infor = "<br>No se pudo cargar la tabla. Descripción del error:<br>" + ej.Message;
                            ingreso.logs("TBC-35", Program.usuario, 0);
                        }
                        finally
                        {
                            cone.Close();
                        }
                        if (sw_TBLCTAPAS_HIST > 0)
                            flag_error = 0;
                        informe += infor;
                    }
                }
                else
                    informe += "<br>No se pudo cargar la tabla debido a que se produjeron errores de carga anteriores.";

                informe += "<br><br><b>Reporte sobre la carga de la tabla TBLDBC:</b>";
                if (flag_error == 1)
                {
                    if (sw_TBLDBC == 0)
                        informe += "<br>Se cargó la tabla anteriormente";
                    else
                    {
                        try
                        {
                            ingreso.logs("TBC-13", Program.usuario, 0);
                            string Query = "select CL_FECEXTRACCION,CL_CIC,CL_PATCLIENTE,CL_MATCLIENTE,CL_NOMBRECLIENTE,CL_NOMCLIENTE,CL_NOMCOMERCIAL,CL_NRODIRECCIONES,CL_FEC01,CL_TIPIDC,CL_EXTIDC,CL_IDC,CL_RUC,CL_TIPPERSONA,CL_TIPCLIENTE,CL_FUNNEGOCIOS,CL_TIPBANCA";
                            //Query = Query + " from " + TBLDBC;
                            //Query = Query + " where CL_CIC IN (SELECT DISTINCT CP_CIC from " + TBLCTAPAS + " where CP_APLICATIVO = 'CTE' and CP_SITUACION != '03')";

                            Query = Query + " from "+ TBLDBC;
                            Query = Query + " where CL_CIC in (select distinct CL_CIC from "+ TBLRCO;
                            Query = Query + " where CL_OPERACION in (select substring(CP_NROCUENTA,6,3)+'C'+right(CP_NROCUENTA,16)";
                            Query = Query + " from " + TBLCTAPAS + " where CP_APLICATIVO = 'CTE' and CP_SITUACION !='03'))";

                            cone2.Open();
                            DataTable dt = new DataTable();
                            SqlCommand cmd = new SqlCommand(Query, cone2);
                            cmd.CommandTimeout = 5000;
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            cant = dt.Rows.Count;
                        
                            SqlCommand cmdt = new SqlCommand("sp_carga_tbldbc", cone);
                            cmdt.CommandType = CommandType.StoredProcedure;
                            SqlParameter cant1 = new SqlParameter("@cant", SqlDbType.Int);
                            cant1.Direction = ParameterDirection.Input;
                            cant1.Value = cant;
                            cmdt.Parameters.Add(cant1);
                            SqlParameter tab = cmdt.Parameters.AddWithValue("@TBLDBC_RE", dt);
                            tab.SqlDbType = SqlDbType.Structured;
                            SqlParameter cod = cmdt.Parameters.Add("@cod", SqlDbType.Int);
                            cod.Direction = ParameterDirection.Output;
                            SqlParameter inf = cmdt.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                            inf.Direction = ParameterDirection.Output;
                       
                            cone.Open();
                            cmdt.CommandTimeout = 0;
                            cmdt.ExecuteNonQuery();
                            sw_TBLDBC = (int)cmdt.Parameters["@cod"].Value;
                            infor = (string)cmdt.Parameters["@informe"].Value;

                            if (sw_TBLDBC == 0)
                            {
                                Query = "select CL_FECEXTRACCION,CL_CIC,CL_PATCLIENTE,CL_MATCLIENTE,CL_NOMBRECLIENTE,CL_NOMCLIENTE,CL_NOMCOMERCIAL,CL_NRODIRECCIONES,CL_FEC01,CL_TIPIDC,CL_EXTIDC,CL_IDC,CL_RUC,CL_TIPPERSONA,CL_TIPCLIENTE,CL_FUNNEGOCIOS,CL_TIPBANCA";
                                Query = Query + " from " + TBLDBC;
                                Query = Query + " where CL_CIC IN (SELECT DISTINCT CP_CIC from " + TBLCTAPAS + " where CP_APLICATIVO = 'CTE' and CP_SITUACION != '03')";

                                dt = new DataTable();
                                cmd = new SqlCommand(Query, cone2);
                                cmd.CommandTimeout = 5000;
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dt);
                                cant = dt.Rows.Count;

                                cmdt = new SqlCommand("sp_carga_tbldbc2", cone);
                                cmdt.CommandType = CommandType.StoredProcedure;
                                cant1 = new SqlParameter("@cant", SqlDbType.Int);
                                cant1.Direction = ParameterDirection.Input;
                                cant1.Value = cant;
                                cmdt.Parameters.Add(cant1);
                                tab = cmdt.Parameters.AddWithValue("@TBLDBC_RE", dt);
                                tab.SqlDbType = SqlDbType.Structured;
                                cod = cmdt.Parameters.Add("@cod", SqlDbType.Int);
                                cod.Direction = ParameterDirection.Output;
                                inf = cmdt.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                                inf.Direction = ParameterDirection.Output;

                                cmdt.ExecuteNonQuery();
                                sw_TBLDBC = (int)cmdt.Parameters["@cod"].Value;
                                infor = (string)cmdt.Parameters["@informe"].Value;

                                ingreso.logs("TBC-18", Program.usuario, 0);
                            }

                            
                        }
                        catch (SqlException ej)
                        {
                            sw_TBLDBC = 1;
                            infor = "<br>No se pudo cargar la tabla. Descripción del error:<br>" + ej.Message;
                            ingreso.logs("TBC-33", Program.usuario, 0);
                        }
                        finally
                        {
                            cone.Close();
                            cone2.Close();
                        }
                        if (sw_TBLDBC > 0)
                            flag_error = 0;
                        informe += infor;                     
                    }
                }
                else
                    informe += "<br>No se pudo cargar la tabla debido a que se produjeron errores de carga anteriores.";

                informe += "<br><br><b>Reporte sobre la carga de la tabla TBLRCO:</b>";
                if (flag_error == 1)
                {
                    if (sw_TBLRCO == 0)
                        informe += "<br>Se cargó la tabla anteriormente";
                    else
                    {
                        try
                        {
                            ingreso.logs("TBC-14", Program.usuario, 0);
                            string Query = "select CL_FECEXTRACCION, CL_CIC, CL_OPERACION, CL_TIPRCO, CL_FECTRANSACCION, CL_SITUACIONRCO, CL_TIPRELACIONRCO, CL_PORRELACION, CL_TIOAUX, CL_STAELIMINACION";
                            Query = Query + " from " + TBLRCO;
                            Query = Query + " where CL_OPERACION in (select distinct substring(CP_NROCUENTA,6,3)+'C'+RIGHT(CP_NROCUENTA,16)";
                            Query = Query + " from " + TBLCTAPAS + " where CP_APLICATIVO = 'CTE' AND CP_SITUACION != '03')";

                            cone2.Open();
                            DataTable dt = new DataTable();
                            SqlCommand cmd = new SqlCommand(Query, cone2);
                            cmd.CommandTimeout = 1000;
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            cant = dt.Rows.Count;

                            SqlCommand cmdt = new SqlCommand("sp_carga_tblrco", cone);
                            cmdt.CommandType = CommandType.StoredProcedure;
                            SqlParameter cant1 = new SqlParameter("@cant", SqlDbType.Int);
                            cant1.Direction = ParameterDirection.Input;
                            cant1.Value = cant;
                            cmdt.Parameters.Add(cant1);
                            SqlParameter tab = cmdt.Parameters.AddWithValue("@TBLRCO_RE", dt);
                            tab.SqlDbType = SqlDbType.Structured;
                            SqlParameter cod = cmdt.Parameters.Add("@cod", SqlDbType.Int);
                            cod.Direction = ParameterDirection.Output;
                            SqlParameter inf = cmdt.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                            inf.Direction = ParameterDirection.Output;
                                                        
                            cone.Open();
                            cmdt.ExecuteNonQuery();
                            cmdt.CommandTimeout = 0;
                            sw_TBLRCO = (int)cmdt.Parameters["@cod"].Value;
                            infor = (string)cmdt.Parameters["@informe"].Value;

                            ingreso.logs("TBC-19", Program.usuario, 0);
                        }
                        catch (SqlException ej)
                        {
                            sw_TBLRCO = 1;
                            infor = "<br>No se pudo cargar la tabla. Descripción del error:<br>" + ej.Message;
                            ingreso.logs("TBC-34", Program.usuario, 0);
                        }
                        finally
                        {
                            cone.Close();
                            cone2.Close();
                        }
                        if (sw_TBLRCO > 0)
                            flag_error = 0;
                        informe += infor;
                    }
                }
                else
                    informe += "<br>No se pudo cargar la tabla debido a que se produjeron errores de carga anteriores.";

                if (sw_journal + sw_TBLCTAPAS + sw_TBLDBC + sw_TBLRCO + sw_TBLCTAPAS_HIST == 0)
                {
                    //sw_procedimiento = 0, indica la carga de tablas del Banco Exitosa
                    sw_procedimiento = 0;
                    ingreso.logs("TBC-21", Program.usuario, 0);
                }
                else
                {
                    ingreso.logs("TBC-22", Program.usuario, 0);
                    if (!repro)
                    {
                        vuelta++;
                        reintento(2);   //nos devuelve la hora final y el retardo del proceso 2
                        //la hora final se almacena en la vaiable global "hora_fin" y el retardo en la variable global "retardo_bd"
                        TimeSpan horafin_bd = TimeSpan.Parse(hora_fin);
                        DateTime horaf = DateTime.Now.AddMinutes(retardo_bd - 1);
                        string horafin = horaf.ToString("HH:mm");
                        TimeSpan horafin_sist = TimeSpan.Parse(horafin);

                        if (horafin_sist <= horafin_bd)
                        {
                            informe += "<br><br>El proceso se realizo con fallas en la vuelta " + vuelta + " por tanto se volverá a intentar en " + retardo_bd + " minutos.<br><br><b>Estados temporales de las tablas:</b><br>Tabla Journal = " + sw_journal + "<br>Tabla TBLCTAPAS = " + sw_TBLCTAPAS + "<br>Tabla TBLCTAPAS_HIST = " + sw_TBLCTAPAS_HIST + "<br>Tabla TBLDBC = " + sw_TBLDBC + "<br>Tabla TBLRCO = " + sw_TBLRCO + ".";
                            retardo_bd = retardo_bd * 60 * 1000;
                            DateTime hora21 = DateTime.Now;
                            hora_fin_proc = hora21.ToString("HH:mm:ss");
                            añadir_detalleproceso(fecha_2, fecha_1, 2, "CARGA DE TABLAS DEL BANCO", hora_inicio_proc, hora_fin_proc, sw_procedimiento, usuario, informe, "", sw_journal, sw_TBLCTAPAS + sw_TBLCTAPAS_HIST, sw_TBLDBC, sw_TBLRCO);
                            abrir_outlook();
                            EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Carga de Tablas del Banco", informe, desti_fallos);
                            cerrar_outlook();
                            MessageBoxTemporal.Show(informe, "Error en la Carga de Tablas del Banco:", 0, true);
                            Thread.Sleep(retardo_bd);
                        }
                        else
                        {
                            informe = informe + "<br><br>El tiempo límite para el proceso de Carga de Tablas fue superado por tanto se finalizó el proceso con fallas durante su ejecución.";
                            flag_error = 0;
                        }
                    }
                    else
                    {
                        flag_error = 0;
                        MessageBox.Show("El Reproceso se realizo con fallas. Informe detallado:\n" + informe);
                    }
                }
                

            } while (flag_error == 1 && (sw_procedimiento == 1));
            
            DateTime hora2 = DateTime.Now;
            hora_fin_proc = hora2.ToString("HH:mm:ss");
            informe += "<br><br>Finalizó el proceso.<br><br><b>Estados finales:</b><br>Tabla Journal = " + sw_journal + "<br>Tabla TBLCTAPAS = " + sw_TBLCTAPAS + "<br>Tabla TBLCTAPAS_HIST = " + sw_TBLCTAPAS_HIST + "<br>Tabla TBLDBC = " + sw_TBLDBC + "<br>Tabla TBLRCO = " + sw_TBLRCO + ".";            
            
            abrir_outlook();
            if (repro)
            {
                añadir_detalleproceso(fecha_2, fecha_1, 2, "REPROCESO - CARGA DE TABLAS DEL BANCO ("+ fecha_1 +")", hora_inicio_proc, hora_fin_proc, sw_procedimiento, usuario, informe, "", sw_journal, sw_TBLCTAPAS + sw_TBLCTAPAS_HIST, sw_TBLDBC, sw_TBLRCO);
                if (sw_procedimiento == 0)
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Reproceso Exitoso - Carga de Tablas del Banco", informe, desti_exitos);
                    ingreso.logs("RTB-10", Program.usuario, 0);
                }
                else
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Reproceso Fallido - Carga de Tablas del Banco", informe, desti_fallos);
                    ingreso.logs("RTB-11", Program.usuario, 0);
                }                               
            }
            else
            {
                añadir_detalleproceso(fecha_2, fecha_1, 2, "CARGA DE TABLAS DEL BANCO (" + fecha_1 + ")", hora_inicio_proc, hora_fin_proc, sw_procedimiento, usuario, informe, "", sw_journal, sw_TBLCTAPAS + sw_TBLCTAPAS_HIST, sw_TBLDBC, sw_TBLRCO);           
                if (sw_procedimiento == 0)
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Exitoso - Carga de Tablas del Banco", informe, desti_exitos);
                    ingreso.logs("TBC-40", Program.usuario, 0);                    
                }
                else
                {
                    EnviaCorreo.Enviar_SinArchivos("SCR: Fallido - Carga de Tablas del Banco", informe, desti_fallos);
                    ingreso.logs("TBC-41", Program.usuario, 0);  
                }
            }
            cerrar_outlook();
        }

        private void identificar_usuario()
        {
            if (nivel_usr == 1)
            {
                //repro.Enabled = false;
                repro.Enabled = true;  //esto debe ir en falso... solo se habilita por que todavia no se tiene la identificacion de usuario en el sistema
                report.Enabled = true;
                admin.Enabled = true;
            }
            else
            {
                if (nivel_usr == 2)
                {
                    repro.Enabled = true;
                    report.Enabled = false;
                    admin.Enabled = false;
                }
                else
                {
                    repro.Enabled = false;
                    report.Enabled = true;
                    admin.Enabled = false;
                }
            }

        }

        private void obtiene_destinatarios(int proc)
        {
            desti_exitos = "";
            desti_fallos = "";
            try
            {
                SqlCommand cmdp = new SqlCommand();
                cmdp.CommandText = "sp_destinatarios";
                cmdp.CommandType = CommandType.StoredProcedure;
                cmdp.Connection = cone;
                cmdp.Parameters.Clear();

                SqlParameter proce = new SqlParameter("@proceso", SqlDbType.Int);
                proce.Direction = ParameterDirection.Input;
                proce.Value = proc;
                cmdp.Parameters.Add(proce);    
                SqlParameter fallido = cmdp.Parameters.Add("@fallas", SqlDbType.VarChar, 5000);
                fallido.Direction = ParameterDirection.Output;
                SqlParameter exitoso = cmdp.Parameters.Add("@exito", SqlDbType.VarChar, 5000);
                exitoso.Direction = ParameterDirection.Output;      
                cone.Open();
                cmdp.ExecuteNonQuery();
                desti_fallos = (string)cmdp.Parameters["@fallas"].Value;  
                desti_exitos = (string)cmdp.Parameters["@exito"].Value;                       
            }
            catch (SqlException exc)
            {
                abrir_outlook();
                EnviaCorreo.Enviar_SinArchivos("SCR: Fallo al cargar los destinatarios del proceso " + proc.ToString(), "Se produjo un error al cargar los destinatarios para el proceso.<br><br>Descripción del error:<br>" + exc.Message, correo_fallos);
                cerrar_outlook();
            }
            finally
            { cone.Close(); }
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void robot(int proc, DataTable dt,int ciclo)
        {
            //***************************************************************************************
            //***************************************************************************************

            //Generar el Excel para ejecutar la macro del robot EXTRA:
            //Primeramente limpiar carpeta:
            DirectoryInfo dir = new DirectoryInfo("D:\\Extra");
            foreach (FileInfo fi in dir.GetFiles())
            {
                    fi.Delete();
            }

            //Crear un archivo Excel con el detalle de cuentas corrientes que deben buscarse en el EXTRA:
            Microsoft.Office.Interop.Excel.Application aplication;
            Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
            Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;

            aplication = new Microsoft.Office.Interop.Excel.Application();
            libros_trabajo = aplication.Workbooks.Add();
            hoja_trabajo = libros_trabajo.Worksheets.get_Item(1);
            object misValue = System.Reflection.Missing.Value;


            Range r2 = hoja_trabajo.get_Range("A1", "E100");
            r2.Select();
            r2.EntireColumn.NumberFormat = "@";

            hoja_trabajo.Cells[1, "A"] = "Cuentas Corrientes";
            hoja_trabajo.Cells[1, "B"] = "CTA_ROBOT";
            hoja_trabajo.Cells[1, "C"] = "NOMB_ROBOT";
            hoja_trabajo.Cells[1, "D"] = "CI_ROBOT";
            hoja_trabajo.Cells[1, "E"] = "EST_CTA_ROBOT";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null) == false)
                    {
                        hoja_trabajo.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                    }
                }
            }
            
            libros_trabajo.SaveAs("D:\\Extra\\Cuentas Corrientes.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            //libros_trabajo.SaveAs(b, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            libros_trabajo.Close(true, misValue, misValue);
            aplication.Quit();

            releaseObject(hoja_trabajo);
            releaseObject(libros_trabajo);
            releaseObject(aplication);

            MessageBox.Show("Favor ejecutar la macro de EXTRA antes de cerrar este mensaje");

            //La ejecución de la macro creará un archivo de nombre "Cuentas corrientes.xlsx en el Disco D, este archivo debe ser importado a la BD:"
            string excelFilePath = "D:\\Extra\\Cuentas Corrientes.xlsx";
            
            string ssqltable = "EXTRA_CUENTAS_CORRIENTES";
            string myexceldataquery = "select * from [Hoja1$] union all select * from [Hoja1$]";


            string sexcelconnectionstring = @"Provider = Microsoft.ACE.oledb.12.0;data source=" + excelFilePath + ";extended properties=" + "\"excel 12.0;hdr=yes;\"";

            //Limpiar la tabla antes de insertar:
            string sclearsql = "delete from " + ssqltable;
            SqlCommand sqlcmd = new SqlCommand(sclearsql, cone);

            cone.Open();
            sqlcmd.ExecuteNonQuery();
            cone.Close();

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);

            oledbconn.Open();
            OleDbDataReader dr = oledbcmd.ExecuteReader();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(cone);
            bulkcopy.DestinationTableName = ssqltable;
            cone.Open();
            while (dr.Read())
            {
                bulkcopy.WriteToServer(dr);
            }
            dr.Close();
            cone.Close();
            oledbconn.Close();


            //***************************************************************************************
            //***************************************************************************************
            //MessageBox.Show(proc+"");
            string q;           
            string tabla = "";
            ingreso.logs("XTR-00", Program.usuario, 0);

            if (proc == 3)
            {                
                tabla = "TBLCLAUSURAS";
            }
            if (proc == 4 || proc == 5)
            {               
                tabla = "TBLREHABILITACIONES";
            }
            
            
            int counter = 0;
            
            string macro = "SC_0018";  
         

            //*-*
            //q = "Truncate table "+tabla_btra+";";
            //SqlCommand comando = new SqlCommand(q, cone3);                  
            //cone3.Open();
            //comando.ExecuteNonQuery();
            //cone3.Close();

          
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    int posi = i + 1;
               
            //    if (dt.Rows[i][0].ToString() != "")
            //    {
            //        ingreso.logs("XTR-01", Program.usuario, 0);
            //        //........conexion2 BCRAUD RBM_BTRA.... ya no se necesita la conexion2 la BD RBM_BTRA ya esta dentro del servidor Bcraud
            //        q = "Insert into "+tabla_btra+" values(" + Convert.ToInt32(posi) + ",'" + dt.Rows[i][0].ToString() + "','B02CTOR1','IMBC','','','','----','___','___','___','___','___','___','___','___','___','___','___','" + Convert.ToInt32(posi) + "');";                    
            //        comando = new SqlCommand(q, cone3);
            //        cone3.Open();
            //        comando.ExecuteNonQuery();
            //        cone3.Close();
            //        ultimo = i;
            //    }
            //}

            //q = "update xtr.SOLICITUD_SCH set ID_ESTADO=1, HORA_INI=REPLACE(CONVERT(CHAR(5),DATEADD(mi,5,GETDATE()),114),':',''),ID_DIA=DAY(GETDATE()),FECHA_INI=DAY(GETDATE()),ID_MES=MONTH(GETDATE()) WHERE ID_SOL_SCH='" + macro + "'; update xtr.SERV_SESION set ID_TIPO_ESTADO=2 WHERE DESCRIP_SESION='SESION LIBRE'";
            //comando = new SqlCommand(q, cone3);
          
            //cone3.Open();
            //comando.ExecuteNonQuery();
            //cone3.Close();


            //*-*

            //System.Diagnostics.Process.Start("D:/Copy of TrjPlanesCTA/SpartamusPrimePlanesTarjetas/SpartamusPrimePlanesTarjetas/bin/Release/SpartamusPrimePlanesTarjetas.exe");
            
            //System.Diagnostics.Process.Start("" + ConfigurationManager.AppSettings["dirRobot"] + "");
            ingreso.logs("XTR-11", Program.usuario, 0);

            while (counter <= 15)
            {
                int flagabierto1 = 0;
                for (int i = 0; i <= 600000; i++)
                {
                    label100.Text = i.ToString();
                }

                //System.Diagnostics.Process[] procesos = System.Diagnostics.Process.GetProcesses();
                // recorrer los procesos existentes
                //foreach (System.Diagnostics.Process proceso in procesos)
                
                //{                    
                //    // Verificamos si el programa que ejecuta el EXTRA se encuentra dentro de los procesos
                //    if (proceso.ProcessName == "SpartamusPrimePlanesTarjetas")
                //    {
                //        flagabierto1 = flagabierto1 + 1;
                //        // Exit loop code.
                //    }                    
                //}

                if (flagabierto1 == 0)
                {
                    ingreso.logs("XTR-12", Program.usuario, 0);

                    SqlCommand cmdp = new SqlCommand("sp_robot", cone);
                    cmdp.CommandType = CommandType.StoredProcedure;
                    cmdp.Parameters.Clear();
                    SqlParameter p = new SqlParameter("@proc", SqlDbType.Int);
                    p.Direction = ParameterDirection.Input;
                    p.Value = proc;
                    cmdp.Parameters.Add(p);
                    SqlParameter fe = new SqlParameter("@fecha2", SqlDbType.VarChar);
                    fe.Direction = ParameterDirection.Input;
                    fe.Value = dia_actual;
                    cmdp.Parameters.Add(fe);
                    SqlParameter ta = new SqlParameter("@tabla", SqlDbType.VarChar);
                    ta.Direction = ParameterDirection.Input;
                    ta.Value = tabla_btra;
                    cmdp.Parameters.Add(ta);

                    cone.Open();
                    cmdp.ExecuteNonQuery();                   
                    cone.Close();
                    counter = 15;

                    ingreso.logs("XTR-21", Program.usuario, 0);
                }
                counter = counter + 1;   
            }
        }
        
        private void exportar_diario(int proc,int ciclo, string dia_eval)
        {
            reportes_proceso_3 = "";
            
            ExportarEXCEL excel = new ExportarEXCEL();

            SqlCommand cmd = new SqlCommand("sp_obtener_reportes", cone);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            SqlParameter nm = new SqlParameter("@num_proc", SqlDbType.Int);
            nm.Direction = ParameterDirection.Input;
            nm.Value = proc;
            cmd.Parameters.Add(nm);
            try
            {
                cone.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string cod_reporte = dr.GetString(0);
                        string nom_reporte = dr.GetString(1);
                        string num_proc = dr.GetString(2);

                        excel.ExportarDT(dia_eval, dia_eval, cod_reporte, dia_actual + "-" + ciclo.ToString());
                        reportes_proceso_3 = num_proc + "-" + dia_actual + "-" + ciclo.ToString() + "-" + nom_reporte + ".xlsx;" + reportes_proceso_3;
                    }
                }
            }
            catch (SqlException ex)
            {
                cone.Close();
            }
            finally
            {
                cone.Close();
            }    
            
        }
        
        private void enviar_correos(int proc,string cuerpo,string cuerpo2,string cuerpo3)
        {
            string reportes_excel = "";
            string sp;
            string cuerpo_correo = "";

            if (proc == 3)
                sp = "sp_obtener_destinatarios_clau";
            else
                sp = "sp_obtener_destinatarios_rehab";


            SqlCommand cmd = new SqlCommand(sp, cone);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            cone.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            reportes_proceso_3 = reportes_proceso_3.TrimEnd(';');

            //MessageBox.Show(reportes_proceso_3);

            //char[] delimitador = {';'};
            string[] all_reports;
            all_reports = reportes_proceso_3.Split(';');

            abrir_outlook();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    reportes_excel = "";

                    string reportes = dr.GetString(0);

                    string correo = dr.GetString(1);

                    if (proc == 3)
                    {
                        cuerpo_correo = cuerpo;
                        int i = 0;
                        foreach (string repo in all_reports)
                        {
                            if (reportes.Substring(i, 1) == "0")
                            {
                                reportes_excel = repo + ";" + reportes_excel;
                            }
                            i++;
                        }
                        EnviaCorreo.Enviar_ConArchivos("SCR: Reportes - Proceso de Clausura", cuerpo_correo, correo, reportes_excel);
                    }
                    else
                    {
                        if (reportes.Substring(0, 4) != "1111")
                            cuerpo_correo = cuerpo;
                        if (reportes.Substring(4, 4) != "1111")
                            cuerpo_correo = cuerpo_correo + cuerpo2;
                        cuerpo_correo = cuerpo_correo + cuerpo3;

                        int i = 0;
                        foreach (string repo in all_reports)
                        {
                            if (reportes.Substring(i, 1) == "0")
                            {
                                reportes_excel = repo + ";" + reportes_excel;
                            }
                            i++;
                        }                        
                        EnviaCorreo.Enviar_ConArchivos("SCR: Reportes - Proceso de Rehabilitación", cuerpo_correo, correo, reportes_excel);
                    }
                }
            }
            cone.Close();
            cerrar_outlook();  
        }

        private void cuerpo_correo(int pro,string fecha)
        {
            detalle1 = "";
            detalle2 = "";
            detalle3 = "";
            string sp;

            if (pro == 3)
            {
                sp = "sp_clausuras_datos_dia";
                SqlCommand cmdp = new SqlCommand();
                cmdp.CommandText = sp;
                cmdp.CommandType = CommandType.StoredProcedure;
                cmdp.Connection = cone;
                cmdp.Parameters.Clear();

                SqlParameter fec = new SqlParameter("@fecha", SqlDbType.VarChar);
                fec.Direction = ParameterDirection.Input;
                fec.Value = fecha;
                cmdp.Parameters.Add(fec);
                SqlParameter fec2 = new SqlParameter("@fecha2", SqlDbType.VarChar);
                fec2.Direction = ParameterDirection.Input;
                fec2.Value = fecha;
                cmdp.Parameters.Add(fec2);
                SqlParameter inf = cmdp.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                inf.Direction = ParameterDirection.Output;

                cone.Open();
                cmdp.ExecuteNonQuery();
                detalle1 = (string)cmdp.Parameters["@informe"].Value;
                cone.Close();               
            }
            else
            {
                sp = "sp_rehabilitaciones_datos_dia";
                SqlCommand cmdp = new SqlCommand();
                cmdp.CommandText = sp;
                cmdp.CommandType = CommandType.StoredProcedure;
                cmdp.Connection = cone;
                cmdp.Parameters.Clear();

                SqlParameter fec = new SqlParameter("@fecha1", SqlDbType.VarChar);
                fec.Direction = ParameterDirection.Input;
                fec.Value = fecha;
                cmdp.Parameters.Add(fec);
                SqlParameter fec2 = new SqlParameter("@fecha2", SqlDbType.VarChar);
                fec2.Direction = ParameterDirection.Input;
                fec2.Value = fecha;
                cmdp.Parameters.Add(fec2);
                SqlParameter inf = cmdp.Parameters.Add("@informe", SqlDbType.VarChar, 5000);
                inf.Direction = ParameterDirection.Output;
                SqlParameter inf2 = cmdp.Parameters.Add("@informe2", SqlDbType.VarChar, 5000);
                inf2.Direction = ParameterDirection.Output;
                SqlParameter inf3 = cmdp.Parameters.Add("@informe3", SqlDbType.VarChar, 5000);
                inf3.Direction = ParameterDirection.Output;

                cone.Open();
                cmdp.ExecuteNonQuery();
                detalle1 = (string)cmdp.Parameters["@informe"].Value;
                detalle2 = (string)cmdp.Parameters["@informe2"].Value;
                detalle3 = (string)cmdp.Parameters["@informe3"].Value;
                cone.Close();
            }
        }

        private int verifica_ejecucion(int np, string fp)
        {
            int val;
            SqlCommand cmdp = new SqlCommand("sp_verifica_proc", cone);
            cmdp.CommandType = CommandType.StoredProcedure;
            cmdp.Parameters.Clear();

            SqlParameter num = new SqlParameter("@proc", SqlDbType.Int);
            num.Direction = ParameterDirection.Input;
            num.Value = np;
            cmdp.Parameters.Add(num);
            SqlParameter fec = new SqlParameter("@fecha", SqlDbType.VarChar);
            fec.Direction = ParameterDirection.Input;
            fec.Value = fp;
            cmdp.Parameters.Add(fec);
            SqlParameter est = cmdp.Parameters.Add("@estado", SqlDbType.Int);
            est.Direction = ParameterDirection.Output;

            cone.Open();
            cmdp.ExecuteNonQuery();
            val = (Int32)cmdp.Parameters["@estado"].Value;
            cone.Close();
            return val;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            carga_tablas(dia_proceso, dia_actual, false);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Form_Gestion_Usuarios.DefInstance.Show();      
        }

        private void gestiónDeFeriadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Gestion_Feriados.DefInstance.Show();
        }

        private void gestiónDeDestinatariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.operacion = 1;
            Form_Gestion_Destinatarios.DefInstance.Show();    
        }

        private void horariosDeProcesosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Modificacion_Horarios.DefInstance.Show();    
        }
        
        private void habilitaciónRerocesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Habilitacion_Reproceso.DefInstance.Show(); 
        }       
      
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form_Cambiar_Clave.DefInstance.Show();
        }

        private int verifica_robot(int nump, string feact)
        {
            SqlCommand cmdp = new SqlCommand("sp_verifica_robot", cone);
            cmdp.CommandType = CommandType.StoredProcedure;
            cmdp.Parameters.Clear();
            SqlParameter nu = new SqlParameter("@num", SqlDbType.Int);
            nu.Direction = ParameterDirection.Input;
            nu.Value = nump;
            cmdp.Parameters.Add(nu);
            SqlParameter fech = new SqlParameter("@fecha", SqlDbType.VarChar);
            fech.Direction = ParameterDirection.Input;
            fech.Value = feact;
            cmdp.Parameters.Add(fech);
            SqlParameter extr = cmdp.Parameters.Add("@extra", SqlDbType.Int);
            extr.Direction = ParameterDirection.Output;

            cone.Open();
            cmdp.ExecuteNonQuery();
            Int32 ext = (Int32)cmdp.Parameters["@extra"].Value;
            cone.Close();
           
            return ext;
        }

        private void clsusuraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ante = verifica_ejecucion(3, dia_proceso);
            if (ante == 1)
            {
                DialogResult result = MessageBox.Show("Seguro que quiere realizar la ejecucion del proceso de forma manual?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    proceso_clausura(1, dia_proceso, dia_actual);
                    MessageBox.Show("Se ejecutó el proceso.");
                }
                else
                    MessageBox.Show("No se realizó la ejecución del proceso manual.");
            }
            else
                MessageBox.Show("El proceso de la fecha se ejecutó exitosamente\n\nSi quiere ejecutar se debe realizar un Reproceso", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void rehabilitaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ante = verifica_ejecucion(4, dia_proceso);
            if (ante == 1)
            {
                DialogResult result = MessageBox.Show("Seguro que quiere realizar la ejecucion del proceso de forma manual?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    proceso_rehabilitacion(1, dia_proceso, dia_actual);
                    MessageBox.Show("Se ejecutó el proceso.");
                }
                else
                    MessageBox.Show("No se realizó la ejecución del proceso manual.");                
            }
            else
                MessageBox.Show("El proceso de la fecha se ejecutó exitosamente\n\nSi quiere ejecutar se debe realizar un Reproceso", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void cargaDeTablasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int ante = verifica_ejecucion(2, dia_proceso);
            if (ante == 1)
            {
                DialogResult result = MessageBox.Show("Seguro que quiere realizar la ejecucion del proceso de forma manual?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    carga_tablas(dia_proceso, dia_actual, false);
                    MessageBox.Show("Se ejecutó el proceso.");
                }
                else
                    MessageBox.Show("No se realizó la ejecución del proceso manual.");
            }
            else
                MessageBox.Show("El proceso de la fecha se ejecutó exitosamente\n\nSi quiere ejecutar se debe realizar un Reproceso", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void cargaDeCircularesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ante = verifica_ejecucion(1, dia_proceso);
            if (ante == 1)
            {
                DialogResult result = MessageBox.Show("Seguro que quiere realizar la ejecucion del proceso de forma manual?", "Salir", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    carga_circulares(dia_de_proceso, true);
                    MessageBox.Show("Se ejecutó el proceso.");
                }
                else
                    MessageBox.Show("No se realizó la ejecución del proceso manual.");
            }
            else
                MessageBox.Show("El proceso de la fecha se ejecutó exitosamente\n\nSi quiere ejecutar se debe realizar un Reproceso", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        
        private void abrir_outlook()
        {
            
            Process[] procesos = Process.GetProcesses();
            foreach (Process proceso in procesos)
            {
                if (proceso.ProcessName == "OUTLOOK")
                {
                    outlook = true;
                }                
            }

            if(!outlook)
                System.Diagnostics.Process.Start("OUTLOOK.EXE");           
        }

        private void cerrar_outlook()
        {
            if (!outlook)
            {
                Process[] myProcesses;
                myProcesses = Process.GetProcessesByName("OUTLOOK");
                foreach (Process myProcess in myProcesses)
                {
                    myProcess.CloseMainWindow();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
                           
    }
}
