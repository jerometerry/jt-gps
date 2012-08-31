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
        public LatLng Coordinates { get; set; }

        public NmeaGLL()
        {
        }

        public NmeaGLL(NmeaSentence sentence)
            : base(sentence)
        {
            string lat = sentence.Fields[0];
            string latDir = sentence.Fields[1]; // n or s
            string lng = sentence.Fields[2];
            string lngDir = sentence.Fields[3]; // e or w

            string utc = sentence.Fields[4];
            string status = sentence.Fields[5];

            this.Coordinates = new LatLng(lat, latDir, lng, lngDir, status);
        }
    }
}
