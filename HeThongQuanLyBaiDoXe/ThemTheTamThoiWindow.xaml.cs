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

namespace HeThongQuanLyBaiDoXe
{
    public delegate void ThemTheTamThoi(TheTamThoi the);

    /// <summary>
    /// Interaction logic for ThemTheTamThoiWindow.xaml
    /// </summary>
    public partial class ThemTheTamThoiWindow : Window
    {
        public ThemTheTamThoi OnThemTheTamThoi;
        public ThemTheTamThoiWindow()
        {
            InitializeComponent();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            this.OnThemTheTamThoi.Invoke(new TheTamThoi("", txtSoThe.Text.Trim(), txtMaThe.Text.Trim(), "True", "False", "6000", "", ""));
            this.Close();
        }
    }
}
