using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using JeromeTerry.GpsDemo.Nmea;

namespace JeromeTerry.GpsDemo
{
    public sealed class WebServer
    {
        private HttpListener _listener;
        private bool _listening = false;

        public WebServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:183/");
        }

        private void Listen()
        {
            while (_listening)
            {
                IAsyncResult result = _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
                result.AsyncWaitHandle.WaitOne();
            }

            _listener.Close();
        }

        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = _listener.EndGetContext(result);
                Uri url = context.Request.Url;
                string path = url.ToString();
                System.IO.Stream stream = context.Response.OutputStream;

                string response = null;

                if (path.IndexOf("index.html", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    string dir = System.Windows.Forms.Application.StartupPath;
                    string file = System.IO.Path.Combine(dir, "index.html");
                    response = System.IO.File.ReadAllText(file);
                }
                else if (path.IndexOf("CurrentGpsCoordinates", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    LatLng latLng = new LatLng();
                    latLng.Valid = true;
                    latLng.Latitude = 47;
                    latLng.Longitude = -52;

                    System.Web.Script.Serialization.JavaScriptSerializer exporter =
                        new System.Web.Script.Serialization.JavaScriptSerializer();
                    response = exporter.Serialize(latLng);
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

        public static byte[] GetUtf8(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            System.Text.Encoding enc = System.Text.UTF8Encoding.Default;
            byte[] data = enc.GetBytes(s);
            return data;
        }

        public void Start()
        {
            _listener.Start();

            _listening = true;
            Thread thread = new Thread(new ThreadStart(Listen));
            thread.Start();

        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }
    }
}
