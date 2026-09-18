using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPrototype
{
    public static class CoordinateConverter
    {
        public static (double Latitude, double Longitude) EastingNorthingToLatLon(double easting, double northing)
        {
            var (latOsgb36, lonOsgb36) = GridToOsgb36LatLon(easting, northing);
            var (x, y, z) = LatLonToCartesian(latOsgb36, lonOsgb36, AiryA, AiryB);
            var (xWgs, yWgs, zWgs) = ApplyHelmertTransform(x, y, z);
            return CartesianToLatLon(xWgs, yWgs, zWgs, WgsA, WgsB);
        }

        // Airy 1830 ellipsoid - the shape the National Grid/OSGB36 is built on.
        private const double AiryA = 6377563.396;
        private const double AiryB = 6356256.909;

        // WGS84 ellipsoid - what GPS and elevation APIs use.
        private const double WgsA = 6378137.0;
        private const double WgsB = 6356752.314245;

        // National Grid true origin and scale factor (fixed by definition).
        private const double F0 = 0.9996012717;
        private const double N0 = -100000.0;
        private const double E0 = 400000.0;
        private static readonly double Lat0 = ToRadians(49.0);
        private static readonly double Lon0 = ToRadians(-2.0);

        private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
        private static double ToDegrees(double radians) => radians * 180.0 / Math.PI;

        // Stage 1: reverses the Transverse Mercator projection. Latitude
        // has no direct formula from northing alone (the relationship is
        // an infinite series), so this iterates: guess a latitude, work
        // out how far along the meridian that guess implies, compare it
        // to the actual northing, and adjust - repeating until the two
        // agree to within a hundredth of a millimetre.
        private static (double Lat, double Lon) GridToOsgb36LatLon(double easting, double northing) 
        {
            double a = AiryA, b = AiryB;
            double e2 = 1 - (b * b) / (a * a);
            double n = (a - b) / (a + b);
            double n2 = n * n, n3 = n * n * n;

            double lat = Lat0;
            double m = 0;

            do
            {
                lat = (northing - N0 - m) / (a * F0) + lat;

                double ma = (1 + n + 1.25 * n2 + 1.25 * n3) * (lat - Lat0);
                double mb = (3 * n + 3 * n2 + 2.625 * n3) * Math.Sin(lat - Lat0) * Math.Cos(lat + Lat0);
                double mc = (1.875 * n2 + 1.875 * n3) * Math.Sin(2 * (lat - Lat0)) * Math.Cos(2 * (lat + Lat0));
                double md = (35.0 / 24.0) * n3 * Math.Sin(3 * (lat - Lat0)) * Math.Cos(3 * (lat + Lat0));
                m = b * F0 * (ma - mb + mc - md);

            } while (northing - N0 - m >= 0.00001);

            double sinLat = Math.Sin(lat);
            double nu = a * F0 / Math.Sqrt(1 - e2 * sinLat * sinLat);
            double rho = a * F0 * (1 - e2) / Math.Pow(1 - e2 * sinLat * sinLat, 1.5);
            double eta2 = nu / rho - 1;

            double tanLat = Math.Tan(lat);
            double secLat = 1 / Math.Cos(lat);
            double tanLat2 = tanLat * tanLat;
            double tanLat4 = tanLat2 * tanLat2;
            double tanLat6 = tanLat4 * tanLat2;

            double vii = tanLat / (2 * rho * nu);
            double viii = tanLat / (24 * rho * Math.Pow(nu, 3)) * (5 + 3 * tanLat2 + eta2 - 9 * tanLat2 * eta2);
            double ix = tanLat / (720 * rho * Math.Pow(nu, 5)) * (61 + 90 * tanLat2 + 45 * tanLat4);

            double xFactor = secLat / nu;
            double xi = secLat / (6 * Math.Pow(nu, 3)) * (nu / rho + 2 * tanLat2);
            double xii = secLat / (120 * Math.Pow(nu, 5)) * (5 + 28 * tanLat2 + 24 * tanLat4);
            double xiia = secLat / (5040 * Math.Pow(nu, 7)) * (61 + 662 * tanLat2 + 1320 * tanLat4 + 720 * tanLat6);

            double de = easting - E0;
            double finalLat = lat - vii * Math.Pow(de, 2) + viii * Math.Pow(de, 4) - ix * Math.Pow(de, 6);
            double finalLon = Lon0 + xFactor * de - xi * Math.Pow(de, 3) + xii * Math.Pow(de, 5) - xiia * Math.Pow(de, 7);

            return (finalLat, finalLon);
        }

        // Stages 2/4 share this shape: geographic (lat/lon) <-> Cartesian.
        private static (double X, double Y, double Z) LatLonToCartesian(double lat, double lon, double a, double b)
        {
            double e2 = 1 - (b * b) / (a * a);
            double nu = a / Math.Sqrt(1 - e2 * Math.Sin(lat) * Math.Sin(lat));

            double x = nu * Math.Cos(lat) * Math.Cos(lon);
            double y = nu * Math.Cos(lat) * Math.Sin(lon);
            double z = (1 - e2) * nu * Math.Sin(lat);
            return (x, y, z);
        }

        // Stage 3: the actual datum shift. These seven numbers are OS's
        // officially published OSGB36 -> WGS84 parameters.
        private static (double X, double Y, double Z) ApplyHelmertTransform(double x, double y, double z)
        {
            const double tx = 446.448;
            const double ty = -125.157;
            const double tz = 542.060;
            const double s = -20.4894 / 1_000_000.0;
            double rx = ArcSecondsToRadians(0.1502);
            double ry = ArcSecondsToRadians(0.2470);
            double rz = ArcSecondsToRadians(0.8421);

            double xNew = tx + (1 + s) * x - rz * y + ry * z;
            double yNew = ty + rz * x + (1 + s) * y - rx * z;
            double zNew = tz - ry * x + rx * y + (1 + s) * z;
            return (xNew, yNew, zNew);
        }

        private static double ArcSecondsToRadians(double arcSeconds) => arcSeconds * (Math.PI / 180.0) / 3600.0;

        // Stage 4: Cartesian back to geographic, on WGS84 this time.
        // Latitude has no closed-form solution here either, so this
        // iterates a handful of times - it converges fast.
        private static (double Latitude, double Longitude) CartesianToLatLon(double x, double y, double z, double a, double b)
        {
            double e2 = 1 - (b * b) / (a * a);
            double p = Math.Sqrt(x * x + y * y);
            double lon = Math.Atan2(y, x);
            double lat = Math.Atan2(z, p * (1 - e2));

            for (int i = 0; i < 10; i++)
            {
                double sinLat = Math.Sin(lat);
                double nu = a / Math.Sqrt(1 - e2 * sinLat * sinLat);
                lat = Math.Atan2(z + e2 * nu * sinLat, p);
            }

            return (ToDegrees(lat), ToDegrees(lon));
        }
    }
}
