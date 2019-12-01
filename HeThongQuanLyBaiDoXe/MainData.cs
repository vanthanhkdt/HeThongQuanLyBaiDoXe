using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeThongQuanLyBaiDoXe
{
    public class AccoundData
    {
        public class Registers
        {
            public Registers(object sTT, object hoTen, object maSo, object matKhau, object khoaLop, object maTheGui, object phanQuyen, object lyDo, object daXuLy, object daNop, object hinhAnh)
            {
                STT = sTT.ToString();
                HoTen = hoTen.ToString();
                MaSo = maSo.ToString();
                MatKhau = matKhau.ToString();
                KhoaLop = khoaLop.ToString();
                MaTheGui = maTheGui.ToString();
                PhanQuyen = phanQuyen.ToString();
                LyDo = lyDo.ToString();
                DaXuLy = daXuLy.ToString();
                DaNop = daNop.ToString();
                HinhAnh = (byte[])hinhAnh;
            }

            public string STT { get; set; }
            public string HoTen { get; set; }
            public string MaSo { get; set; }
            public string MatKhau { get; set; }
            public string KhoaLop { get; set; }
            public string MaTheGui { get; set; }
            public string PhanQuyen { get; set; }
            public string LyDo { get; set; }
            public string DaXuLy { get; set; }
            public string DaNop { get; set; }
            public byte[] HinhAnh { get; set; }
            public bool Color { get; set; }
        }

        public class Users
        {
            public Users()
            { }
            public Users(object sTT, object hoTen, object maSo, object matKhau, object khoaLop, object maTheGui, object phanQuyen, object choPhepHoatDong,
                object nguoiThem, object ngayThem, object soDuKhaDung, object dangGui, object truyCapLanCuoi, object thoiGianGuiCuoi, object hinhAnh,object donGia)
            {
                STT = sTT.ToString();
                HoTen = hoTen.ToString();
                MaSo = maSo.ToString();
                MatKhau = matKhau.ToString();
                KhoaLop = khoaLop.ToString();
                MaTheGui = maTheGui.ToString();
                PhanQuyen = phanQuyen.ToString();
                ChoPhepHoatDong = choPhepHoatDong.ToString();
                NguoiThem = nguoiThem.ToString();
                NgayThem = ngayThem.ToString();
                SoDuKhaDung = soDuKhaDung.ToString();
                DangGui = dangGui.ToString();
                TruyCapLanCuoi = truyCapLanCuoi.ToString();
                ThoiGianGuiCuoi = thoiGianGuiCuoi.ToString();
              
                HinhAnh = hinhAnh==System.DBNull.Value?null: (byte[])hinhAnh;
                DonGia = donGia.ToString();
                Color = (bool)choPhepHoatDong;
            }

            public string STT { get; set; }
            public string HoTen { get; set; }
            public string MaSo { get; set; }
            public string MatKhau { get; set; }
            public string KhoaLop { get; set; }
            public string MaTheGui { get; set; }
            public string PhanQuyen { get; set; }
            public string ChoPhepHoatDong { get; set; }
            public string NguoiThem { get; set; }
            public string NgayThem { get; set; }
            public string SoDuKhaDung { get; set; }
            public string DangGui { get; set; }
            public string TruyCapLanCuoi { get; set; }
            public string ThoiGianGuiCuoi { get; set; }
            public byte[] HinhAnh { get; set; }
            public string DonGia { get; set; }
            public bool Color { get; set; }
        }
    }
    public class TaoMaThe
    {
        public TaoMaThe(string sTT, string seRi, string maTheNap, string menhGia, string trangThai)
        {
            STT = sTT;
            SeRi = seRi;
            MaTheNap = maTheNap;
            MenhGia = menhGia;
            TrangThai = trangThai;
        }

        public string STT { get; set; }
        public string SeRi { get; set; }
        public string MaTheNap { get; set; }
        public string MenhGia { get; set; }
        public string TrangThai { get; set; }
    }
}
