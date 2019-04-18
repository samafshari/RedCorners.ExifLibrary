using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

namespace RedCorners.ExifLibrary.Demo
{
    public class MainViewModel : BindableModel
    {
        public string Path { get; set; } = @"F:\Home\Desktop\D5300\test.jpg";
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public MainViewModel()
        {
        }

        public Command StoreCommand => new Command(() =>
        {
            try
            {
                var file = ImageFile.FromFile(Path);
                file.SetGPSCoords(Latitude, Longitude);
                file.Save(Path);

                MessageBox.Show("File saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        });

        public Command LoadCommand => new Command(() =>
        {
            try
            {
                var file = ImageFile.FromFile(Path);
                var coords = file.GetGPSCoords();
                if (coords.HasValue)
                    (Latitude, Longitude) = coords.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            RaisePropertyChanged(nameof(Latitude));
            RaisePropertyChanged(nameof(Longitude));
        });

        public Command BrowseCommand => new Command(() =>
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "JPEG Files |*.JPG;*.JPEG";
            dialog.ShowDialog();
            if (File.Exists(dialog.FileName))
                Path = dialog.FileName;

            RaisePropertyChanged(nameof(Path));
        });
    }
}
