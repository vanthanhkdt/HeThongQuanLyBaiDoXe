using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HeThongQuanLyBaiDoXe.AccoundData;

namespace HeThongQuanLyBaiDoXe
{
    public static class Table
    {
        public static Registers ParseRegister(DataRow row)
        {
            var stt = row["STT"];
            var hoTen = row["HoTen"];
            var maSo = row["MaSo"];
            var matKhau = row["MatKhau"];
            var khoaLop = row["KhoaLop"];
            var maTheGui = row["MaTheGui"];
            var phanQuyen = row["PhanQuyen"];
            var lyDo = row["LyDo"];
            var daXuLy = row["DaXuLy"];
            var daNop = row["DaNop"];
            var hinhAnh = row["HinhAnh"] == System.DBNull.Value ? null : row["HinhAnh"];

            return new Registers(stt, hoTen, maSo, matKhau, khoaLop, maTheGui, phanQuyen, lyDo, daXuLy, daNop, hinhAnh);
        }
        public static Users ParseUser(DataRow row)
        {
            var stt = row["STT"];
            var hoTen = row["HoTen"];
            var maSo = row["MaSo"];
            var matKhau = row["MatKhau"];
            var khoaLop = row["KhoaLop"];
            var maTheGui = row["MaTheGui"];
            var phanQuyen = row["PhanQuyen"];
            var choPhepHoatDong = row["ChoPhepHoatDong"];
            var nguoiThem = row["ChoPhepHoatDong"];
            var ngayThem = row["ChoPhepHoatDong"];
            var soDuKhaDung = row["SoDuKhaDung"];
            var dangGui = row["DangGui"];
            var truyCapLanCuoi = row["TruyCapLanCuoi"];
            var guiLanCuoi = row["ThoiGianGuiCuoi"];
            var hinhAnh = row["HinhAnh"] == System.DBNull.Value ? null : row["HinhAnh"];
            var donGia = row["DonGia"];

            return new Users(stt, hoTen, maSo, matKhau, khoaLop, maTheGui, phanQuyen, choPhepHoatDong, nguoiThem, ngayThem, soDuKhaDung, dangGui,
                truyCapLanCuoi, guiLanCuoi, hinhAnh, donGia);
        }

        public static string LayTenPhanQuyenTuTen(string name)
        {
            switch (name)
            {
                case "NhaPhatTrien": return "Nhà phát triển";
                case "GiangVien": return "Giảng viên";
                case "SinhVien": return "Sinh viên";
                case "NhanVienBaiXe": return "Nhân viên Bãi xe";
                case "Khach": return "Khách";
                case "QuanTriVien": return "Quản trị viên";
                default: return "(N/A)";
            }
        }
        public static string LayTenPhanQuyenTuMa(string code)
        {
            switch (code)
            {
                case "0": return "Nhà phát triển";
                case "3": return "Giảng viên";
                case "2": return "Sinh viên";
                case "1": return "Nhân viên Bãi xe";
                case "4": return "Khách";
                case "5": return "Quản trị viên";
                default: return "(N/A)";
            }
        }
        public static string LayMaPhanQuyen(string name)
        {
            switch (name)
            {
                case "Nhà phát triển": return "0";
                case "Nhân viên Bãi xe": return "1";
                case "Sinh viên": return "2";
                case "Giảng viên": return "3";
                case "Khách": return "4";
                case "Quản trị viên": return "5";
                default: return "0";
            }
        }

        public static string[] PhanQuyen = new[] { "PhanQuyen", "MaPhanQuyen"};
        public static string[] Users = new[] { //"STT",
            "HoTen",
            "MaSo",
            "MatKhau",
            "KhoaLop",
            "MaTheGui",
            "PhanQuyen",
            "ChoPhepHoatDong",
            "NguoiThem",
            "NgayThem",
            "SoDuKhaDung",
            "DangGui",
            "TruyCapLanCuoi",
            "ThoiGianGuiCuoi",
            "HinhAnh",
            "DonGia"};
        /// <summary>
        ///  hoTen,maSo,khoaLop,maTheGui,matKhau,phanQuyen,lyDo,daXuLy,daNop,hinhAnh 
        /// </summary>
        public static string[] Registration = new[] {//"STT",
            "HoTen",
            "MaSo",
            "KhoaLop",
            "MaTheGui",
            "MatKhau",
            "PhanQuyen",
            "LyDo",
            "DaXuLy",
            "DaNop",
            "HinhAnh",
        };

        /// <summary>
        /// Card List
        /// </summary>
        public static string[] CardList = new[] {//"STT",
            "SoSeri",
            "MaThe",
            "GiaTri",
            "NgayKichHoat",
            "DaKichHoat",
            "TaiKhoanKichHoat",
            "ChoPhepHoatDong"};

        /// <summary>
        /// The Tam Thoi
        /// </summary>
        public static string[] TheTamThoi = new[] {//"STT",
            "SoThe",
            "MaThe",
            "DangGui",
            "ChoPhepHoatDong",
            "ThoiGianGuiCuoi",
            "ThoiGianTraCuoi",
            "DonGia"};

        public static string[] Activities = new[] { //"STT",
            "MaSo",
            "HoatDong",
            "ThanhCong",
            "ThoiGian",
            "NoiDung",
            "MaTheNap",
            "SoTienNap"
        };

        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string XoaChuCoDauDeHienThiLCD(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }
    public enum TableName
    {
        Users = 0,
        Registration,
        TheTamThoi,
        CardList,
        Activities
    }
}
