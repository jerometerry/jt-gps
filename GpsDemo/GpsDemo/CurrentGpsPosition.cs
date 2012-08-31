using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JeromeTerry.GpsDemo.Nmea;

namespace JeromeTerry.GpsDemo
{
    /// <summary>
    /// CurrentGpsPosition is a static class holding a static LatLng coordinate,
    /// representing the last known position from a GPS receiver.
    /// </summary>
    public class CurrentGpsPosition
    {
        /// <summary>
        /// Get / set the last known GPS coordinates
        /// </summary>
        public static LatLng Value { get; set; }
    }
}
