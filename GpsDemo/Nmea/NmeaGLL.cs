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
        public LatLng Coordinate { get; set; }

        public NmeaGLL()
        {
        }

        public NmeaGLL(NmeaSentence sentence)
            : base(sentence)
        {
            this.Coordinate = new LatLng();
            this.Coordinate.Valid = false;

            string lat = sentence.Fields[0];
            string latDir = sentence.Fields[1]; // n or s

            double degMin = double.Parse(lat);
            int deg = (int)(degMin / 100);
            double min = degMin - (deg * 100);

            this.Coordinate.Latitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(latDir, "s", true) == 0)
            {
                this.Coordinate.Latitude *= -1;
            }

            string lng = sentence.Fields[2];
            string lngDir = sentence.Fields[3]; // e or w

            degMin = double.Parse(lng);
            deg = (int)(degMin / 1000);
            min = degMin - (deg * 1000);

            this.Coordinate.Longitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(lngDir, "w", true) == 0)
            {
                this.Coordinate.Longitude *= -1;
            }

            string utc = sentence.Fields[4];
            string status = sentence.Fields[5];

            this.Coordinate.Valid = false;
            if (string.Compare(status, "a", true) == 0)
            {
                this.Coordinate.Valid = true;
            }
        }
    }
}
