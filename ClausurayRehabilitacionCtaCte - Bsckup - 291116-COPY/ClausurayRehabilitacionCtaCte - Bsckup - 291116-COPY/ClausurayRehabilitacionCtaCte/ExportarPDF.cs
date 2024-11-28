using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ClausurayRehabilitacionCtaCte
{
    class ExportarPDF
    {
        public string titulo_rep = "";
        public string nomb_arch = "";        
        public string proceso = "";        
        public string dimensiones = "";
        public string[] encab2 = new string[15];                

        public string imgruta = ConfigurationManager.AppSettings["ruta_imagen"];
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);

        public float[] dimension(int cant, string dimen)
        {
            string[] dim = new string[cant];
            float[] values = new float[cant];
            dim = dimen.Split("*".ToCharArray());

            for (int j = 0; j < cant; j++)
            {
                values[j] = float.Parse(dim[j]);
            }
            
            return values;
        }

        public void datos(string rep)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_obtener_datos_reporte", cone);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlParameter cod = new SqlParameter("@cod_rep", SqlDbType.VarChar);
                cod.Direction = ParameterDirection.Input;
                cod.Value = rep;
                cmd.Parameters.Add(cod);

                cone.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {                        
                        titulo_rep = dr.GetString(2);
                        nomb_arch = dr.GetString(3);                        
                        dimensiones = dr.GetString(7);
                    }
                }
            }
            catch (SqlException ex)
            { 
                MessageBox.Show("Error al intentar obtener los datos del reporte\n" + ex.Message); 
            }
            finally
            { cone.Close(); }   
        }

        public void ExportarDGV(DataGridView dgv,string reporte, string fecha1, string fecha2,string nrep)
        {          
                if (dgv.RowCount < 2)
                {
                    MessageBox.Show("No Hay Datos Para Realizar Un Reporte");
                }
                else
                {
                    datos(nrep);

                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = nomb_arch;
                    save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        string filename = save.FileName;
                        Document doc = new Document(PageSize.LETTER.Rotate(),20,20,50,35);                        
                       
                        try
                        {
                            DateTime dia = DateTime.Today;
                            string sdia = dia.ToString("dd \\de MMMMM \\de yyyy");
                            FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
                            PdfWriter writer = PdfWriter.GetInstance(doc, file);
                            writer.ViewerPreferences = PdfWriter.PageModeUseThumbs;
                            writer.ViewerPreferences = PdfWriter.PageLayoutOneColumn;
                            doc.Open();                           

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imgruta);
                            jpg.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                            doc.Add(jpg);                        

                            Paragraph titulo = new Paragraph("BANCO DE CRÉDITO DE BOLIVIA S.A.", FontFactory.GetFont("ARIAL", 14));
                            titulo.Alignment = Element.ALIGN_CENTER;
                            Paragraph titulo1 = new Paragraph("AUDITORÍA CONTINUA", FontFactory.GetFont("ARIAL", 12));
                            titulo1.Alignment = Element.ALIGN_CENTER;
                            Paragraph titulo2 = new Paragraph("Clausura y Rehabilitación de Cuentas Corrientes", FontFactory.GetFont("ARIAL", 12));
                            titulo2.Alignment = Element.ALIGN_CENTER;
                            Paragraph fecha = new Paragraph("\n\nLa Paz, "+sdia, FontFactory.GetFont("ARIAL", 12));
                            fecha.Alignment = Element.ALIGN_LEFT;
                            Paragraph rep = new Paragraph("\n"+titulo_rep, FontFactory.GetFont("ARIAL", 12));
                            rep.Alignment = Element.ALIGN_LEFT;

                            Paragraph fechas;

                            if (fecha1 == fecha2)
                            {
                                fecha1 = fecha1.Substring(8, 2) + "." + fecha1.Substring(5, 2) + "." + fecha1.Substring(2, 2);
                                fechas = new Paragraph("\nFecha de extracción de datos: " + fecha1 + "\n\n", FontFactory.GetFont("ARIAL", 12));
                            }
                            else
                            {
                                fecha1 = fecha1.Substring(8, 2) + "." + fecha1.Substring(5, 2) + "." + fecha1.Substring(2, 2);
                                fecha2 = fecha2.Substring(8, 2) + "." + fecha2.Substring(5, 2) + "." + fecha2.Substring(2, 2);
                                fechas = new Paragraph("\nRango de datos:\nFecha Inicial: " + fecha1 + "       Fecha Final:" + fecha2 + "\n\n", FontFactory.GetFont("ARIAL", 12));
                            }
                           
                            fechas.Alignment = Element.ALIGN_LEFT;
                                                                                   
                            doc.Add(titulo);
                            doc.Add(titulo1);
                            doc.Add(titulo2);
                            doc.Add(fecha);
                            doc.Add(rep);
                            doc.Add(fechas);

                            //MessageBox.Show("bien");

                            PdfPTable tabla = new PdfPTable(dgv.Columns.Count);
                            float[] headerwidths = dimension(dgv.Columns.Count, dimensiones);
                           

                            //iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA);
                            //Phrase objP = new Phrase("A", fuente);
                            Phrase objP;// = new Phrase("A", FontFactory.GetFont("ARIAL", 20));
                            //tabla.SetWidths(9,9,9,9,9,9,9,8);
                            tabla.SetWidths(headerwidths);
                            tabla.WidthPercentage = 100;
                            tabla.DefaultCell.BorderWidth = 1;

                            for (int i = 0; i < dgv.Columns.Count; i++)
                            {
                                objP = new Phrase(dgv.Columns[i].HeaderText,FontFactory.GetFont("ARIAL",9));
                                tabla.HorizontalAlignment = Element.ALIGN_CENTER;
                                tabla.AddCell(objP);
                                //tabla.AddCell(new Phrase(dgv.Columns[i].HeaderText));
                            }

                            tabla.HeaderRows = 1;

                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgv.Columns.Count; j++)
                                {
                                    if (dgv[j, i].Value != null)
                                    {
                                        objP = new Phrase(dgv[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8));
                                        tabla.AddCell(objP);
                                        //   tabla.AddCell(new Phrase(dgv[j, i].Value.ToString()));
                                    }                                    
                                }
                                tabla.CompleteRow();
                            }

                            doc.Add(tabla);

                            doc.Close();
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al exportar: \n"+ex.Message);
                        }
                    }

                }
           

        }
    }
}
