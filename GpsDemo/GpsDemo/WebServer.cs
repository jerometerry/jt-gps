using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using JeromeTerry.GpsDemo.Nmea;

namespace JeromeTerry.GpsDemo
{
    /// <summary>
    /// WebServer is a simple embedded web server used to display a Google Map 
    /// showing the current GPS fix.
    /// 
    /// WebServer only serves up one of two possilbe items:
    /// 
    /// 1. index.html, the web page that displays the Google Map
    /// 2. CurrentGpsCoordinates, which returns the current GPS fix in JSON
    /// 
    /// Any requests for resources other than these 2 will result in the
    /// requested resource URL be returned in plain text
    /// </summary>
    public sealed class WebServer
    {
        #region Implementation Data
        private HttpListener _listener;
        private bool _listening = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new WebServer
        /// </summary>
        /// <param name="prefix">The prefix to start the listener on (e.g. http://*:183/)</param>
        public WebServer(string prefix)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }
        #endregion

        #region Operations (WebServer)
        /// <summary>
        /// Start the WebServer
        /// </summary>
        public void Start()
        {
            _listener.Start();

            _listening = true;
            Thread thread = new Thread(new ThreadStart(Listen));
            thread.Start();

        }

        /// <summary>
        /// Stop the WebServer
        /// </summary>
        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }
        #endregion

        #region Implementation Operations
        /// <summary>
        /// Thread procedure to accept incoming HTTP connections
        /// </summary>
        private void Listen()
        {
            while (_listening)
            {
                IAsyncResult result = _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
                result.AsyncWaitHandle.WaitOne();
            }

            _listener.Close();
        }

        /// <summary>
        /// Callback method that services each incoming HTTP request
        /// </summary>
        /// <param name="result"></param>
        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = _listener.EndGetContext(result);
                Uri url = context.Request.Url;
                System.IO.Stream stream = context.Response.OutputStream;

                string response = null;

                if (RequestedIndexHtml(url))
                {
                    response = ServeUpIndexHtml();
                }
                else if (RequestedGpsFix(url))
                {
                    response = ServeUpGpsFix();
                }
                else
                {
                    response = url.ToString();
                }

                byte[] data = GetUtf8(response);
                if (data != null)
                {
                    stream.Write(data, 0, data.Length);
                }

                context.Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Determine if the requested URL is index.html
        /// </summary>
        /// <param name="url">The requested URL</param>
        /// <returns>True if the request is for index.html, false otherwise</returns>
        private static bool RequestedIndexHtml(Uri url)
        {
            string path = url.ToString();
            return path.IndexOf("index.html", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// Determine if the requested URL is for the current GPS fix
        /// </summary>
        /// <param name="url">The requested URL</param>
        /// <returns>True if the request is for the current GPS fix, false otherwise</returns>
        private static bool RequestedGpsFix(Uri url)
        {
            string path = url.ToString();
            return path.IndexOf("CurrentGpsCoordinates", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// Return the contents of index.html
        /// </summary>
        /// <returns></returns>
        private string ServeUpIndexHtml()
        {
            string dir = System.Windows.Forms.Application.StartupPath;
            string file = System.IO.Path.Combine(dir, "index.html");
            return System.IO.File.ReadAllText(file);
        }

        /// <summary>
        /// Return the current GPS fix in JSON
        /// </summary>
        /// <returns></returns>
        private string ServeUpGpsFix()
        {
            LatLng latLng = CurrentGpsPosition.Value;

            System.Web.Script.Serialization.JavaScriptSerializer exporter =
                new System.Web.Script.Serialization.JavaScriptSerializer();
            return exporter.Serialize(latLng);
        }

        /// <summary>
        /// Converts the given string to a unicode (UTF-8) byte array
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns>The UTF-8 byte array for the given string</returns>
        private static byte[] GetUtf8(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            System.Text.Encoding enc = System.Text.UTF8Encoding.Default;
            byte[] data = enc.GetBytes(s);
            return data;
        }
        #endregion
    }
}
