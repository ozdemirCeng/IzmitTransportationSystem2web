using System;

namespace IzmitTransportationSystem.Models
{
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        // Haversine formülü ile iki nokta arasındaki mesafeyi hesaplar (km cinsinden)
        public double CalculateDistance(Coordinates other)
        {
            var R = 6371; // Dünya yarıçapı (km)
            var lat1 = DegreesToRadians(Latitude);
            var lat2 = DegreesToRadians(other.Latitude);
            var dLat = DegreesToRadians(other.Latitude - Latitude);
            var dLon = DegreesToRadians(other.Longitude - Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
