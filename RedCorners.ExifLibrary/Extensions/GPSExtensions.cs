using System;
using System.Collections.Generic;
using System.Text;

namespace RedCorners.ExifLibrary
{
    public static class GPSExtensions
    {
        public static void SetGPSCoords(this ImageFile file, (float Latitude, float Longitude) coords) =>
            SetGPSCoords(file, coords.Latitude, coords.Longitude);

        public static (float Latitude, float Longitude)? GetGPSCoords(this ImageFile file)
        {
            try
            {
                var latitudeRef = file.Properties[ExifTag.GPSLatitudeRef].Value as GPSLatitudeRef?;
                var longitudeRef = file.Properties[ExifTag.GPSLongitudeRef].Value as GPSLongitudeRef?;
                var gpsLatitude = file.Properties[ExifTag.GPSLatitude] as GPSLatitudeLongitude;
                var gpsLongitude = file.Properties[ExifTag.GPSLongitude] as GPSLatitudeLongitude;

                if (latitudeRef is null || longitudeRef is null || gpsLatitude is null || gpsLongitude is null)
                    return null;

                var latitude = gpsLatitude.ToFloat() * (latitudeRef.Value is GPSLatitudeRef.North ? 1.0f : -1.0f);
                var longitude = gpsLongitude.ToFloat() * (longitudeRef.Value is GPSLongitudeRef.East ? 1.0f : -1.0f);

                return (latitude, longitude);
            }
            catch
            {
                return null;
            }
        }

        public static void SetGPSCoords(this ImageFile file, float latitude, float longitude)
        {
            var latitudeRef = latitude >= 0 ? GPSLatitudeRef.North : GPSLatitudeRef.South;
            var longitudeRef = longitude >= 0 ? GPSLongitudeRef.East : GPSLongitudeRef.West;

            var objLat = GPSLatitudeLongitude.FromFloat(Math.Abs((float)latitude));
            var objLng = GPSLatitudeLongitude.FromFloat(Math.Abs((float)longitude));

            file.Properties.Set(ExifTag.GPSLatitude, objLat.d, objLat.m, objLat.s);
            file.Properties.Set(ExifTag.GPSLongitude, objLng.d, objLng.m, objLng.s);
            file.Properties.Set(ExifTag.GPSLatitudeRef, latitudeRef);
            file.Properties.Set(ExifTag.GPSLongitudeRef, longitudeRef);
        }
    }
}
