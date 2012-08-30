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
            this.Coordinates = new LatLng();
            this.Coordinates.Valid = false;

            string lat = sentence.Fields[0];
            string latDir = sentence.Fields[1]; // n or s

            int deg = int.Parse(lat.Substring(0,2));
            double min = double.Parse(lat.Substring(2));

            this.Coordinates.Latitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(latDir, "s", true) == 0)
            {
                this.Coordinates.Latitude *= -1;
            }

            string lng = sentence.Fields[2];
            string lngDir = sentence.Fields[3]; // e or w

            deg = int.Parse(lng.Substring(0, 3));
            min = double.Parse(lng.Substring(3));

            this.Coordinates.Longitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(lngDir, "w", true) == 0)
            {
                this.Coordinates.Longitude *= -1;
            }

            string utc = sentence.Fields[4];
            string status = sentence.Fields[5];

            this.Coordinates.Valid = false;
            if (string.Compare(status, "a", true) == 0)
            {
                this.Coordinates.Valid = true;
            }
        }
    }
}
