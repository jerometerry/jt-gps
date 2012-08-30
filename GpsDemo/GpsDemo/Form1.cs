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
    public partial class Form1 : Form
    {
        SerialPortReader _portReader;
        NmeaParser _parser;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _parser = new NmeaParser();
            _parser.NmeaSentenceReceived += new NmeaSentenceReceivedEventHandler(_parser_NmeaSentenceReceived);

            _portReader = new SerialPortReader("COM1");
            _portReader.DataReceived += new GpsDataReceivedEventHandler(_portReader_DataReceived);
            _portReader.Start();
        }

        void _parser_NmeaSentenceReceived(NmeaSentence sentence)
        {
            NmeaGLL gll = sentence as NmeaGLL;
            if (gll != null)
            {
                Console.WriteLine("GLL: Lat {0}, Lng {1}", gll.Coordinate.Latitude, gll.Coordinate.Longitude);
            }
        }

        void _portReader_DataReceived(string data)
        {
            _parser.AppendData(data);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _portReader.Stop();
            _portReader.Dispose();
        }
    }
}
