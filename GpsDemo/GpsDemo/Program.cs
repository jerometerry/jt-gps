using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JeromeTerry.GpsDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WebServer server = new WebServer();
            server.Start();

            Application.Run(new Form1());

            server.Stop();
        }
    }
}
