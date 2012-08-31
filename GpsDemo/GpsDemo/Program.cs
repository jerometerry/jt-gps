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

            // start the web server on port 183, since we're working with the 
            // NMEA 0183 protocol :)
            WebServer server = new WebServer("http://localhost:183/");
            server.Start();

            Application.Run(new Form1());

            // stop the web server
            server.Stop();
        }
    }
}
