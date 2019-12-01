using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
        #endregion
        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            sqlUtility = new SQLUtility(@"Data Source = DESKTOP-JM571ID\SQLEXPRESS; Initial Catalog = DBBaiDoXe; User id = doantotnghiepbaidoxe; Password = baidoxe!@#$;");

            TaiPhanQuyen();
            TaiSoTienNopTruoc();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //LoadAuthorize
        private void TaiPhanQuyen()
        {
            var tatCaQuyen = sqlUtility.GetDistinct(TableName.PhanQuyen, "PhanQuyen");
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
        private int GetNo()
        {
            SQLUtility utilLoadLine = new SQLUtility(@"Data Source = DESKTOP-JM571ID\SQLEXPRESS; Initial Catalog = DBBaiDoXe;");
            DataTable dt = utilLoadLine.GetDataTable("select MAX(_no) AS maxcount FROM dbo.REGISTER");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["maxcount"].ToString() != "")
                {
                    return (1 + int.Parse(dt.Rows[0]["maxcount"].ToString()));
                }
                return 0;
            }
            return 0;
        }
        private void TaiSoTienNopTruoc()
        {
            int index = 0;
            cbbDaNop.Items.Insert(index++, "100000");
            cbbDaNop.Items.Insert(index++, "200000");
            cbbDaNop.Items.Insert(index++, "500000");
        }

        private bool CheckmySingleId(string mySingleId)
        {
            SQLUtility utilLoadLine = new SQLUtility(@"Data Source = DESKTOP-JM571ID\SQLEXPRESS; Initial Catalog = DBBaiDoXe;");
            DataTable dt = utilLoadLine.GetDataTable("select _mySingleId FROM REGISTER WHERE _mySingleId='" + mySingleId + "'");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //AddRole();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtDep.Text.Length == 0)
            {
                tblErrorMessage.Text = "Chưa nhập Khoa / Lớp.";
                txtDep.Focus();
            }

            //else if (Regex.IsMatch(txtID.Text, "[^0-9]+")) // txtmySingleId !
            //{
            //    tblErrorMessage.Text = "ID không đúng.";
            //    txtDep.Select(0, txtDep.Text.Length);
            //    txtDep.Focus();
            //}
            else
            {
                string name = txtName.Text;
                string maSo = txtBienKiemSoat.Text;
                string dep = txtDep.Text;
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
                else
                {
                    string ketQua = sqlUtility.DangKyTaiKhoan(txtName.Text, txtBienKiemSoat.Text, txtDep.Text, txtCode.Text, pwPassword.Password, cbbPhanQuyen.SelectedItem.ToString(), txtReason.Text, "", cbbDaNop.SelectedItem.ToString(), this.SrcImage);
                    if (ketQua!=string.Empty)
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
    }
}
