using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausurayRehabilitacionCtaCte
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormPrincipal());
            Application.Run(new Form_Ingreso()); //Comentado
        }
        
        public static int nivel;
        public static string usuario;
        public static int operacion;
        public static string val_glob;
        public static bool Administrador = false;

    }
}
