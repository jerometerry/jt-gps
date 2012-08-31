using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using JeromeTerry.GpsDemo.Nmea;

namespace JeromeTerry.GpsDemo
{
    /// <summary>
    /// Form1 is the main Form for the GPS Demo application.
    /// </summary>
    public partial class Form1 : Form
    {
        #region Implementation Data
        SerialPortReader _portReader;
        NmeaParser _parser;
        #endregion

        #region Constructors
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Implementation Operations
        private void Form1_Load(object sender, EventArgs e)
        {
            _parser = new NmeaParser();
            _parser.NmeaSentenceReceived += new NmeaSentenceReceivedEventHandler(_parser_NmeaSentenceReceived);

            string[] ports = SerialPortReader.GetAvailablePorts();
            if (ports != null && ports.Length > 0)
            {
                this._comPorts.Items.AddRange(ports);
                this._comPorts.SelectedIndex = 0;
            }

            string port = this._comPorts.SelectedItem as string;

            _portReader = new SerialPortReader(port);
            _portReader.DataReceived += new GpsDataReceivedEventHandler(_portReader_DataReceived);

            UpdateControls();
        }

        private void _parser_NmeaSentenceReceived(NmeaSentence sentence)
        {
            NmeaGLL gll = sentence as NmeaGLL;
            if (gll != null)
            {
                CurrentGpsPosition.Value = new LatLng(gll.Coordinates);
                Console.WriteLine("GLL: Lat {0}, Lng {1}", gll.Coordinates.Latitude, gll.Coordinates.Longitude);
            }
        }

        private void _portReader_DataReceived(string data)
        {
            _parser.Append(data);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _portReader.Stop();
            _portReader.Dispose();
        }

        private void _btnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._portReader.PortName))
            {
                _portReader.PortName = _comPorts.SelectedItem as string;
                _portReader.Start();
                UpdateControls();
            }
        }

        private void _btnStop_Click(object sender, EventArgs e)
        {
            if (_portReader.Open)
            {
                _portReader.Stop();
                UpdateControls();
            }
        }

        private void UpdateControls()
        {
            bool open = _portReader.Open;
            _comPorts.Enabled = !open;
            _btnStart.Enabled = !open;
            _btnStop.Enabled = open;
        }

        private void _miExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void _miHelpIndex_Click(object sender, EventArgs e)
        {
            HelpDialog dlg = new HelpDialog();
            dlg.ShowDialog();
        }
        #endregion
    }
}
