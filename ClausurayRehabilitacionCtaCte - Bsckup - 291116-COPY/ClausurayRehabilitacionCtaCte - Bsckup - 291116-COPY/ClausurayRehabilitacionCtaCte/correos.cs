using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using Outlook = Microsoft.Office.Interop.Outlook;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Diagnostics;

namespace ClausurayRehabilitacionCtaCte
{

    class correos
    {
        //SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);       
        public string emisor = ConfigurationManager.AppSettings["emisor"];
        public string pass = ConfigurationManager.AppSettings["pass"];

        public void correo_informe(string mensaje,string asunto,string destinatarios)
        { 
                System.Diagnostics.Process.Start("OUTLOOK.EXE");

                try
                {
                    destinatarios = destinatarios.TrimEnd(';');
                    //Mail Message
                    MailMessage mM = new MailMessage();
                    //Mail Address
                    mM.From = new MailAddress(emisor);
                    //los destinatarios del correo
                    //string destino = "pablo64ma@gmail.com;pablo.64_ma@hotmail.com";
                    char[] delimitador = new char[] { ';' };
                    foreach (string destinos in destinatarios.Split(delimitador))
                    {
                        mM.To.Add(new MailAddress(destinos));
                    }
                    //asunto del correo
                    mM.Subject = asunto;
                    //arvhivos adjuntos         
                    //mM.Attachments.Add(new Attachment("D:\\CRCTACTE\\librox.xlsx"));
                    //cuerpo del mensaje
                    mensaje = "<font face='Arial'><center><b><h3>BANCO DE CRÉDITO DE BOLIVIA S.A.</h3></b><h4>AUDITORÍA CONTINUA<br></h4></center>" + mensaje + "</FONT>";
                    mM.Body = mensaje;
                    mM.IsBodyHtml = true;
                    //SMTP client
                    SmtpClient sC = new SmtpClient("smtp.live.com");
                    //numero de puerto para hotmail
                    sC.Port = 25;
                    //para loguearse en hotmail
                    sC.Credentials = new System.Net.NetworkCredential(emisor, pass);
                    //enabled SSL
                    sC.EnableSsl = true;
                    //Send an email
                    sC.Send(mM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("no se envio el correo...." + ex);
                }
      
                cerrar("OUTLOOK");
           
        }

       
        static void cerrar(string proceso)
        {
            Process[] myProcesses;
            myProcesses = Process.GetProcessesByName(proceso);
            foreach (Process myProcess in myProcesses)
            {
                myProcess.CloseMainWindow();
            }
        }

    }
}
