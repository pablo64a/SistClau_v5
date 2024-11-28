using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Xml;
using Microsoft.Office;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace ClausurayRehabilitacionCtaCte
{
    class ExportarEXCEL
    {
        public string imgruta = ConfigurationManager.AppSettings["ruta_imagen"];
        public string dir_arch = ConfigurationManager.AppSettings["ruta_archivos"];
        SqlConnection cone = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion1"].ConnectionString);

        public string titulo = "";
        public string nomb_arch = "";
        public string rango = "";
        public string proceso = "";
        public string encab;
        public string[] encab2 = new string[15];
        public string f1 = "";
        public string f2 = "";
        public string fecha;
        public string num_proc = "3-";
        // public string col;

        public void encabezado(string rep)
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
                    num_proc = dr.GetString(1);
                    titulo = dr.GetString(2);
                    nomb_arch = dr.GetString(3);
                    rango = dr.GetString(4);
                    proceso = dr.GetString(5);
                    encab = dr.GetString(6);
                }
            }

            if (f1 == f2)
            {
                fecha = "Fecha: " + f1.Substring(8,2)+"."+f1.Substring(5,2)+"."+f1.Substring(2,2);
            }
            else
            {
                fecha = "Fecha Inicio: " + f1.Substring(8, 2) + "." + f1.Substring(5, 2) + "." + f1.Substring(2, 2) + "       Fecha Fin: " + f2.Substring(8, 2) + "." + f2.Substring(5, 2) + "." + f2.Substring(2, 2);
            }
            cone.Close();
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
                      
        public void ExportarDG(DataGridView dgv, string fecha1, string fecha2, string nrep)
        {
            f1 = fecha1;
            f2 = fecha2;
            if (dgv.RowCount < 2)
            {
                MessageBox.Show("No Hay Datos Para Realizar Un Reporte");
            }
            else
            {
                try
                { encabezado(nrep); }
                catch (SqlException ex)
                { MessageBox.Show("Error al intentar obtener los datos del reporte\n" + ex.Message); }
                finally
                { cone.Close(); }                

                SaveFileDialog save = new SaveFileDialog();
                save.FileName = nomb_arch;
                save.Filter = "Excel (*.xls)|*.xls|All Files (*.*)|*.*";
                if (save.ShowDialog() == DialogResult.OK)
                {                    
                    try
                    {
                        Microsoft.Office.Interop.Excel.Application aplication;
                        Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                        Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;

                        aplication = new Microsoft.Office.Interop.Excel.Application();
                        libros_trabajo = aplication.Workbooks.Add();
                        hoja_trabajo = libros_trabajo.Worksheets.get_Item(1);
                        object misValue = System.Reflection.Missing.Value;
                        //para dar nombre a la hoja excel

                        //para quitar las lineas de cuadrícula
                        aplication.ActiveWindow.DisplayGridlines = false;
                        hoja_trabajo.Name = nomb_arch;

                        //hoja_trabajo.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;                 

                        //titulo = reporte;
                        string [] tituloscol = new string [15];
                        tituloscol = encab.Split("*".ToCharArray());                        

                        Microsoft.Office.Interop.Excel.Range r1;
                        r1 = hoja_trabajo.get_Range("A1", "C2");
                        r1.Select(); //Es necesario seleccinar un rango para poder insertar
                        Microsoft.Office.Interop.Excel.Pictures oPictures = (Microsoft.Office.Interop.Excel.Pictures)hoja_trabajo.Pictures(System.Reflection.Missing.Value);                        
                        string path1 = imgruta;                

                        hoja_trabajo.Shapes.AddPicture(@path1, Microsoft.Office.Core.MsoTriState.msoFalse,Microsoft.Office.Core.MsoTriState.msoCTrue,float.Parse(r1.Left.ToString()), float.Parse(r1.Top.ToString()),float.Parse(r1.Width.ToString()), float.Parse(r1.Height.ToString()));
                        //oPictures.Insert(path1, System.Reflection.Missing.Value);                                          
                        
                        Range r2 = hoja_trabajo.get_Range("A13", rango + "13");                        
                        r2.Select();
                        r2.EntireColumn.NumberFormat = "@";                        
                        
                        Range r = hoja_trabajo.get_Range("A4", rango+"4");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);
                        r = hoja_trabajo.get_Range("A5", rango+"5");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);
                        r = hoja_trabajo.get_Range("A6", rango+"6");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);
                        r = hoja_trabajo.get_Range("A10", rango + "10");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);
                        r = hoja_trabajo.get_Range("A8", rango + "8");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);
                        r = hoja_trabajo.get_Range("A9", rango + "9");
                        r.Select();
                        r.MergeCells = false;
                        r.Merge(true);

                        hoja_trabajo.Cells[4, "A"] = "BANCO DE CRÉDITO DE BOLIVIA S.A.";
                        hoja_trabajo.Cells[5, "A"] = "AUDITORÍA CONTINUA";
                        hoja_trabajo.Cells[6, "A"] = "CLAUSURA Y REHABILITACIÓN DE CUENTAS CORRIENTES";

                        r = hoja_trabajo.get_Range("A4", rango+"6");
                        r.Font.Bold = true;
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                        hoja_trabajo.Cells[8, "A"] = proceso;
                        hoja_trabajo.Cells[9, "A"] = titulo;
                        hoja_trabajo.Cells[10, "A"] = fecha;

                        r = hoja_trabajo.get_Range("A8", rango+"10");
                        r.Font.Bold = true;
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                        for (int i = 0; i <= 14; i++)
                        {
                            hoja_trabajo.Cells[12, i + 1] = tituloscol[i];
                        }

                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            for (int j = 0 ; j < dgv.Columns.Count; j++)
                            {
                                if ((dgv.Rows[i].Cells[j].Value == null) == false)
                                {                                    
                                    hoja_trabajo.Cells[i + 13, j + 1] = dgv.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                        }

                        int final = dgv.Rows.Count + 11;
                        
                        r = hoja_trabajo.get_Range("A12", rango + final.ToString());               
                        r.Select();
                        r.Columns.AutoFit();
                        
                        r = hoja_trabajo.get_Range("A12", rango + final.ToString());                       
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r.Cells.BorderAround2();

                        r = hoja_trabajo.get_Range("A12", rango + "12");
                        r.Font.Bold = true;
                        r.Font.ColorIndex = 2;
                        r.Interior.ColorIndex = 25;
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                        libros_trabajo.SaveAs(save.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                        libros_trabajo.Close(true, misValue, misValue);
                        aplication.Quit();

                        releaseObject(hoja_trabajo);
                        releaseObject(libros_trabajo);
                        releaseObject(aplication);
                    }
                    catch (Exception ex)
                    {                        
                        MessageBox.Show("Se produjo un error al exportar.\n" + ex.Message + ex.StackTrace);
                    }
                }
            }
        }
        
        public void ExportarDT(string fecha1, string fecha2, string nrep, string nomb)
        {
            f1 = fecha1.Substring(0, 4) + "-" + fecha1.Substring(4, 2) + "-" + fecha1.Substring(6, 2);
            f2 = fecha2.Substring(0, 4) + "-" + fecha2.Substring(4, 2) + "-" + fecha2.Substring(6, 2);


            //Eliminacion de procesos - Excdl  del Task manager 
            foreach (Process p in System.Diagnostics.Process.GetProcessesByName("excel"))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error de cierre de Procesos : " + e.ToString());
                }
            }

            try
            {
                encabezado(nrep);

                System.Data.DataTable dt = new System.Data.DataTable();
                SqlCommand cmda = new SqlCommand();
                cmda.CommandText = "sp_reportes_" + nrep;
                cmda.CommandType = CommandType.StoredProcedure;
                cmda.Connection = cone;
                cmda.Parameters.Clear();
                SqlParameter fech1 = new SqlParameter("@fecha1", SqlDbType.VarChar);
                fech1.Direction = ParameterDirection.Input;
                fech1.Value = fecha1;
                cmda.Parameters.Add(fech1);
                SqlParameter fech2 = new SqlParameter("@fecha2", SqlDbType.VarChar);
                fech2.Direction = ParameterDirection.Input;
                fech2.Value = fecha2;
                cmda.Parameters.Add(fech2);

                cone.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmda);

                da.Fill(dt);

                Microsoft.Office.Interop.Excel.Application aplication;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;

                aplication = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplication.Workbooks.Add();
                hoja_trabajo = libros_trabajo.Worksheets.get_Item(1);
                object misValue = System.Reflection.Missing.Value;
                //para dar nombre a la hoja excel
                hoja_trabajo.Name = nomb_arch;

                //para quitar las lineas de cuadrícula
                aplication.ActiveWindow.DisplayGridlines = false;

                //hoja_trabajo.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;

                //titulo = reporte;

                string[] tituloscol = new string[15];
                tituloscol = encab.Split("*".ToCharArray());

                Microsoft.Office.Interop.Excel.Range r1;
                r1 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.get_Range("A1", "C2");
                r1.Select(); //Es necesario seleccinar un rango para poder insertar
                Microsoft.Office.Interop.Excel.Pictures oPictures = (Microsoft.Office.Interop.Excel.Pictures)hoja_trabajo.Pictures(System.Reflection.Missing.Value);
                string path1 = imgruta;

                hoja_trabajo.Shapes.AddPicture(@path1, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, float.Parse(r1.Left.ToString()), float.Parse(r1.Top.ToString()), float.Parse(r1.Width.ToString()), float.Parse(r1.Height.ToString()));

                Range r2 = hoja_trabajo.get_Range("A13", rango + "13");
                r2.Select();
                r2.EntireColumn.NumberFormat = "@";

                Range r = hoja_trabajo.get_Range("A4", rango + "4");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);
                r = hoja_trabajo.get_Range("A5", rango + "5");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);
                r = hoja_trabajo.get_Range("A6", rango + "6");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);
                r = hoja_trabajo.get_Range("A10", rango + "10");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);
                r = hoja_trabajo.get_Range("A8", rango + "8");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);
                r = hoja_trabajo.get_Range("A9", rango + "9");
                r.Select();
                r.MergeCells = false;
                r.Merge(true);

                hoja_trabajo.Cells[4, "A"] = "BANCO DE CRÉDITO DE BOLIVIA S.A.";
                hoja_trabajo.Cells[5, "A"] = "AUDITORÍA CONTINUA";
                hoja_trabajo.Cells[6, "A"] = "Clausura y Rehabilitación de Cuentas Corrientes";

                r = hoja_trabajo.get_Range("A4", rango + "6");
                r.Font.Bold = true;
                r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                hoja_trabajo.Cells[8, "A"] = proceso;
                hoja_trabajo.Cells[9, "A"] = titulo;
                hoja_trabajo.Cells[10, "A"] = fecha;

                r = hoja_trabajo.get_Range("A8", rango + "10");
                r.Font.Bold = true;
                r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                for (int i = 0; i <= 14; i++)
                {
                    hoja_trabajo.Cells[12, i + 1] = tituloscol[i];
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if ((dt.Rows[i][j] == null) == false)
                        {
                            hoja_trabajo.Cells[i + 13, j + 1] = dt.Rows[i][j].ToString();
                        }
                    }
                }

                int final = dt.Rows.Count + 12;

                r = hoja_trabajo.get_Range("A12", rango + final.ToString());
                r.Select();
                r.Columns.AutoFit();

                r = hoja_trabajo.get_Range("A12", rango + final.ToString());
                r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                r.Cells.BorderAround2();

                r = hoja_trabajo.get_Range("A12", rango + "12");
                r.Font.Bold = true;
                r.Font.ColorIndex = 2;
                r.Interior.ColorIndex = 25;
                r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                libros_trabajo.SaveAs(dir_arch + num_proc + "-" + nomb +"-" +nomb_arch + ".xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                //libros_trabajo.SaveAs(b, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true, misValue, misValue);
                aplication.Quit();

                releaseObject(hoja_trabajo);
                releaseObject(libros_trabajo);
                releaseObject(aplication);
                
                //Eliminacion de procesos - Excdl  del Task manager 
                foreach (Process p in System.Diagnostics.Process.GetProcessesByName("excel"))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    catch( Exception e)
                    {
                        MessageBox.Show("Error de cierre de Procesos : " + e.ToString());
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar generar el archivo Excel.\n\nDescripción del error:\n" + ex.Message);
            }
            finally
            {
                cone.Close();
            }
        }
    }
}
