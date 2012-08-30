using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// NmeaGGA is used to parse NMEA 0183 GGA sentences
    /// </summary>
    public class NmeaGGA : NmeaSentence
    {
        public NmeaGGA()
        {
        }

        public NmeaGGA(NmeaSentence sentence)
            : base(sentence)
        {
        }
    }
}
