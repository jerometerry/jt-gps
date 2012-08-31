using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace JeromeTerry.GpsDemo.Nmea
{
    public delegate void GpsDataReceivedEventHandler(string data);

    /// <summary>
    /// Simple class for reading ASCII characters from a serial port, for use with the NMEA protocol
    /// </summary>
    public sealed class SerialPortReader : IDisposable
    {
        #region Implementation Data

        /// <summary>
        /// The SerialPort object that will handing reading data for us
        /// </summary>
        SerialPort _port;

        /// <summary>
        /// Control value used to terminate the receive thread gracefully
        /// </summary>
        private bool _readingData;
        
        /// <summary>
        /// The receive thread
        /// </summary>
        Thread _readThread;

        /// <summary>
        /// AutoResetEvent used to block until data is available on the COM port.
        /// Use of the AutoResetEvent to block is lest of a waste of CPU than 
        /// just doing Thread.Sleep.
        /// </summary>
        AutoResetEvent _wait;

        /// <summary>
        /// Event that is raised when new data is read from the Serial (COM) port
        /// </summary>
        public event GpsDataReceivedEventHandler DataReceived;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new SerialPortReader
        /// </summary>
        /// <param name="portNumber">The COM port name to read from</param>
        public SerialPortReader(string portNumber)
        {
            _port = new SerialPort(portNumber, 4800, Parity.None, 8, StopBits.One);
            _port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
            _wait = new AutoResetEvent(false);
        }
        #endregion

        #region Operations (SerialPortReader)
        /// <summary>
        /// Get / set the COM port name to read from
        /// </summary>
        public string PortName
        {
            get
            {
                return _port.PortName;
            }
            set
            {
                _port.PortName = value;
            }
        }

        /// <summary>
        /// Get whether the current serial port is open or not
        /// </summary>
        public bool Open
        {
            get
            {
                return _readingData;
            }
        }

        /// <summary>
        /// Get the list of available COM ports
        /// </summary>
        /// <returns></returns>
        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Open the selected COM port and start reading data
        /// </summary>
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

        /// <summary>
        /// Close the selected COM port and stop reading data
        /// </summary>
        public void Stop()
        {
            _readingData = false;
            _port.Close();
        }
        #endregion

        #region Implementation Operations
        /// <summary>
        /// Callback method that notifies us that data is available in the incoming buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _wait.Set();
        }

        /// <summary>
        /// Thread procedure to read incoming data, and fire the DataReceived event handler
        /// when new data is received
        /// </summary>
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
        #endregion

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
