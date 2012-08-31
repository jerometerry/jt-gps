using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// LatLng represents a geographic position, represented in latitude / longitude
    /// </summary>
    public class LatLng
    {
        /// <summary>
        /// Get / set the latitude
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Get / set the longitude
        /// </summary>
        public decimal Longitude { get; set; }
        
        /// <summary>
        /// Get / set whether the position is valid or not
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Constructs a new LatLng at 0 degrees latitude, 0 degress longitude, 
        /// somewhere in the Gulf of Guinea in the Atlantic Ocean
        /// </summary>
        public LatLng()
        {
        }

        /// <summary>
        /// Constructs a new LatLng, copying the values from the given LatLng
        /// </summary>
        /// <param name="toCopy">The LatLng to copy the values from</param>
        public LatLng(LatLng toCopy)
        {
            this.Latitude = toCopy.Latitude;
            this.Longitude = toCopy.Longitude;
            this.Valid = toCopy.Valid;
        }

        /// <summary>
        /// Consturct a new LatLng using the data fields of the GLL sentence of the NMEA 0183 protocol
        /// </summary>
        /// <param name="lat">The latitude, of the form ddmm.mmmm</param>
        /// <param name="latDir">The direction of the latitude north or south of the equator (n for north, s for south)</param>
        /// <param name="lng">The longitude, of the form dddmm.mmmm</param>
        /// <param name="lngDir">The direction of the longitude east or west of the prime meridian (e for east, w for west)</param>
        /// <param name="status">Whether the GPS position is valid or not (A for valid, V for invalid/void)</param>
        public LatLng(string lat, string latDir, string lng, string lngDir, string status)
        {
            int deg = int.Parse(lat.Substring(0, 2));
            double min = double.Parse(lat.Substring(2));

            this.Latitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(latDir, "s", true) == 0)
            {
                this.Latitude *= -1;
            }

            deg = int.Parse(lng.Substring(0, 3));
            min = double.Parse(lng.Substring(3));

            this.Longitude = Convert.ToDecimal(deg) + (Convert.ToDecimal(min) / Convert.ToDecimal(60));
            if (string.Compare(lngDir, "w", true) == 0)
            {
                this.Longitude *= -1;
            }

            this.Valid = false;
            if (string.Compare(status, "a", true) == 0)
            {
                this.Valid = true;
            }
        }

        /// <summary>
        /// Converts the LatLng to a string containing the latitude and longitude values in degrees,
        /// separated by a comma.
        /// </summary>
        /// <returns>The latitude / longitude degree values, separated by a comma</returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Latitude, this.Longitude);
        }
    }
}
