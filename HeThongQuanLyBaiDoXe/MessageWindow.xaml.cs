using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HeThongQuanLyBaiDoXe
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public delegate void PhanHoiDonGia(int phanHoi);
    public partial class MessageWindow : Window
    {
        public PhanHoiDonGia OnPhanHoiDonGia;
        DispatcherTimer timer;
        public MessageWindow(string thongBao,PackIconKind packIconKind=PackIconKind.Folder,bool tuDongTat=true)
        {
            InitializeComponent();
            this.Show();
            this.packIcon.Kind = packIconKind;
            this.tblThongBao.Text = thongBao;
            if (tuDongTat)
            {
                this.btnChapNhan.Visibility = Visibility.Collapsed;
                this.btnTuChoi.Visibility = Visibility.Collapsed;
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 2);
                timer.Tick += (sender, e) => { this.Close(); };
                timer.Start();
            }
        }

        private void BtnChapNhan_Click(object sender, RoutedEventArgs e)
        {
            OnPhanHoiDonGia.Invoke(1000);
            this.Close();
        }

        private void BtnTuChoi_Click(object sender, RoutedEventArgs e)
        {
            OnPhanHoiDonGia.Invoke(0);
            this.Close();
        }
    }
}
