using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// Constants defines the key NMEA 0183 protocol constants used to parse NMEA sentences
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// All NMEA sentences are terminated with carriage return \ line feed
        /// </summary>
        public const string NMEA_CRLF = "\r\n";

        /// <summary>
        /// Define an array containging NMEA_CRLF, for use with string.Split, so that we
        /// don't create this same array every time we parse an NMEA sentence
        /// </summary>
        public static readonly string[] NMEA_DELIMITERS = new string[] { NMEA_CRLF };
        
        /// <summary>
        /// All NMEA sentences start with the dollar sign
        /// </summary>
        public const string NMEA_PREFIX = "$";
        
        /// <summary>
        /// If an NMEA sentence contains a checksum, there will be an asterisk at the end of the 
        /// sentence, followed by a 2 HEX digit check sum, followed by the CR\LF.
        /// </summary>
        public const string NMEA_CHK_MARKER = "*";
    }
}
