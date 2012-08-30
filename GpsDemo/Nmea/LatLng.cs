using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    public class LatLng
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Valid { get; set; }

        public LatLng()
        {
        }

        public LatLng(LatLng toCopy)
        {
            this.Latitude = toCopy.Latitude;
            this.Longitude = toCopy.Longitude;
            this.Valid = toCopy.Valid;
        }
    }
}
