//#define KET_NOI_CONG_COM //Comment this line if you dont want to use COM ports (for only Demo)

using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Ports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static HeThongQuanLyBaiDoXe.AccoundData;
using Excel = Microsoft.Office.Interop.Excel;

namespace HeThongQuanLyBaiDoXe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private SQLUtility sqlUtility;
        CongComRaVao congComCuaVao;
        CongComRaVao congComCuaRa;
        private Users user;// = new Users() { MaSo="K125520207029"};
        private Users userDaChon;

        public int tongSoLuongCho = 200;
        public const string CU_PHAP_NAP_THE = "FEE+NAPTHE=";
        public const string LCD_LINE = "\r\n";
        BackgroundWorker exportEXCEL = new BackgroundWorker();
        //DispatcherTimer timerCapNhatDashBoard;

        public MainWindow(Users user)
        {
            InitializeComponent();
            DataContext = this;

            this.user = user;
            ParsePaymentUsers(user);
            this.QuanTriVien = user.PhanQuyen.Equals("5");
            btnAccount.ToolTip = user.HoTen;
            sqlUtility = new SQLUtility();

            LoadData();
            LayCongCom();
            ParseDashBoard(tongSoLuongCho - sqlUtility.SoLuongDangGui()); //Update Dash Board

            congComCuaVao = new CongComRaVao(LoaiCongRaVao.Vao, Properties.Settings.Default.COMCuaVao);
#if KET_NOI_CONG_COM
            congComCuaVao.BatDauKetNoi();
#endif

            congComCuaVao.TienHanhKiemTra += (loaiCong, duLieu) =>
            {
                string ketQua = KiemTraDuLieuRaVao(loaiCong, duLieu);
                if (ketQua == "TaiKhoanKhongTonTai" || ketQua == "KhongTimThay")
                    return;
                Users u = sqlUtility.LayUserTuMaTheGui(duLieu);
                string duLieuPhanHoi = ketQua;
                if (string.IsNullOrEmpty(ketQua))
                {                                                            // LCD screen Constructor
                    duLieuPhanHoi = "Thanh Cong: Vao"                        // Hàng 1: Thanh Cong: Vao
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + Table.XoaChuCoDauDeHienThiLCD(u.HoTen) // Hàng 2: Họ tên
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "Ma So: " + u.MaSo                     // Hàng 3: Biển kiểm soát/ CMND
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "SD: " + u.SoDuKhaDung;                // Hàng 4: Số dư khả dụng
                    congComCuaVao.PhanHoiHanhDong(HoatDong.Vao, true, duLieuPhanHoi);
                    Dispatcher.Invoke(() => { MessageWindow m = new MessageWindow(duLieuPhanHoi); });
                    ParseDashBoard(tongSoLuongCho - sqlUtility.SoLuongDangGui()); //Update Dash Board
                }
                else
                {                                                            // LCD screen Constructor
                    duLieuPhanHoi = "That bai: Vao"                          // Hàng 1: That bai: Vao
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + Table.XoaChuCoDauDeHienThiLCD(u.HoTen) // Hàng 2: Họ tên
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "So Du Khong Du"                       // Hàng 3: Thông báo Số dư không đủ
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "SD: " + u.SoDuKhaDung;                // Hàng 4: Số dư khả dụng
                    congComCuaVao.PhanHoiHanhDong(HoatDong.Vao, false, duLieuPhanHoi);
                    Dispatcher.Invoke(() => { MessageWindow m = new MessageWindow(duLieuPhanHoi); });
                }
            };
            congComCuaRa = new CongComRaVao(LoaiCongRaVao.Ra, Properties.Settings.Default.COMCuaRa);
#if KET_NOI_CONG_COM
            congComCuaRa.BatDauKetNoi();
#endif
            congComCuaRa.TienHanhKiemTra += (loaiCong, duLieu) =>
            {
                string ketQua = KiemTraDuLieuRaVao(loaiCong, duLieu);
                if (ketQua == "TaiKhoanKhongTonTai" || ketQua == "KhongTimThay")
                    return;
                Users u = sqlUtility.LayUserTuMaTheGui(duLieu);
                string duLieuPhanHoi = ketQua;
                if (string.IsNullOrEmpty(ketQua))
                {                                                            // LCD screen Constructor
                    duLieuPhanHoi = "Thanh Cong: Ra"                         // Hàng 1: Thanh Cong: Ra
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + Table.XoaChuCoDauDeHienThiLCD(u.HoTen) // Hàng 2: Họ tên
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "SD: " + u.SoDuKhaDung                 // Hàng 3: Biển kiểm soát/ CMND
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "SD: " + u.SoDuKhaDung;                // Hàng 4: Số dư khả dụng
                    congComCuaRa.PhanHoiHanhDong(HoatDong.Ra, true, duLieuPhanHoi);
                    Dispatcher.Invoke(() => { MessageWindow m = new MessageWindow(duLieuPhanHoi); });
                    ParseDashBoard(tongSoLuongCho - sqlUtility.SoLuongDangGui()); //Update Dash Board
                }
                else
                {                                                            // LCD screen Constructor
                    duLieuPhanHoi = "That bai: Ra"                           // Hàng 1: That bai: Ra
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + Table.XoaChuCoDauDeHienThiLCD(u.HoTen) // Hàng 2: Họ tên
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "So Du Khong Du"                       // Hàng 3: Thông báo Số dư không đủ
                                    + LCD_LINE                               // //////////////////////// Ký tự Xuống dòng
                                    + "SD: " + u.SoDuKhaDung;                // Hàng 4: Số dư khả dụng
                    congComCuaRa.PhanHoiHanhDong(HoatDong.Ra, false, duLieuPhanHoi);
                    Dispatcher.Invoke(() => { MessageWindow m = new MessageWindow(duLieuPhanHoi); });
                }
            };
            // Tao ma the nap
            TaiComboBoxTaoMaThe();

            // Tao ma the
            exportEXCEL.DoWork += ExportEXCEL_DoWork;
            //exportEXCEL.RunWorkerCompleted += ExportEXCEL_RunWorkerCompleted;
            //exportEXCEL.WorkerReportsProgress = true;
            //exportEXCEL.WorkerSupportsCancellation = true;

            //timerCapNhatDashBoard = new DispatcherTimer();
            //timerCapNhatDashBoard.Interval = new TimeSpan(0, 0, 20); // Update DashBoard every 20s
            //timerCapNhatDashBoard.Tick += (sender, e) =>
            //{
            //    ParseDashBoard(tongSoLuongCho - sqlUtility.SoLuongDangGui());
            //};
            //timerCapNhatDashBoard.Start();

            KhoiTaoBieuDo();
            TaiBieuDo();
        }

        private void ExportEXCEL_DoWork(object sender, DoWorkEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog save = new Microsoft.Win32.SaveFileDialog();
            save.Filter = "(Tất cả các tập tin)|*.*|(Excel)|*.xlsx|(Excel 97-2003)|*.xls";
            save.FilterIndex = 2;
            save.ShowDialog();
            if (save.FileName != "")
            {
                // Tạo Excel App
                Excel.Application app = new Excel.Application();
                // Tạo 1 Workbok                
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                // Tạo Sheet
                Excel.Worksheet ws = null;
                //app.Visible = true;

                ws = wb.ActiveSheet;
                // Changing the name of active sheet  
                ws.Name = "DanhSach_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                app.ActiveWindow.DisplayGridlines = false;
                try
                {
                    int count = dgTaoMa.Items.Count;
                    TaoMaThe tmp = dgTaoMa.Items[0] as TaoMaThe;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        // Xuât ra file
                        ws = wb.ActiveSheet;
                        ws.Cells.Style.Font.Name = "Calibri Light";
                        // ws.Name = "";
                        ws.Cells[1, 1].EntireColumn.ColumnWidth = 4.25;
                        ws.Cells[1, 2].EntireColumn.ColumnWidth = 16.88;
                        ws.Cells[1, 3].EntireColumn.ColumnWidth = 16.88;
                        ws.Cells[1, 4].EntireColumn.ColumnWidth = 8.50;
                        ws.Cells[1, 5].EntireColumn.ColumnWidth = 10.38;


                        // DU LIEU HANG 1: TIEU DE
                        // ws.Range[ws.Cells[1, 1], ws.Cells[1, 10]].Merge();
                        ws.Cells[1, 1].Value = "[FEE-TNUT] - PHÁT HÀNH MÃ THẺ NẠP " + DateTime.Now.ToString("dd-MM-yyyy");
                        ws.Cells[1, 1].Font.Color = Excel.XlRgbColor.rgbRed;
                        ws.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        ws.Cells[1, 1].Font.Size = 13;
                        ws.Cells[1, 1].Font.Bold = true;
                        //ws.Cells[1, 1].Interior.Color = Excel.XlRgbColor.rgbYellow;

                        // DU LIEU HANG 2: NGAY THANG NAM EXPORT
                        //ws.Range[ws.Cells[2, 1], ws.Cells[2, 10]].Merge();

                        // Tổng 
                        ws.Cells[2, 1] = "Tổng: " + count.ToString() + " thẻ. Thành tiền: " + DinhDangTien((count * Convert.ToInt32(tmp.MenhGia)).ToString());
                        ws.Cells[2, 1].Font.Color = Excel.XlRgbColor.rgbBlue;
                        ws.Cells[2, 1].Font.Size = 9;

                        ws.Cells[2, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        ws.Cells[2, 1].Font.Bold = true;

                        // Cập nhật
                        ws.Cells[2, 5] = "Cập nhật: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        ws.Cells[2, 5].Font.Color = Excel.XlRgbColor.rgbBlue;
                        ws.Cells[2, 5].Font.Size = 9;

                        ws.Cells[2, 5].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.Cells[2, 5].Font.Bold = false;

                        List<string> header = new List<string>();

                        header.Add("STT");
                        header.Add("Sê-ri");
                        header.Add("Mã thẻ nạp");
                        header.Add("Mệnh giá");
                        header.Add("Trạng thái");

                        for (int j = 0; j < header.Count; j++)
                        {
                            Excel.Range myRange = (Excel.Range)ws.Cells[3, j + 1];
                            ws.Cells[3, j + 1].Font.Bold = true;
                            ws.Cells[3, j + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            ws.Cells[3, j + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                            ws.Cells[3, j + 1].Interior.Color = Excel.XlRgbColor.rgbYellow;

                            myRange.Value2 = header[j];
                        }

                        for (int j = 0; j < count; j++)
                        {
                            List<string> list = new List<string>();
                            TaoMaThe item = dgTaoMa.Items[j] as TaoMaThe;

                            list.Add(item.STT);
                            list.Add(item.SeRi);
                            list.Add(item.MaTheNap);
                            list.Add(item.MenhGia);
                            list.Add(item.TrangThai);

                            for (int i = 0; i < list.Count; i++)
                            {
                                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[j + 4, i + 1];
                                ws.Cells[j + 4, i + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                ws.Cells[j + 4, i + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                                myRange.Value2 = list[i];
                            }
                        }


                        // WorkSheet 2: Thẻ nạp

                        Excel.Worksheet ws2 = wb.Sheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                        ws2.Name = "TheNap_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        app.ActiveWindow.DisplayGridlines = false;
                        ws2.Cells.Style.Font.Name = "Calibri Light";

                        int rowStep = 1;
                        int colStep = 5;
                        int rowBegin = 1;
                        int colBegin = 2;

                        ws2.Cells[1, colBegin].EntireColumn.ColumnWidth = 8.6;
                        ws2.Cells[1, colBegin + 1].EntireColumn.ColumnWidth = 8.6;
                        ws2.Cells[1, colBegin + 2].EntireColumn.ColumnWidth = 8.6;
                        ws2.Cells[1, colBegin + 3].EntireColumn.ColumnWidth = 2.0;


                        for (int j = 0; j < count; j++)
                        {
                            TaoMaThe item = dgTaoMa.Items[j] as TaoMaThe;

                            //Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[j + 4, i + 1];
                            //ws.Cells[j+1, colBegin].Borders.Weight = Excel.XlBorderWeight.xlThin;

                            // Vẽ đường viền
                            var range = ws2.Range[ws2.Cells[rowStep + 1, colBegin], ws2.Cells[rowStep + 5, colBegin + 3]];
                            //range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            Excel.Borders border = range.Borders;
                            border[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            border[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                            border[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            border[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            //range.Borders[BordersLineType.EdgeBottom] = Excel.XlBorderWeight.xlThin;
                            //range.BorderAround = Excel.XlBorderWeight.xlThin;
                            range.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                            // Tiêu đề
                            ws2.Cells[rowStep + 1, colBegin].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 1, colBegin].Font.Bold = true;
                            ws2.Cells[rowStep + 1, colBegin].Value2 = "FEE - THẺ GỬI XE";

                            // Mệnh giá
                            ws2.Range[ws2.Cells[rowStep + 2, colBegin], ws2.Cells[rowStep + 2, colBegin + 3]].Merge();
                            ws2.Cells[rowStep + 2, colBegin].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            ws2.Cells[rowStep + 2, colBegin].Font.Size = 16;
                            ws2.Cells[rowStep + 2, colBegin].Font.Bold = true;
                            ws2.Cells[rowStep + 2, colBegin].Value2 = DinhDangTien(item.MenhGia);

                            // Sê-ri
                            ws2.Cells[rowStep + 3, colBegin].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 3, colBegin].Value2 = "Sê-ri:";
                            ws2.Range[ws2.Cells[rowStep + 3, colBegin + 1], ws2.Cells[rowStep + 3, colBegin + 3]].Merge();
                            ws2.Cells[rowStep + 3, colBegin + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 3, colBegin + 1].Font.Color = Excel.XlRgbColor.rgbBlue;
                            ws2.Cells[rowStep + 3, colBegin + 1].Value2 = item.SeRi;

                            // Mã thẻ
                            ws2.Cells[rowStep + 4, colBegin].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 4, colBegin].Value2 = "Mã thẻ:";
                            ws2.Range[ws2.Cells[rowStep + 4, colBegin + 1], ws2.Cells[rowStep + 4, colBegin + 3]].Merge();
                            ws2.Cells[rowStep + 4, colBegin + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 4, colBegin + 1].Font.Color = Excel.XlRgbColor.rgbBlue;
                            ws2.Cells[rowStep + 4, colBegin + 1].Interior.Color = Excel.XlRgbColor.rgbDimGray;
                            ws2.Cells[rowStep + 4, colBegin + 1].Value2 = item.MaTheNap;

                            // Phát hành
                            ws2.Cells[rowStep + 5, colBegin].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 5, colBegin].Value2 = "Phát hành:";
                            ws2.Cells[rowStep + 5, colBegin + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            ws2.Cells[rowStep + 5, colBegin + 1].Value2 = DateTime.Now.ToString("dd-MM-yyyy");

                            // Vẽ thẻ tiếp theo
                            rowStep += 6;
                        }

                        // Lưu lại
                        wb.SaveAs(save.FileName);

                        //System.Diagnostics.Process.Start(save.FileName);
                        System.Windows.MessageBox.Show("Tệp tin đã lưu thành công!", "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Information);
                        //File.AppendAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t  " + "EXPORT DATA TO EXCEL SUCCESSFULLY: " + save.FileName.ToString() + "\r\n");
                        // HIEN THI TEP TIN EXCEL VUA TAO
                        app.Visible = true;

                        // Tắt Excel sau khi hoàn thành
                        //wb.Close(0);
                        tblTrangThaiInMaTheNap.Text = "Cập nhật: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": Xuất bản thành công " + count + " thẻ nạp.";

                        MessageWindow messageWindow = new MessageWindow("Đã lưu tập tin thành công!");
                        messageWindow.Show();
                    }));
                }

                catch (Exception Ex)
                {
                    System.Windows.MessageBox.Show(Ex.Message, "Thông báo lỗi!", MessageBoxButton.OK, MessageBoxImage.Error);
                    tblTrangThaiInMaTheNap.Text = "Notification: Export Excel Error: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + Ex.Message;
                }
            }
        }

        /// <summary>
        /// Kiem Tra Du Lieu Ra/Vao
        /// </summary>
        /// <param name="loaiCong"></param>
        /// <param name="duLieu">Ma the</param>
        /// <returns></returns>
        private string KiemTraDuLieuRaVao(LoaiCongRaVao loaiCong, string duLieu)
        {
            DataTable dataTable = new DataTable();
            var ketQua = sqlUtility.KiemTraRaVao(duLieu, ref dataTable);
            if (dataTable.Rows.Count <= 0)
            {
                return "TaiKhoanKhongTonTai";
            }
            Users user = Table.ParseUser(dataTable.Rows[0]);
            int soDuKhaDung = Convert.ToInt32(user.SoDuKhaDung);
            int donGia = Convert.ToInt32(user.DonGia);
            int soNgayGui = sqlUtility.TinhToanSoNgayGui(user.MaSo);
            switch (loaiCong)
            {
                case LoaiCongRaVao.Ra:
                    if (dataTable.Rows.Count > 0)
                    {
                        if (soDuKhaDung >= DonGia * (soNgayGui - 1))
                        {
                            sqlUtility.CapNhatSoDuKhaDung(HoatDong.Ra, user.MaSo, user.DonGia);
                            sqlUtility.CapNhatGuiTraXe(user.MaSo, HoatDong.Ra);
                            sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.Ra, true, DateTime.Now.ToString(), "Ra thành công.", "", "");
                            return string.Empty;
                        }
                        else
                        {
                            sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.Ra, false, DateTime.Now.ToString(), "Ra thất bại. Số dư không đủ.", "", "");
                            return "SoDuKhongDu";
                        }
                    }
                    return "KhongTimThay";
                case LoaiCongRaVao.Vao:
                    if (dataTable.Rows.Count > 0)
                    {
                        if (soDuKhaDung >= DonGia)
                        {
                            sqlUtility.CapNhatSoDuKhaDung(HoatDong.Vao, user.MaSo, user.DonGia);
                            sqlUtility.CapNhatGuiTraXe(user.MaSo, HoatDong.Vao);
                            sqlUtility.CapNhatThoiGianGuiCuoi(user.MaSo);
                            sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.Vao, true, DateTime.Now.ToString(), "Vào thành công.", "", "");
                            return string.Empty;
                        }
                        else
                        {
                            sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.Vao, false, DateTime.Now.ToString(), "Vào thất bại. Số dư không đủ.", "", "");
                            return "SoDuKhongDu";
                        }
                    }
                    return "KhongTimThay";
                default:
                    return "Error";
            }
        }

        private void ParseDashBoard(int soLuongChoKhaDung)
        {
            tblTongSoLuongCho.Text = tongSoLuongCho.ToString();
            tblSoLuongChoKhaDung.Text = soLuongChoKhaDung.ToString();

            int soLuongDangGui = tongSoLuongCho - soLuongChoKhaDung;

            tblTiLeXeDangGui.Text = "" + Math.Round(100 * (double)soLuongDangGui / (double)tongSoLuongCho);
            tblTiLeXeDaTra.Text = "" + Math.Round(100 * (double)soLuongDangGui / (double)tongSoLuongCho);

            tblTinhTrangXeGui.Text = soLuongChoKhaDung > tongSoLuongCho / 2 ? "Số lượng xe đang gửi ít" : "Số lượng xe đang gửi nhiều";
            tblTinhTrangChoTrong.Text = soLuongChoKhaDung > tongSoLuongCho / 2 ? "Số lượng chỗ trống nhiều" : "Số lượng chỗ trống ít";

            int soLuotGuiHomQua = sqlUtility.SoLuotGuiTheoNgay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            int soLuotGuiHomNay = sqlUtility.SoLuotGuiTheoNgay(DateTime.Now.ToString("yyyy-MM-dd"));

            double tiLeGuiHomQua = Math.Round(100 * (double)soLuotGuiHomQua / (double)tongSoLuongCho);
            double tiLeGuiHomNay = Math.Round(100 * (double)soLuotGuiHomNay / (double)tongSoLuongCho);

            double tiLeGuiThayDoi = tiLeGuiHomNay - tiLeGuiHomQua;
            tblTiLeThayDoi.Text = tiLeGuiThayDoi.ToString() + "%";

            if (tiLeGuiThayDoi == 0)
            {
                packIconTiLeThayDoi.Kind = PackIconKind.Ban;
                btlTinhTrangTiLeThayDoi.Text = "Không thay đổi";
            }
            else if (tiLeGuiThayDoi < 0 && tiLeGuiThayDoi > -10)
            {
                packIconTiLeThayDoi.Kind = PackIconKind.ArrowDown;
                btlTinhTrangTiLeThayDoi.Text = "Giảm nhẹ";
            }
            else if (tiLeGuiThayDoi < -10)
            {
                packIconTiLeThayDoi.Kind = PackIconKind.ArrowDown;
                btlTinhTrangTiLeThayDoi.Text = "Giảm mạnh";
            }
            else if (tiLeGuiThayDoi > 0 && tiLeGuiThayDoi < 10)
            {
                packIconTiLeThayDoi.Kind = PackIconKind.ArrowUp;
                btlTinhTrangTiLeThayDoi.Text = "Tăng nhẹ";
            }
            else if (tiLeGuiThayDoi > 10)
            {
                packIconTiLeThayDoi.Kind = PackIconKind.ArrowUp;
                btlTinhTrangTiLeThayDoi.Text = "Tăng mạnh";
            }
            tblThoiGianCapNhat.Text = DateTime.Now.ToString("HH:mm dd/MM/yyyy");

            TaiBieuDo(); // Biểu đồ
        }
        private void LoadData()
        {
            try
            {
                // Trang Quản trị
                dgQuanTriTaiKhoan.Items.Clear();
                DataTable table = new DataTable();

                table = sqlUtility.GetDataTable(TableName.Registration);

                if (table != null)
                {
                    dgQuanTriTaiKhoan.Items.Clear();
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var item = Table.ParseRegister(table.Rows[i]);
                        item.PhanQuyen = Table.LayTenPhanQuyenTuMa(item.PhanQuyen);
                        item.DaXuLy = (bool)(table.Rows[i]["DaXuLy"]) == false ? "Đang chờ" : "Đã hoàn thành";
                        item.Color = (bool)(table.Rows[i]["DaXuLy"]);
                        dgQuanTriTaiKhoan.Items.Insert(0, item);
                    }
                }

                // Thẻ tạm thời
                dgTheTamThoi.Items.Clear();
                table = new DataTable();

                table = sqlUtility.GetDataTable(TableName.TheTamThoi);

                if (table != null)
                {
                    dgTheTamThoi.Items.Clear();
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        dgTheTamThoi.Items.Add(new TheTamThoi(
                            (table.Rows[i]["STT"]).ToString(),
                            (table.Rows[i]["SoThe"]).ToString(),
                            (table.Rows[i]["MaThe"]).ToString(),
                            (bool)(table.Rows[i]["ChoPhepHoatDong"]) == true ? "Có" : "Không",
                            (bool)(table.Rows[i]["DangGui"]) == true ? "Có" : "Không",
                            (table.Rows[i]["MaThe"]).ToString(),
                            ((DateTime?)(table.Rows[i]["ThoiGianGuiCuoi"])).HasValue ? DateTime.Now.ToString() : "",
                            ((DateTime?)(table.Rows[i]["ThoiGianTraCuoi"])).HasValue ? DateTime.Now.ToString() : ""
                            ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Show data Error!" + ex.Message);
            }
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #region Tab Tao Ma The
        private void TaiComboBoxTaoMaThe()
        {
            // So luong
            cbbSoLuongMaTheTao.ItemsSource = new[] { "50", "100", "200", "500", "1000" };
            cbbSoLuongMaTheTao.SelectedIndex = 2;

            // Menh gia
            cbbMenhGiaTheTao.ItemsSource = new[] { "1000", "5000", "10000", "20000", "50000", "100000", "200000", "500000", "1000000" };
            cbbMenhGiaTheTao.SelectedIndex = 5;
        }
        private string[] TaoMaTheNapTuDongNgauNhien2()
        {
            var ketQua = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            //MessageBox.Show(""+ketQua.Length); === 32 character
            return new[] {
                ketQua.Substring(0,16),
                ketQua.Substring(16,16)
            };
        }
        private string TaoMaTheNapTuDongNgauNhien()
        {
            var ketQua = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            return ketQua.Substring(0, 16);
        }
        private string TaoSeriTuDongNgauNhien()
        {
            var ketQua = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            return "FEE" + ketQua.Substring(0, 10);
        }
        private void BtnTaoMaTheNap_Click(object sender, RoutedEventArgs e)
        {
            dgTaoMa.Items.Clear();
            var soLuong = Convert.ToInt32(cbbSoLuongMaTheTao.SelectedItem);
            var menhGia = cbbMenhGiaTheTao.SelectedItem.ToString();
            var seri = TaoSeriTuDongNgauNhien();
            for (int i = 0; i < soLuong; i++)
            {
                dgTaoMa.Items.Add(new TaoMaThe((i + 1).ToString(), seri + String.Format("{0:0000}", i + 1), TaoMaTheNapTuDongNgauNhien(), menhGia, "Đang chờ"/*"Đã ban hành"*/));
            }
            MessageBox.Show($"Đã tạo thành công {soLuong} mã thẻ có mệnh giá {string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", Convert.ToInt32(menhGia))}.{Environment.NewLine}Tổng giá trị: {string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", soLuong * Convert.ToInt32(menhGia))}", "Thông báo!");
            btnInMaTheNap.IsEnabled = true;
        }

        private void BtnInMaTheNap_Click(object sender, RoutedEventArgs e)
        {
            if (!exportEXCEL.IsBusy)
            {
                tblTrangThaiInMaTheNap.Visibility = Visibility.Visible;
                tblTrangThaiInMaTheNap.Text = "Đang xuất bản. Vui lòng đợi...";
                exportEXCEL.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Hệ thống bận. Thử lại sau.", "Thông báo!");
            }
        }

        private void BtnPhatHanhMaTheNap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in dgTaoMa.Items)
                {
                    var duLieu = item as TaoMaThe;
                    sqlUtility.Insert(TableName.CardList, Table.CardList, new[] {
                    duLieu.SeRi,
                    duLieu.MaTheNap,
                    duLieu.MenhGia,
                    DateTime.Now.ToString(),
                    "0",
                    "",
                    "1"
                    });
                }

                // Cập nhật giao diện
                for (int i = 0; i < dgTaoMa.Items.Count; i++)
                {
                    var item = dgTaoMa.Items[i];
                    var duLieu = item as TaoMaThe;
                    duLieu.TrangThai = "Đã phát hành";
                    dgTaoMa.Items.RemoveAt(i);
                    dgTaoMa.Items.Insert(i, duLieu);
                }
                // Thông báo thành công
                MessageBox.Show($"Đã phát hành thành công { Convert.ToInt32(cbbSoLuongMaTheTao.SelectedItem)} mã thẻ có mệnh giá {string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", Convert.ToInt32(cbbMenhGiaTheTao.SelectedItem.ToString()))}.{Environment.NewLine}Tổng giá trị: {string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", Convert.ToInt32(cbbSoLuongMaTheTao.SelectedItem.ToString()) * Convert.ToInt32(cbbMenhGiaTheTao.SelectedItem.ToString()))}", "Thông báo!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi!");
            }
        }

        private void BtnHuyTaoMaTheNap_Click(object sender, RoutedEventArgs e)
        {
            this.IsShowMenuVisibility = true;
            this.TabControlSelectedIndex = 0;
        }
        #endregion
        #region Tab Cai Dat
        private void LayCongCom()
        {
            var danhSachCongComKhaDung = SerialPort.GetPortNames();
            foreach (var item in danhSachCongComKhaDung)
            {
                this.cbbCongComCuaVao.Items.Add(item);
                this.cbbCongComCuaRa.Items.Add(item);
            }

            this.cbbCongComCuaVao.Items.Add(Properties.Settings.Default.COMCuaVao);
            this.cbbCongComCuaRa.Items.Add(Properties.Settings.Default.COMCuaRa);

            this.cbbCongComCuaVao.SelectedIndex = danhSachCongComKhaDung.Length;
            this.cbbCongComCuaRa.SelectedIndex = danhSachCongComKhaDung.Length;

        }
        #endregion
        #region Tab The Tam Thoi
        private void MenuItemThemMoi_Click(object sender, RoutedEventArgs e)
        {
            ThemTheTamThoiWindow ttttWindow = new ThemTheTamThoiWindow(this.congComCuaVao);
            ttttWindow.OnThemTheTamThoi += (the) =>
            {
                sqlUtility.Insert(TableName.TheTamThoi, Table.TheTamThoi, new[] { the.SoThe, the.MaThe, the.ChoPhepHoatDong, the.DangGui, the.ThoiGianGuiCuoi, the.ThoiGianTraCuoi, the.DonGia });
            };
            ttttWindow.ShowDialog();
            LoadData();
        }

        private void MenuItemHuyThe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemLamTuoi_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void MenuItemChiTiet_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #region Tab Quan Ly The
        #region Bien Toan Cuc
        private int donGia = 6000;
        public int DonGia
        {
            get { return donGia; }
            set { donGia = value; OnPropertyChanged("DonGia"); }
        }
        #endregion

        private void BtnTangGia_Click(object sender, RoutedEventArgs e)
        {
            if (DonGia > 0 && DonGia % 10000 == 0)
            {
                var thongBao = new MessageWindow($"Đơn giá hiện tại đã vượt mức {DinhDangTien(DonGia.ToString())}/ngày. Bạn có muốn tiếp tục?", PackIconKind.Coin, false);
                thongBao.OnPhanHoiDonGia += (phanHoi) => { DonGia += phanHoi; };
            }
            else
            {
                DonGia += 1000;
            }
        }

        private void BtnGiamGia_Click(object sender, RoutedEventArgs e)
        {
            if (DonGia <= 0)
            {
                var thongBao = new MessageWindow($"Đơn giá hiện tại đã nhỏ hơn mức {DinhDangTien(DonGia.ToString())}/ngày. Bạn có muốn tiếp tục?", PackIconKind.DollarOff, false);
                thongBao.OnPhanHoiDonGia += (phanHoi) => { DonGia -= phanHoi; };
            }
            else
            {
                DonGia -= 1000;
            }
        }

        private void BtnLuuGia_Click(object sender, RoutedEventArgs e)
        {
            sqlUtility.CapNhatDonGia(userDaChon.MaSo, tblDonGia.Text);
            MessageBox.Show($"Đã cập nhật đơn giá: {DinhDangTien(tblDonGia.Text)}/ngày.{Environment.NewLine}Họ và tên: {userDaChon.HoTen}{Environment.NewLine}Mã số: {userDaChon.MaSo}", "Thành công!");
        }
        #endregion

        #region Tab Thanh Toan
        private void BtnSendTT_Click(object sender, RoutedEventArgs e)
        {
            stackPanelMessageTT.Children.Add(new MessageSentUserControl(user.HoTen, DateTime.Now.ToString("HH:mm"), txtMessageTT.Text));

            DataTable dataTable = new DataTable();
            string duLieu = txtMessageTT.Text;
            if (duLieu.StartsWith(CU_PHAP_NAP_THE))
            {
                string[] maThe = duLieu.Split(new[] { CU_PHAP_NAP_THE }, StringSplitOptions.RemoveEmptyEntries);
                string maTheNap = string.Empty;
                if (maThe?.Length > 0)
                    maTheNap = maThe[0].ToUpper().Trim();

                string ketQua = sqlUtility.KiemTraMaTheNap(maTheNap, ref dataTable);
                if (string.IsNullOrEmpty(ketQua))
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        sqlUtility.CapNhatNapTheThanhCong(user.MaSo, maTheNap);
                        sqlUtility.CapNhatSoDuKhaDung(HoatDong.NapThe, user.MaSo, dataTable.Rows[0]["GiaTri"].ToString());
                        user = Table.ParseUser(sqlUtility.GetDataTable($"SELECT * FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE MaSo = '{user.MaSo}' AND ChoPhepHoatDong=1;").Rows[0]);
                        stackPanelMessageTT.Children.Add(new MessageReceivedUserControl("TNUT-FEE", DateTime.Now.ToString("HH:mm"),
                           $"Nạp thẻ thành công.{Environment.NewLine}Tài khoản: {user.HoTen} ({user.MaSo}).{Environment.NewLine}Số tiền nạp: {DinhDangTien(dataTable.Rows[0]["GiaTri"].ToString())}{Environment.NewLine}Số dư khả dụng: {DinhDangTien(user.SoDuKhaDung)}"));
                        ParsePaymentUsers(user);
                        sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.NapThe, true, DateTime.Now.ToString(), "Nạp thẻ thành công.", maTheNap, dataTable.Rows[0]["GiaTri"].ToString());
                    }
                }
                else
                {
                    stackPanelMessageTT.Children.Add(new MessageReceivedUserControl("TNUT-FEE", DateTime.Now.ToString("HH:mm"),
                           "Thất bại. Kiểm tra lại mã thẻ nạp."));
                    sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.NapThe, false, DateTime.Now.ToString(), "Nạp thẻ thất bại.", maTheNap, "");
                }
            }
            scrollViewerQuanLyTheTT.ScrollToBottom();
        }

        private void BtnHanhDongTT_Click(object sender, RoutedEventArgs e)
        {
            txtMessageTT.Text = CU_PHAP_NAP_THE;
            txtMessageTT.Focus();
            txtMessageTT.CaretIndex = CU_PHAP_NAP_THE.Length;
        }
        #endregion
        #region Tab Tai Khoan
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Configure the message box to be displayed
            string messageBoxText = "Bạn có chắc chắn muốn phê duyệt tài khoản này?";
            string caption = "Xác nhận phê duyệt";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        Registers dt = (Registers)dgQuanTriTaiKhoan.SelectedItem;
                        DataTable table = new DataTable();
                        table = sqlUtility.GetDataTable($"SELECT * FROM TBRegistration WHERE HoTen = N'{dt.HoTen}' AND MaSo = N'{dt.MaSo}'");

                        if (table != null)
                        {
                            //var data = Table.ParseRegister(table.Rows[0]);

                            var hoTen = table.Rows[0]["HoTen"];
                            var maSo = table.Rows[0]["MaSo"];
                            var matKhau = table.Rows[0]["MatKhau"];
                            var khoaLop = table.Rows[0]["KhoaLop"];
                            var maTheGui = table.Rows[0]["MaTheGui"];
                            var phanQuyen = table.Rows[0]["PhanQuyen"];
                            var choPhepHoatDong = "TRUE";
                            var nguoiThem = user.MaSo;
                            var ngayThem = DateTime.Now.ToString();
                            var soDuKhaDung = table.Rows[0]["DaNop"].ToString().Trim();
                            var dangGui = "FALSE";
                            var truyCapLanCuoi = "";
                            var guiLanCuoi = "";
                            var hinhAnh = table.Rows[0]["HinhAnh"];
                            string donGia = "6000";

                            string ketQua = sqlUtility.ApproveUser(hoTen, maSo, matKhau, khoaLop, maTheGui, phanQuyen, choPhepHoatDong,
                                nguoiThem, ngayThem, soDuKhaDung, dangGui, truyCapLanCuoi, guiLanCuoi, hinhAnh, donGia);
                            if (string.IsNullOrEmpty(ketQua))
                            {
                                LoadData();
                                break;
                            }
                            MessageBox.Show(ketQua);
                        }
                        break;
                    }
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Bạn có chắc chắn muốn từ chối tài khoản này?";
            string caption = "Xác nhận từ chối";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        try
                        {
                            string selectedHoTen = ((Registers)dgQuanTriTaiKhoan.SelectedItem).HoTen;
                            string selectedMaTheGui = ((Registers)dgQuanTriTaiKhoan.SelectedItem).MaSo;
                            sqlUtility.DeleteAccount(selectedHoTen, selectedMaTheGui);

                            LoadData();
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                        break;
                    }
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        #endregion

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        public BitmapImage ToBitmapImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static ImageSource ByteToBitmapImage(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        private void ChiTiet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedMaSo = ((Registers)dgQuanTriTaiKhoan.SelectedItem).MaSo;

                DataTable table = new DataTable();
                table = sqlUtility.GetDataTable("SELECT * FROM TBUsers WHERE MaSo = N'" + selectedMaSo + "' ;");
                if (table.Rows.Count > 0)
                {
                    var data = Table.ParseUser(table.Rows[0]);
                    userDaChon = data;
                    ParseProfileUsers(data);

                    this.TabControlSelectedIndex = 4;
                    this.IsShowMenuVisibility = false;
                }
                else
                {
                    table = sqlUtility.GetDataTable("SELECT * FROM TBRegistration WHERE MaSo = N'" + selectedMaSo + "' ;");
                    {
                        if (table.Rows.Count > 0)
                        {
                            var data = Table.ParseRegister(table.Rows[0]);
                            ParseProfileRegisters(data);

                            this.TabControlSelectedIndex = 4;
                            this.IsShowMenuVisibility = false;
                        }
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnMauNapThe_Click(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = CU_PHAP_NAP_THE;
            txtMessage.Focus();
            txtMessage.CaretIndex = CU_PHAP_NAP_THE.Length;
        }


        private int tabControlSelectedIndex = 0;
        public int TabControlSelectedIndex
        {
            get { return tabControlSelectedIndex; }
            set { tabControlSelectedIndex = value; OnPropertyChanged("TabControlSelectedIndex"); }
        }

        private bool isShowMenuVisibility = true;
        public bool IsShowMenuVisibility
        {
            get { return isShowMenuVisibility; }
            set { isShowMenuVisibility = value; OnPropertyChanged("IsShowMenuVisibility"); }
        }

        private void BtnCloseDetail_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            this.IsShowMenuVisibility = true;
            if (button.Name == "btnCloseDetailTT")
            {
                this.TabControlSelectedIndex = 0;
            }
            else
            {
                this.TabControlSelectedIndex = 1;
            }

        }

        private void ParseProfileUsers(Users user)
        {
            DonGia = Convert.ToInt32(user.DonGia);
            imageProfileDetail.ImageSource = ByteToBitmapImage(user.HinhAnh);
            tblProfileNameDetail.Text = user.HoTen;
            tblBienKiemSoat.Text = user.MaSo;
            tblProfileAuthorDetail.Text = Table.LayTenPhanQuyenTuMa(user.PhanQuyen);
            tblAddress.Text = user.KhoaLop;
            tblMaTheGui.Text = user.MaTheGui;
            tblTaiKhoanKhaDung.Text = user.SoDuKhaDung;
            borderTrangThai.Background = user.DangGui == "True" ? Brushes.Green : Brushes.Gray;
            tblStatus.Text = user.DangGui == "True" ? "Đang gửi" : "Không gửi";
        }

        private void ParsePaymentUsers(Users user)
        {
            tblTTHoTen.Text = user.HoTen;
            imageProfileDetailTT.ImageSource = ByteToBitmapImage(user.HinhAnh);
            tblTTProfileHoTen.Text = user.HoTen;
            tblTTBienKiemSoat.Text = user.MaSo;
            tblTTProfileAuthorDetail.Text = Table.LayTenPhanQuyenTuMa(user.PhanQuyen);
            tblTTAddress.Text = user.KhoaLop;
            tblMaTheGui.Text = user.MaTheGui;
            tblTTSoDuKhaDung.Text = user.SoDuKhaDung;
            borderTrangThaiTT.Background = user.DangGui == "True" ? Brushes.Green : Brushes.Gray;
        }

        private void ParseProfileRegisters(Registers register)
        {
            imageProfileDetail.ImageSource = ByteToBitmapImage(register.HinhAnh);
            tblProfileNameDetail.Text = register.HoTen;
            tblBienKiemSoat.Text = user.MaSo;
            tblProfileAuthorDetail.Text = Table.LayTenPhanQuyenTuMa(register.PhanQuyen);
            tblAddress.Text = register.KhoaLop;
            tblMaTheGui.Text = user.MaTheGui;
            tblTaiKhoanKhaDung.Text = register.DaNop;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            stackPanelMessage.Children.Add(new MessageSentUserControl(user.HoTen, DateTime.Now.ToString("HH:mm"), txtMessage.Text));

            DataTable dataTable = new DataTable();
            string duLieu = txtMessage.Text;
            if (duLieu.StartsWith(CU_PHAP_NAP_THE))
            {
                string[] maThe = duLieu.Split(new[] { CU_PHAP_NAP_THE }, StringSplitOptions.RemoveEmptyEntries);
                string maTheNap = string.Empty;
                if (maThe?.Length > 0)
                    maTheNap = maThe[0].ToUpper().Trim();

                string ketQua = sqlUtility.KiemTraMaTheNap(maTheNap, ref dataTable);
                if (string.IsNullOrEmpty(ketQua))
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        sqlUtility.CapNhatNapTheThanhCong(userDaChon.MaSo, maTheNap);
                        sqlUtility.CapNhatSoDuKhaDung(HoatDong.NapThe, userDaChon.MaSo, dataTable.Rows[0]["GiaTri"].ToString());
                        userDaChon = Table.ParseUser(sqlUtility.GetDataTable($"SELECT * FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE MaSo = '{userDaChon.MaSo}' AND ChoPhepHoatDong=1;").Rows[0]);
                        stackPanelMessage.Children.Add(new MessageReceivedUserControl("TNUT-FEE", DateTime.Now.ToString("HH:mm"),
                           $"Nạp thẻ thành công cho tài khoản {userDaChon.HoTen} ({userDaChon.MaSo}).{Environment.NewLine}Số tiền nạp: {DinhDangTien(dataTable.Rows[0]["GiaTri"].ToString())}{Environment.NewLine}Số dư khả dụng: {DinhDangTien(userDaChon.SoDuKhaDung)}"));
                        ParseProfileUsers(userDaChon);
                        sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.NapThe, true, DateTime.Now.ToString(), "Nạp thẻ thành công.", maTheNap, dataTable.Rows[0]["GiaTri"].ToString());
                    }
                }
                else
                {
                    stackPanelMessage.Children.Add(new MessageReceivedUserControl("TNUT-FEE", DateTime.Now.ToString("HH:mm"),
                           "Thất bại. Kiểm tra lại mã thẻ nạp."));
                    sqlUtility.CapNhatHoatDong(user.MaSo, HoatDong.NapThe, false, DateTime.Now.ToString(), "Nạp thẻ thất bại.", maTheNap, "");
                }
            }
            scrollViewerQuanLyThe.ScrollToBottom();
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 0;
        }

        private void BtnQuanLyTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 1;
        }
        private void BtnTrinhTaoMaTheNap_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 2;
        }

        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 3;
        }
        private void BtnTheTamThoi_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 6;
        }
        private void BtnCaiDat_Click(object sender, RoutedEventArgs e)
        {
            this.TabControlSelectedIndex = 5;
        }

        private void BtnHuyCaiDat_Click(object sender, RoutedEventArgs e)
        {
            this.IsShowMenuVisibility = true;
            this.TabControlSelectedIndex = 0;
        }

        private void BtnLuuCaiDat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                congComCuaVao = new CongComRaVao(LoaiCongRaVao.Vao, cbbCongComCuaVao.SelectedItem.ToString());
                congComCuaRa = new CongComRaVao(LoaiCongRaVao.Ra, cbbCongComCuaRa.SelectedItem.ToString());

                // Luu cai dat
                Properties.Settings.Default.COMCuaVao = cbbCongComCuaVao.SelectedItem.ToString();
                Properties.Settings.Default.COMCuaRa = cbbCongComCuaRa.SelectedItem.ToString();
                Properties.Settings.Default.Save();

                // Thông báo thành công
                var thongBao = new MessageWindow("Kết nối thành công !", PackIconKind.Done);
                BtnHuyCaiDat_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message, "Thất bại !");
            }
        }

        private string DinhDangTien(string money)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", Convert.ToInt32(money));
        }

        #region 0. Init LoopBack Charts
        public SeriesCollection BienDongGuiXeSeriesCollection { get; set; }

        public Func<double, string> YFormatter { get; set; }
        public List<string> XLabels { get; set; }

        private int soDiemToiDaTrenBieuDo = 30;
        public int SoDiemToiDaTrenBieuDo
        {
            get { return soDiemToiDaTrenBieuDo; }
            set { soDiemToiDaTrenBieuDo = value; }
        }

        public void KhoiTaoBieuDo()
        {
            BienDongGuiXeSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Trung bình trong tuần",
                    PointGeometry =DefaultGeometries.Square,
                    Values = new ChartValues<double>() {},
                    PointGeometrySize = 6
                },
                new LineSeries
                {
                    Title = "Lượng gửi",
                    PointGeometry =DefaultGeometries.Square,
                    Values = new ChartValues<double>() {},
                    PointGeometrySize = 6
                }
            };

            XLabels = new List<string>();
            YFormatter = value => value.ToString();
        }

        private Dictionary<DateTime,int> LaySoLuongTongHopGuiTrongTuan(DateTime dateNow)
        {
            Dictionary<DateTime, int> result = new Dictionary<DateTime, int>();
            for (int i = -6; i <= 0; i++)
            {
                var date = dateNow.AddDays(i);
                string strDate = date.ToString("yyyy-MM-dd");
                result.Add(date, sqlUtility.SoLuotGuiTheoNgay(strDate));
            }
            return result;
        }
        private void TaiBieuDo()
        {
            Dictionary<DateTime, int> tongHopSoLuongGuiTrongTuan = LaySoLuongTongHopGuiTrongTuan(DateTime.Now);
            double luongGuiTrungBinhTrongTuan = tongHopSoLuongGuiTrongTuan.Average(t => t.Value);

            foreach (var item in tongHopSoLuongGuiTrongTuan.OrderBy(kp => kp.Key))
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    if (BienDongGuiXeSeriesCollection[0].Values.Count > SoDiemToiDaTrenBieuDo)
                    {
                        BienDongGuiXeSeriesCollection[0].Values.RemoveAt(0);
                        BienDongGuiXeSeriesCollection[1].Values.RemoveAt(0);
                        XLabels.RemoveAt(0);
                    }

                    BienDongGuiXeSeriesCollection[0].Values.Add(Convert.ToDouble(item.Value));
                    BienDongGuiXeSeriesCollection[1].Values.Add(luongGuiTrungBinhTrongTuan);

                    XLabels.Add(item.Key.ToString("yyyy-MM-dd"));
                }));
            }
        }

        #endregion

        #region Phan Quyen
        private bool nhaPhatTrien;
        public bool NhaPhatTrien
        {
            get { return nhaPhatTrien; }
            set { nhaPhatTrien = value; OnPropertyChanged("NhaPhatTrien"); }
        }

        private bool nhanVienBaiXe;
        public bool NhanVienBaiXe
        {
            get { return nhanVienBaiXe; }
            set { nhanVienBaiXe = value; OnPropertyChanged("NhanVienBaiXe"); }
        }

        private bool sinhVien;
        public bool SinhVien
        {
            get { return sinhVien; }
            set { sinhVien = value; OnPropertyChanged("SinhVien"); }
        }

        private bool giangVien;
        public bool GiangVien
        {
            get { return giangVien; }
            set { giangVien = value; OnPropertyChanged("GiangVien"); }
        }

        private bool khach;
        public bool Khach
        {
            get { return khach; }
            set { khach = value; OnPropertyChanged("Khach"); }
        }

        private bool quanTriVien;
        public bool QuanTriVien
        {
            get { return quanTriVien; }
            set { quanTriVien = value; OnPropertyChanged("QuanTriVien"); }
        }
        #endregion

        private void StackPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Chức năng đang phát triển. Quay lại sau.", "Thông báo");
        }


    }
}
