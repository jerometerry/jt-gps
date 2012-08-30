using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    public class Constants
    {
        public const string NMEA_CRLF = "\r\n";
        public static readonly string[] NMEA_DELIMITERS = new string[] { NMEA_CRLF };
        public const string NMEA_PREFIX = "$";
        public const string NMEA_CHK_MARKER = "*";
    }
}
