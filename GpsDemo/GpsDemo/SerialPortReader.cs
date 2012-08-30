using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace JeromeTerry.GpsDemo
{
    public delegate void GpsDataReceived(string data);

    /// <summary>
    /// Simple class for reading ASCII characters from a serial port, for use with the NMEA protocol
    /// </summary>
    public sealed class SerialPortReader : IDisposable
    {
        SerialPort _port;
        private bool _readingData;
        Thread _readThread;
        AutoResetEvent _wait;

        /// <summary>
        /// Event that is raised when new data is read from the Serial (COM) port
        /// </summary>
        public event GpsDataReceived DataReceived;

        public SerialPortReader(string portNumber)
        {
            _port = new SerialPort(portNumber, 4800, Parity.None, 8, StopBits.One);
            _port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
            _wait = new AutoResetEvent(false);
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _wait.Set();
        }

        public void Start()
        {
            if (_readingData == true)
            {
                return;
            }

            _readThread = new Thread(new ThreadStart(this.ReadGpsData));
            _readThread.Name = "Read GPS Data";
            _readingData = true;

            _port.Open();
            _readThread.Start();
        }

        public void Stop()
        {
            _readingData = false;
            _port.Close();
        }

        private void ReadGpsData()
        {
            byte[] buffer = new byte[_port.ReadBufferSize];
            Encoding encoding = System.Text.ASCIIEncoding.Default;
            while (_readingData)
            {
                int read = 0;
                try
                {
                    read = _port.Read(buffer, 0, _port.ReadBufferSize);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading GPS data: {0}", ex);
                }

                if (read > 0)
                {
                    string data = encoding.GetString(buffer, 0, read);
                    if (this.DataReceived != null)
                    {
                        this.DataReceived(data);
                    }
                }

                // Block for 1 second, or until data is received, which ever 
                // occurs first. We could just wait indefinitely, but then the
                // application would hang on shutdown. At least this way,
                // the application will only pause for a second until the timeout 
                // hits.
                _wait.WaitOne(1000);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_port != null)
            {
                _port.Dispose();
                _port = null;
            }
        }

        #endregion
    }
}
