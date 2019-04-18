# RedCorners.ExifLibrary
RedCorners.ExifLibrary is a .NET Standard port of [ExifLibrary](https://github.com/oozcitak/exiflibrary) by oozcitak. The original project can be found [here](https://code.google.com/archive/p/exiflibrary/), which is under [MIT License](https://opensource.org/licenses/mit-license.php).

This fork adds extensions to write and read GPS coordinates with one function call, and as floating point values.

Original documentation can be found [here](https://code.google.com/archive/p/exiflibrary/wikis/ExifLibrary.wiki).

## Easy Reading of GPS Coordinates
```c#
using RedCorners.ExifLibrary;
var file = ImageFile.FromFile(Path);
var coords = file.GetGPSCoords();
if (coords.HasValue)
    (Latitude, Longitude) = coords.Value;
```

## Easy Writing of GPS Coordinates
```c#
using RedCorners.ExifLibrary;
var file = ImageFile.FromFile(Path);
file.SetGPSCoords(Latitude, Longitude);
file.Save(Path);
```

## Low Level Modification of EXIF Properties
```c#
try
{
    var file = ImageFile.FromFile(Path);

    var objLat = GPSLatitudeLongitude.FromFloat(Latitude);
    var objLng = GPSLatitudeLongitude.FromFloat(Longitude);

    file.Properties.Set(ExifTag.GPSLatitude, objLat.d, objLat.m, objLat.s);
    file.Properties.Set(ExifTag.GPSLongitude, objLng.d, objLng.m, objLng.s);

    file.Save(Path);

    MessageBox.Show("File saved.");
}
catch (Exception ex)
{
    MessageBox.Show(ex.ToString());
}

```

## Low Level Reading of EXIF Properties
```c#
try
{
    var file = ImageFile.FromFile(Path);

    var objLat = file.Properties[ExifTag.GPSLatitude] as GPSLatitudeLongitude;
    var objLng = file.Properties[ExifTag.GPSLongitude] as GPSLatitudeLongitude;

    Latitude = objLat?.ToFloat() ?? 0;
    Longitude = objLng?.ToFloat() ?? 0;
}
catch (Exception ex)
{
    MessageBox.Show(ex.ToString());
}
RaisePropertyChanged(nameof(Latitude));
RaisePropertyChanged(nameof(Longitude));
```