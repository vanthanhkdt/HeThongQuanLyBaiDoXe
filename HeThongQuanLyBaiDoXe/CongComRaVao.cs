using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HeThongQuanLyBaiDoXe
{
    public delegate void KiemTra(LoaiCongRaVao loaiCong, string duLieu);
    public delegate void LayMaThe(string duLieu);
    public class CongComRaVao
    {
        private string tenCongCom;
        private SerialPort congCom;
        public KiemTra TienHanhKiemTra;
        public LayMaThe HanhDongLayMaTheTamThoi;
        private LoaiCongRaVao loaiCongRaVao;

        public CongComRaVao(LoaiCongRaVao loaiCong, string ten)
        {
            this.loaiCongRaVao = loaiCong;
            this.tenCongCom = ten;
        }

        public bool BatDauKetNoi()
        {
            congCom = new SerialPort(tenCongCom, 9600, Parity.None, 8, StopBits.One);
            try
            {
                congCom.Open();
                congCom.DataReceived += Port_DataReceived;
                congCom.DiscardInBuffer();
                var thongBao = new MessageWindow("Kết nối thành công !");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mở cổng COM thất bại: " + tenCongCom + " " + ex.Message, "Thất bại !");
            }
            return false;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="duLieuGui"></param>
        /// <returns></returns>
        public bool Gui(string duLieuGui)
        {
            try
            {
                if (congCom != null && congCom.IsOpen)
                {
                    congCom.Write(duLieuGui + "\r");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi thất bại: " + tenCongCom + " " + ex.Message, "Thất bại !");
                return false;
            }
        }
        public bool PhanHoiHanhDong(HoatDong hoatDong, bool thanhCong, string thongBao)
        {
            string duLieuGui = string.Empty;
            switch (hoatDong)
            {
                case HoatDong.NapThe:
                    duLieuGui += "00"; //Nap the: 00
                    if (thanhCong)
                    {
                        duLieuGui += "1"; // Neu thanh cong, them '1' vao chuoi
                    }
                    else
                    {
                        duLieuGui += "0"; // Neu that bai, them '0' vao chuoi
                    }
                    break;
                case HoatDong.Vao:
                    duLieuGui += "01"; //Vao: 01
                    if (thanhCong)
                    {
                        duLieuGui += "1"; // Neu thanh cong, them '1' vao chuoi
                    }
                    else
                    {
                        duLieuGui += "0"; // Neu that bai, them '0' vao chuoi
                    }
                    break;
                case HoatDong.Ra:
                    duLieuGui += "10"; //Ra: 10
                    if (thanhCong)
                    {
                        duLieuGui += "1"; // Neu thanh cong, them '1' vao chuoi
                    }
                    else
                    {
                        duLieuGui += "0"; // Neu that bai, them '0' vao chuoi
                    }
                    break;
                default:
                    break;
            }
            duLieuGui += thongBao; // Du lieu hien thi LCD
            try
            {
                if (congCom != null && congCom.IsOpen)
                {
                    congCom.Write(duLieuGui + "\r"); // Thêm ký tự '\r' (ký tự kết thúc chuỗi) để mạch không bị đơ.
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi thất bại: " + tenCongCom + " " + ex.Message, "Thất bại !");
                return false;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(150); // Chờ để nhận dữ liệu xong.
            string duLieuNhanDuoc = congCom.ReadExisting();

            //TODO:
            TienHanhKiemTra.Invoke(loaiCongRaVao, duLieuNhanDuoc);
            HanhDongLayMaTheTamThoi.Invoke(duLieuNhanDuoc);
        }

        public bool DangMo()
        {
            return !(congCom == null || !congCom.IsOpen);
        }
        public void ClosePort()
        {
            try
            {
                congCom.Close();
                var thongBao = new MessageWindow($"Ngắt kết nối {congCom.PortName} thành công !");
            }
            catch (Exception e)
            {
                var thongBao = new MessageWindow($"Ngắt kết nối {congCom.PortName} thất bại: {e.Message}");
            }
        }

        public string GetPortName()
        {
            return tenCongCom;
        }
    }

    public enum LoaiCongRaVao
    {
        Ra,
        Vao
    }

    public enum HoatDong
    {
        NapThe = 0,
        Vao,
        Ra,
        Khac
    }
}
