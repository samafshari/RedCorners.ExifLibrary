using System;
using System.Collections.Generic;
using System.Text;

namespace RedCorners.ExifLibrary
{
    public static class GPSExtensions
    {
        public static (float Latitude, float Longitude)? GetGPSCoords(this ImageFile file)
        {
            try
            {
                var objLat = file.Properties[ExifTag.GPSLatitude] as GPSLatitudeLongitude;
                var objLng = file.Properties[ExifTag.GPSLongitude] as GPSLatitudeLongitude;

                if (objLat != null && objLng != null)
                    return (objLat.ToFloat(), objLng.ToFloat());
            }
            catch
            {
            }

            return null;
        }

        public static void SetGPSCoords(this ImageFile file, (float Latitude, float Longitude) coords) =>
            SetGPSCoords(file, coords.Latitude, coords.Longitude);

        public static void SetGPSCoords(this ImageFile file, float latitude, float longitude)
        {
            var objLat = GPSLatitudeLongitude.FromFloat(latitude);
            var objLng = GPSLatitudeLongitude.FromFloat(longitude);

            file.Properties.Set(ExifTag.GPSLatitude, objLat.d, objLat.m, objLat.s);
            file.Properties.Set(ExifTag.GPSLongitude, objLng.d, objLng.m, objLng.s);
        }
    }
}
