using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo
{
    /// <summary>
    /// Simple NMEA 0183 parser for parsing data from a NMEA compliant GPS receiver (e.g. Garmin eTrex)
    /// </summary>
    public class NmeaParser
    {
        StringBuilder _buffer;
        object _lock = new object();

        public const string CRLF = "\r\n";
        public static readonly string[] NMEA_DELIMITERS = new string[] { CRLF };

        public NmeaParser()
        {
            _buffer = new StringBuilder();
        }

        public void AppendData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                // can't append nothing
                return;
            }

            lock (_lock)
            {
                _buffer.Append(data);

                // extract the contents of the buffer to a string, then clear it
                string contents = _buffer.ToString();

                int index = contents.IndexOf(CRLF);
                if (index < 0)
                {
                    // No CR \ LF sequence found in the buffer. There is only one line in the buffer, potentially a partial line
                    // leave it until the next append that contains CR \ LF
                    return;
                }

                // NMEA 0183 sentences begin with $ and end with CR\LF. Split the contents
                // by CR\LF, and push the last potentially partial sentence back into the buffer
                string[] sentences = contents.Split(NMEA_DELIMITERS, StringSplitOptions.None);
                if (sentences == null || sentences.Length == 0)
                {
                    // an error occurred
                    return;
                }

                _buffer.Clear();

                string last = sentences[sentences.Length - 1];
                if (!string.IsNullOrEmpty(last))
                {
                    _buffer.Append(last);
                }

                for (int i = 0; i < sentences.Length - 1; i++)
                {
                    string sentence = sentences[i];
                    Console.WriteLine(sentence);
                }
            }
        }
    }
}
