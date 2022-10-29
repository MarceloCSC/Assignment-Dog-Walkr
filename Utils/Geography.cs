using System;

namespace DogWalkr.Utils
{
    public static class Geography
    {
        public static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double theta = longitude1 - longitude2;
            double distance = Math.Sin(DegreesToRadians(latitude1)) * Math.Sin(DegreesToRadians(latitude2)) +
                              Math.Cos(DegreesToRadians(latitude1)) * Math.Cos(DegreesToRadians(latitude2)) * Math.Cos(DegreesToRadians(theta));

            distance = Math.Acos(distance);
            distance = RadiansToDegrees(distance);
            distance = distance * 60 * 1.1515;

            return distance * 1.609344;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians / Math.PI * 180.0;
        }
    }
}