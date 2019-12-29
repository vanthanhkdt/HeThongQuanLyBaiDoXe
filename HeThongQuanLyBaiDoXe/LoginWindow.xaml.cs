using System;
using System.Collections.Generic;
using System.Data;
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
using static HeThongQuanLyBaiDoXe.AccoundData;

namespace HeThongQuanLyBaiDoXe
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private SQLUtility sqlUtility;
        public LoginWindow()
        {
            InitializeComponent();
            sqlUtility = new SQLUtility();
            RememberUser();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void RememberUser()
        {
            if (Properties.Settings.Default.RememberUser)
            {
                chkRememberPW.IsChecked = true;
                txtID.Text = Properties.Settings.Default.UserName;
                pwPassword.Focus();
            }
            else
            {
                chkRememberPW.IsChecked = false;
                txtID.Focus();
            }
        }

        private void chkRememberPW_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RememberUser = true;
            Properties.Settings.Default.Save();
        }

        private void chkRememberPW_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RememberUser = false;
            Properties.Settings.Default.Save();
        }

        private void pwPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.Text.Length == 0)
            {
                tblErrorMessage.Text = "Vui lòng nhập Biển kiểm soát / CMND";
                txtID.Focus();
            }

            else

            {
                string maSo = txtID.Text.Trim();
                string matKhau = pwPassword.Password.ToString().Trim();

                DataTable dt = new DataTable();
                string ketQuaDangNhap = sqlUtility.KiemTraDangNhap(maSo, sqlUtility.CreateMD5Hash(matKhau), ref dt);
                if (string.IsNullOrEmpty(ketQuaDangNhap))
                {
                    Properties.Settings.Default.UserName = maSo;
                    Properties.Settings.Default.Save();

                    Users nguoiSuDung = Table.ParseUser(dt.Rows[0]);
                    string userName = nguoiSuDung.MaSo.ToString() + " - " + nguoiSuDung.HoTen.ToString();
                    string user = nguoiSuDung.HoTen.ToString();
                    string maSoNho = nguoiSuDung.MaSo.ToString();

                    //string userName = dt.Rows[0]["MaSo"].ToString() + " - " + dt.Rows[0]["HoTen"].ToString();
                    //string user = dt.Rows[0]["HoTen"].ToString();
                    //string maSoNho = dt.Rows[0]["MaSo"].ToString();
                    bool? IsRemember = this.chkRememberPW.IsChecked;

                    //
                    bool isAdmin = false;
                    string role = dt.Rows[0]["PhanQuyen"].ToString().Trim();
                    if (role == "QuanTriVien")
                    {
                        isAdmin = true;
                        //AccManage acc = new AccManage();
                        //acc.txtLogin.Text = "Quản trị viên: " + username;
                        //acc.Show();
                        //acc.lbAdmin.Content = username;
                    }
                    MainWindow mainWindow = new MainWindow(Table.ParseUser(dt.Rows[0]));
                    mainWindow.Show();
                    this.Close();
                }

                else

                {
                    tblErrorMessage.Text = $"Sai thông tin đăng nhập.";
                }
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
