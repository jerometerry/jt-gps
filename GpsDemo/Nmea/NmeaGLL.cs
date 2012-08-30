using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// NmeaGLL is used to parse NMEA 0183 GLL sentences
    /// </summary>
    public class NmeaGLL : NmeaSentence
    {
        public NmeaGLL()
        {
        }

        public NmeaGLL(NmeaSentence sentence)
            : base(sentence)
        {
        }
    }
}
