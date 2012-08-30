using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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

            _portReader = new SerialPortReader("COM1");
            _portReader.DataReceived += new GpsDataReceived(_portReader_DataReceived);
            _portReader.Start();
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
