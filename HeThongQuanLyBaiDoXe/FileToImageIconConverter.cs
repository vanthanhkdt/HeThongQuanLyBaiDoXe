using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HeThongQuanLyBaiDoXe
{
    class FileToImageIconConverter
    {
        private string filePath;

        public string FilePath
        {
            get { return filePath; }
        }
        private System.Windows.Media.ImageSource icon;

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                if (icon == null && System.IO.File.Exists(FilePath))
                {
                    using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(FilePath))
                    {
                        icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sysicon.Handle,
                            System.Windows.Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                return icon;
            }
        }

        public FileToImageIconConverter(string filePath)
        {
            this.filePath = filePath;
        }
    }
    class FileRecord
    {
        public FileRecord(string path)
        {
            if (File.Exists(path))
            {
                this.Name = path;
                Version = FileVersionInfo.GetVersionInfo(path).FileVersion != null ? FileVersionInfo.GetVersionInfo(path).FileVersion : "(không có)";
                Source = new FileToImageIconConverter(path).Icon;
                this.RealName = new FileInfo(path).Name;
            }
        }
        public string Name { get; set; }
        public string Version { get; set; }
        public ImageSource Source { get; set; }
        public string RealName { get; set; }

    }
}
