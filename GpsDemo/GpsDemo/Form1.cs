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
            Console.WriteLine(sentence.Sentence);
            if (sentence.HasChecksum)
            {
                Console.WriteLine("Talker ID: {0} Sentence ID: {1} Field Count {2} Checksum: {3}",
                    sentence.TalkerId, sentence.SentenceId, sentence.FieldCount, sentence.Checksum.ToString("X").PadLeft(2, '0'));
            }
            else
            {
                Console.WriteLine("Talker ID: {0} Sentence ID: {1} Field Count {2}",
                    sentence.TalkerId, sentence.SentenceId, sentence.FieldCount);
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
