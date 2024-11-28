using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace ClausurayRehabilitacionCtaCte
{
    class EnviarCorreo
    {
        string ruta_arch = ConfigurationManager.AppSettings["ruta_archivos"];

       

        public void Enviar_SinArchivos(string titulo,string cuerpo, string destinatario)
        {           
            
           
            // Create the Outlook application.
            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            // Set HTMLBody. 
            //add the body of the email
            oMsg.Subject = titulo;
            cuerpo = "<font face='Arial' size = 3><center><b><h4>BANCO DE CRÉDITO DE BOLIVIA S.A.</h4>AUDITORÍA CONTINUA<br></b></center>" + cuerpo + "</FONT>";
            oMsg.HTMLBody = cuerpo;

            destinatario = destinatario.TrimEnd(';');
            //Subject line
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            char[] delimitador = new char[] { ';' };
            Outlook.Recipient oRecip;
            foreach (string destinos in destinatario.Split(delimitador))
            {
                oRecip = (Outlook.Recipient)oRecips.Add(destinos);               
                oRecip.Resolve();
            }
            
            //destinatarios... (CC con copia) se enviara copia a los mencionados 
            if (destinatario != "")
            { oMsg.CC = ""; }
           
            oMsg.Send();
            // Clean up.
            oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

           
        }

        

        public void Enviar_ConArchivos(string titulo, string cuerpo, string destinatario,string archivos)
        {
            //MessageBox.Show(archivos);

            // Create the Outlook application.
            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            // Set HTMLBody. 
            //add the body of the email
            oMsg.Subject = titulo;
            cuerpo = "<font face='Arial' size = 3><center><b><h4>BANCO DE CRÉDITO DE BOLIVIA S.A.</h4>AUDITORÍA CONTINUA<br></b></center>" + cuerpo + "</FONT>";
            oMsg.HTMLBody = cuerpo;

            archivos = archivos.TrimEnd(';');
            destinatario = destinatario.TrimEnd(';');

            char[] delimitador = new char[] { ';' };
            string sDisplayName = "";
            //int iPosition = (int)oMsg.Body.Length + 1;
            int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
            
            foreach (string archivo in archivos.Split(delimitador))
            {
                //MessageBox.Show(archivo);
                Outlook.Attachment oAttach = oMsg.Attachments.Add(ruta_arch + archivo, iAttachType, Type.Missing, sDisplayName);
            }

            //Subject line
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            
            Outlook.Recipient oRecip;
            foreach (string destinos in destinatario.Split(delimitador))
            {
                oRecip = (Outlook.Recipient)oRecips.Add(destinos);
                oRecip.Resolve();
            }

            //destinatarios... (CC con copia) se enviara copia a los mencionados 
            //if (destinatario != "")
            //{ oMsg.CC = "LSotoT@bancred.com.bo"; }
            if (oRecips.ResolveAll())
            {

            }
            oMsg.Send();
            // Clean up.
            oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

                                                                                                                                                                               
        }


    }
}
