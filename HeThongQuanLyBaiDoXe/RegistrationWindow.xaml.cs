using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private SQLUtility sqlUtility;

        #region Khai Báo Biến
        private string srcImage = string.Empty;
        public string SrcImage
        {
            get { return srcImage; }
            set { srcImage = value; OnPropertyChanged("SrcImage"); }
        }
        private SerialPort congComDocMaThe;
        #endregion
        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            sqlUtility = new SQLUtility();
            BatDauKetNoiCongCOM();

            TaiPhanQuyen();
            TaiSoTienNopTruoc();
        }
        public bool BatDauKetNoiCongCOM()
        {
            var tenCongCom = Properties.Settings.Default.COMCuaVao;
            congComDocMaThe = new SerialPort(tenCongCom, 9600, Parity.None, 8, StopBits.One);
            try
            {
                congComDocMaThe.Open();
                congComDocMaThe.DataReceived += Port_DataReceived;
                congComDocMaThe.DiscardInBuffer();
                var thongBao = new MessageWindow("Kết nối thành công !");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mở cổng COM thất bại: " + tenCongCom + " " + ex.Message, "Thất bại !");
            }
            return false;
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(150); // Chờ để nhận dữ liệu xong.
            string duLieuNhanDuoc = congComDocMaThe.ReadExisting();

            //TODO:
            Dispatcher.Invoke(() => { txtCode.Text = duLieuNhanDuoc; });
            //txtCode.Text = duLieuNhanDuoc;
        }

        public bool DangMo()
        {
            return !(congComDocMaThe == null || !congComDocMaThe.IsOpen);
        }
        public void DongCongCOM()
        {
            try
            {
                congComDocMaThe.Close();
                var thongBao = new MessageWindow($"Ngắt kết nối {congComDocMaThe.PortName} thành công !");
            }
            catch (Exception e)
            {
                var thongBao = new MessageWindow($"Ngắt kết nối {congComDocMaThe.PortName} thất bại: {e.Message}");
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //LoadAuthorize
        private void TaiPhanQuyen()
        {
            //var tatCaQuyen = sqlUtility.GetDistinct(TableName.PhanQuyen, "PhanQuyen");
            var tatCaQuyen = new[] { "NhaPhatTrien", "GiangVien", "SinhVien", "NhanVienBaiXe", "Khach", "QuanTriVien" };
            if (tatCaQuyen.Count() > 0)
            {
                foreach (var item in tatCaQuyen)
                {
                    cbbPhanQuyen.Items.Add(Table.LayTenPhanQuyenTuTen(item));
                }
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow logInWindow = new LoginWindow();
            logInWindow.Show();
            this.Close();
        }

        private void TaiSoTienNopTruoc()
        {
            int index = 0;
            cbbDaNop.Items.Insert(index++, "100000");
            cbbDaNop.Items.Insert(index++, "200000");
            cbbDaNop.Items.Insert(index++, "500000");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //AddRole();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                tblErrorMessage.Text = "Chưa nhập họ tên.";
                txtName.Focus();
            }
            else if (string.IsNullOrEmpty(txtBienKiemSoat.Text.Trim()))
            {
                tblErrorMessage.Text = "Chưa nhập BKS/CMND.";
                txtBienKiemSoat.Focus();
            }
            else if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                tblErrorMessage.Text = "Chưa nhập mã thẻ.";
                txtCode.Focus();
            }
            else if (txtDep.Text.Length == 0)
            {
                tblErrorMessage.Text = "Chưa nhập Khoa / Lớp.";
                txtDep.Focus();
            }

            else if (cbbDaNop.SelectedIndex == -1)
            {
                tblErrorMessage.Text = "Vui lòng chọn Số tiền nộp trước.";
                cbbDaNop.Focus();
            }
            else
            {
                string name = txtName.Text.Trim();
                string maSo = txtBienKiemSoat.Text.Trim();
                string dep = txtDep.Text.Trim();
                string password = pwPassword.Password;

                if (pwPassword.Password.Length == 0)
                {
                    tblErrorMessage.Text = "Chưa nhập mật khẩu.";
                    pwPassword.Focus();
                }

                else if (pwAcceptPassword.Password.Length == 0)

                {
                    tblErrorMessage.Text = "Chưa xác nhận mật khẩu.";
                    pwAcceptPassword.Focus();
                }

                else if (pwPassword.Password != pwAcceptPassword.Password)
                {
                    tblErrorMessage.Text = "Xác nhận mật khẩu sai.";

                    pwAcceptPassword.Focus();
                }
                else if (sqlUtility.KiemTraTonTaiMaSo(txtBienKiemSoat.Text.Trim()))
                {
                    tblErrorMessage.Text = $"BKS/CMND {txtBienKiemSoat.Text.Trim()} đã tồn tại.";
                    txtBienKiemSoat.Focus();
                }
                else if (sqlUtility.KiemTraTonTaiMaThe(txtCode.Text.Trim()))
                {
                    tblErrorMessage.Text = $"Thẻ {txtCode.Text.Trim()} đã tồn tại.";
                    txtCode.Focus();
                }
                else
                {
                    string ketQua = sqlUtility.DangKyTaiKhoan(txtName.Text, txtBienKiemSoat.Text, txtDep.Text, txtCode.Text, pwPassword.Password, cbbPhanQuyen.SelectedItem.ToString(), txtReason.Text, "", cbbDaNop.SelectedItem.ToString(), this.SrcImage);
                    if (ketQua != string.Empty)
                    {
                        tblErrorMessage.Text = "Tài khoản " + txtCode.Text.Trim() + " đã tồn tại.";
                        txtCode.Select(0, txtCode.Text.Length);
                        txtCode.Focus();
                    }
                    else
                    {
                        tblErrorMessage.Text = "";
                        tblErrorMessage.Text = "Đăng ký thành công. Đang chờ phê duyệt.";
                        Reset();
                    }
                }
            }
        }
        public void Reset()
        {
            txtName.Text = "";
            txtBienKiemSoat.Text = "";
            txtCode.Text = "";
            SrcImage = string.Empty;
            txtDep.Text = "";
            txtReason.Text = "";
            pwPassword.Password = "";
            pwAcceptPassword.Password = "";
            cbbDaNop.SelectedIndex = -1;
            cbbPhanQuyen.SelectedIndex = -1;
        }

        private void UploadProfileImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != true)
                return;

            this.SrcImage = openFileDialog.FileName;
        }

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.DongCongCOM();
        }
    }
}
