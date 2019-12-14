using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace HeThongQuanLyBaiDoXe
{
    public class SQLUtility : IDisposable
    {
        #region IDisposable Members

        bool disposed = false;
        SafeHandle handler = new SafeFileHandle(IntPtr.Zero, true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                handler.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public string ConnenctionString { get; set; } = @"Data Source = DESKTOP-JM571ID\SQLEXPRESS; Initial Catalog = DBBaiDoXe; User id = doantotnghiepbaidoxe; Password = baidoxe!@#$;";
        private SqlConnection sqlConnection;

        public SQLUtility()
        {
        }

        public SQLUtility(string connenctionString)
        {
            this.ConnenctionString = connenctionString;
        }
        // Open Connection
        public void Connect()
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = new SqlConnection(this.ConnenctionString);
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // Close Connection
        public void Disconnect()
        {
            if ((sqlConnection != null) && (sqlConnection.State == ConnectionState.Open))
                sqlConnection.Close();
        }

        // Return A DataTable
        public DataTable GetDataTable(string sql)
        {
            Connect();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlConnection);
            DataTable dt = new DataTable();
            if (dt != null)
            {
                da.Fill(dt);
            }

            Disconnect();
            return dt;
        }

        public int KiemTraSoDuKhaDung(string maSo)
        {
            string commandText = $"SELECT * FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE MaSo = N'{maSo}' AND ChoPhepHoatDong=1;";

            Connect();
            SqlDataAdapter da = new SqlDataAdapter(commandText, sqlConnection);
            DataTable dt = new DataTable();
            if (dt != null)
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["SoDuKhaDung"]); // Trả về Số dư khả dụng
            }

            Disconnect();
            return -1; // Tài khoản không tồn tại hoặc không được phép hoạt động.
        }

        /// <summary>
        /// Kiểm tra đăng nhập
        /// </summary>
        /// <param name="maSo">Mã số</param>
        /// <param name="matKhau">Mật khẩu</param>
        /// <param name="dt">giá trị trả về</param>
        /// <returns></returns>
        public string KiemTraDangNhap(string maSo, string matKhau, ref DataTable dt)
        {
            string commandText = $"SELECT * FROM TBUsers WHERE MaSo=@MaSo AND MatKhau=@MatKhau AND ChoPhepHoatDong=1;";
            using (SqlConnection connection = new SqlConnection(ConnenctionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@MaSo", maSo);
                        command.Parameters.AddWithValue("@MatKhau", matKhau);
                        //da.SelectCommand.Parameters.Add(new SqlParameter {ParameterName="@MatKhau",Value=matKhau,SqlDbType=SqlDbType.NVarChar,Size=-1 });
                        //da.SelectCommand.Parameters.AddWithValue("@MatKhau", matKhau);

                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            if (dt != null)
                            {
                                dt = GetDataTable($"SELECT * FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE MaSo = '{maSo}' AND MatKhau='{matKhau}' AND ChoPhepHoatDong=1;");
                                if (dt.Rows.Count > 0)
                                    return string.Empty;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return ex.Message;
                    }
                }
            }
            return "Tài khoản hoặc Mật khẩu không đúng.";
        }

        public string KiemTraRaVao(string maSo, ref DataTable dt)
        {
            string commandText = $"SELECT * FROM TBUsers WHERE MaSo=@MaSo AND ChoPhepHoatDong=1;";
            using (SqlConnection connection = new SqlConnection(ConnenctionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@MaSo", maSo);
                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            if (dt != null)
                            {
                                dt = GetDataTable($"SELECT * FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE MaSo = '{maSo}' AND ChoPhepHoatDong=1;");
                                if (dt.Rows.Count > 0)
                                    return string.Empty;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "Thất bại: " + ex.Message;
                    }
                }
            }
            return "Thất bại";
        }

        /// <summary>
        /// Kiem tra ma the nap
        /// </summary>
        /// <param name="maSo"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string KiemTraMaTheNap(string maTheNap, ref DataTable dt)
        {
            string commandText = $"SELECT * FROM [DBBaiDoXe].[dbo].[TBCardList] WHERE MaThe=@MaThe AND ChoPhepHoatDong=1 AND DaKichHoat=0;";
            using (SqlConnection connection = new SqlConnection(ConnenctionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@MaThe", maTheNap);
                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            if (dt != null)
                            {
                                dt = GetDataTable($"SELECT * FROM [DBBaiDoXe].[dbo].[TBCardList] WHERE MaThe = '{maTheNap}' AND ChoPhepHoatDong=1 AND DaKichHoat=0;");
                                if (dt.Rows.Count > 0)
                                    return string.Empty;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "Thất bại: " + ex.Message;
                    }
                }
            }
            return "Thất bại";
        }

        public DataTable GetDataTable(TableName tableName)
        {
            Connect();
            string table = string.Empty;
            switch (tableName)
            {
                case TableName.Users:
                    table = "TBUsers";
                    break;
                case TableName.Registration:
                    table = "TBRegistration";
                    break;
                case TableName.TheTamThoi:
                    table = "TBTheTamThoi";
                    break;
                case TableName.CardList:
                    table = "TBCardList";
                    break;
                case TableName.Activities:
                    table = "TBActivities";
                    break;
                default:
                    break;
            }
            StringBuilder cmd = new StringBuilder($"SELECT * FROM [DBBaiDoXe].[dbo].[{table}]");
            SqlDataAdapter da = new SqlDataAdapter(cmd.ToString(), sqlConnection);
            DataTable dt = new DataTable();
            if (dt != null)
            {
                da.Fill(dt);
            }

            Disconnect();
            return dt;
        }

        // Execute Query Commands: Insert, Delete, Update
        public void ExecuteNonQuery(string sql)
        {
            Connect();
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            Disconnect();
        }

        // Return A DataReader
        public SqlDataReader GetDataReader(string sql)
        {
            Connect();
            SqlCommand com = new SqlCommand(sql, sqlConnection);
            SqlDataReader dr = com.ExecuteReader();
            return dr;
        }


        // Insert Data Function
        public string InsertWithImage(TableName tableName, string[] fields, string[] values)
        {
            try
            {
                //int filLength = 0;
                //byte[] byteImageArray;
                //byteImageArray = new byte[Convert.ToInt32(filLength)];

                Connect();
                SqlCommand command;
                SqlDataAdapter adapter;
                //SqlDataReader reader = null;
                string commandText = string.Empty;
                string table = string.Empty;
                switch (tableName)
                {
                    case TableName.Users:
                        table = "TBUsers";
                        break;
                    case TableName.Registration:
                        table = "TBRegistration";
                        break;
                    case TableName.TheTamThoi:
                        table = "TBTheTamThoi";
                        break;
                    default:
                        break;
                }
                StringBuilder builder = new StringBuilder($"INSERT INTO [DBBaiDoXe].[dbo].[{table}](");

                for (int i = 0; i < fields.Length; i++)
                {
                    builder.Append(i == fields.Length - 1 ? $"[{fields[i]}]) VALUES(" : $"[{fields[i]}],");
                }
                for (int i = 0; i < values.Length; i++)
                {
                    builder.Append(i != values.Length - 1 ? $"@{fields[i]}, " : $"@{fields[i]})");
                }
                commandText = builder.ToString();
                command = new SqlCommand(commandText, sqlConnection);

                for (int i = 0; i < values.Length; i++)
                {
                    if (fields[i] == "HinhAnh")
                        command.Parameters.AddWithValue(fields[i], string.IsNullOrEmpty(values[i]) ? (object)DBNull.Value : GetData(values[i])).SqlDbType = SqlDbType.Image;
                    else
                        command.Parameters.AddWithValue(fields[i], values[i]);
                }

                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand(commandText, sqlConnection);
                //adapter.InsertCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                command.Dispose();
                sqlConnection.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string InsertWithImageFromDB(TableName tableName, string[] fields, object[] values)
        {
            try
            {
                Connect();
                SqlCommand command;
                SqlDataAdapter adapter;
                string commandText = string.Empty;
                string table = string.Empty;
                switch (tableName)
                {
                    case TableName.Users:
                        table = "TBUsers";
                        break;
                    case TableName.Registration:
                        table = "TBRegistration";
                        break;
                    case TableName.TheTamThoi:
                        table = "TBTheTamThoi";
                        break;
                    default:
                        break;
                }
                StringBuilder builder = new StringBuilder($"INSERT INTO [DBBaiDoXe].[dbo].[{table}](");

                for (int i = 0; i < fields.Length; i++)
                {
                    builder.Append(i == fields.Length - 1 ? $"[{fields[i]}]) VALUES(" : $"[{fields[i]}],");
                }
                for (int i = 0; i < values.Length; i++)
                {
                    builder.Append(i != values.Length - 1 ? $"@{fields[i]}, " : $"@{fields[i]})");
                }
                commandText = builder.ToString();
                command = new SqlCommand(commandText, sqlConnection);

                for (int i = 0; i < values.Length; i++)
                {
                    command.Parameters.AddWithValue(fields[i], values[i]);
                }

                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand(commandText, sqlConnection);
                //adapter.InsertCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                command.Dispose();
                sqlConnection.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string Insert(TableName tableName, string[] fields, string[] values)
        {
            try
            {
                Connect();
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string commandText = string.Empty;
                string table = string.Empty;
                switch (tableName)
                {
                    case TableName.Users:
                        table = "TBUsers";
                        break;
                    case TableName.Registration:
                        table = "TGRegistration";
                        break;
                    case TableName.TheTamThoi:
                        table = "TBTheTamThoi";
                        break;
                    case TableName.CardList:
                        table = "TBCardList";
                        break;
                    case TableName.Activities:
                        table = "TBActivities";
                        break;
                    default:
                        break;
                }
                StringBuilder builder = new StringBuilder($"INSERT INTO [DBBaiDoXe].[dbo].[{table}](");

                for (int i = 0; i < fields.Length; i++)
                {
                    builder.Append(i == fields.Length - 1 ? $"[{fields[i]}]) VALUES(" : $"[{fields[i]}],");
                }
                for (int i = 0; i < values.Length; i++)
                {
                    builder.Append(i != values.Length - 1 ? $"N'{values[i]}', " : $"N'{values[i]}')");
                }
                commandText = builder.ToString();
                command = new SqlCommand(commandText, sqlConnection);
                adapter.InsertCommand = new SqlCommand(commandText, sqlConnection);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
                sqlConnection.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public IEnumerable<string> GetDistinct(TableName tableName, string field)
        {
            string table = string.Empty;
            switch (tableName)
            {
                case TableName.Users:
                    table = "TBUsers";
                    break;
                case TableName.Registration:
                    table = "TGRegistration";
                    break;
                case TableName.TheTamThoi:
                    table = "TBTheTamThoi";
                    break;
                default:
                    break;
            }
            DataTable dt = this.GetDataTable($"SELECT DISTINCT {field} FROM [DBBaiDoXe].[dbo].[{table}]");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    yield return row[field].ToString();
                }
            }
        }

        public string DangKyTaiKhoan(string hoTen, string maSo, string khoaLop, string maTheGui, string matKhau, string phanQuyen, string lyDo, string daXuLy, string daNop, string pathHinhAnh)
        {
            string result = this.InsertWithImage(TableName.Registration, Table.Registration, new[] { hoTen, maSo, khoaLop, maTheGui, CreateMD5Hash(matKhau), Table.LayMaPhanQuyen(phanQuyen), lyDo, "FALSE", daNop, pathHinhAnh });
            if (result == string.Empty)
                return string.Empty;
            return result;
        }

        // Cập nhật đơn giá
        public void CapNhatDonGia(string maSo, string donGia)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "UPDATE [DBBaiDoXe].[dbo].[TBUsers] SET DonGia= '" + donGia + "' WHERE MaSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }

        // Cập nhật số dư khả dụng
        public void CapNhatSoDuKhaDung(HoatDong hoatDong, string maSo, string strSoTien)
        {
            int soDuKhaDung = KiemTraSoDuKhaDung(maSo);
            int soTien = Convert.ToInt32(strSoTien);
            switch (hoatDong)
            {
                case HoatDong.NapThe:
                    soDuKhaDung += soTien;
                    break;
                case HoatDong.Vao:
                    soDuKhaDung -= soTien;
                    break;
                default:
                    break;
            }
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "UPDATE [DBBaiDoXe].[dbo].[TBUsers] SET SoDuKhaDung= '" + soDuKhaDung + "' WHERE MaSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }

        // Cập nhật gửi/trả xe
        public void CapNhatGuiTraXe(string maSo, HoatDong hoatDong)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "UPDATE [DBBaiDoXe].[dbo].[TBUsers] SET DangGui= '" + (hoatDong == HoatDong.Vao ? "1" : "0") + "' WHERE MaSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }

        // Cập nhật truy cập lần cuối
        public void CapNhatTruyCapHeThongLanCuoi(string maSo)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "UPDATE [DBBaiDoXe].[dbo].[TBUsers] SET TruyCapLanCuoi=GETDATE() WHERE MaSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }

        // Cập nhật thời gian gửi cuối
        public void CapNhatThoiGianGuiCuoi(string maSo)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "UPDATE [DBBaiDoXe].[dbo].[TBUsers] SET ThoiGianGuiCuoi=GETDATE() WHERE MaSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }
        // Cập nhật nạp thẻ thành công
        public void CapNhatNapTheThanhCong(string maSo, string maTheNap)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = $"UPDATE [DBBaiDoXe].[dbo].[TBCardList] SET DaKichHoat='1', TaiKhoanKichHoat=N'{maSo}', NgayKichHoat=GETDATE() WHERE MaThe=N'{maTheNap}';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }
        // Xoa
        public void XoaNguoiGui(string maSo)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "DELETE FROM [DBBaiDoXe].[dbo].[TBUsers] WHERE maSo=N'" + maSo + "';";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }

        // Insert LOGs
        public void InsertLog(string log)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "INSERT INTO HISTORYLOG (_date,_log) VALUES (GETDATE(), N'" + log + "')";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }


        // Approve Data Function
        public string ApproveUser(object hoTen, object maSo, object matKhau, object khoaLop, object maTheGui, object phanQuyen, object choPhepHoatDong,
            object nguoiThem, object ngayThem, object soDuKhaDung, object dangGui, object truyCapLanCuoi, object guiLanCuoi, object hinhAnh, object donGia)
        {
            try
            {
                Connect();
                SqlCommand command;
                SqlDataAdapter adapter = new SqlDataAdapter();
                string sql = "UPDATE TBRegistration SET DaXuLy='TRUE' WHERE HoTen=N'" + hoTen + "' AND maSo=N'" + maSo + "';";

                command = new SqlCommand(sql, sqlConnection);
                adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
                sqlConnection.Close();

                string result = this.InsertWithImageFromDB(TableName.Users, Table.Users, new[] {
                hoTen,maSo,matKhau,khoaLop,maTheGui,phanQuyen,choPhepHoatDong,
                nguoiThem,ngayThem,soDuKhaDung,dangGui,truyCapLanCuoi,guiLanCuoi,hinhAnh,donGia
                });

                return result;
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return ex.Message;
            }
        }
        // Delete Account Function
        public void DeleteAccount(string name, string maSo)
        {
            Connect();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "DELETE FROM TBRegistration WHERE HoTen=N'" + name + "' AND MaSo=N'" + maSo + "'; " + "DELETE FROM TBUsers WHERE HoTen=N'" + name + "' AND MaSo=N'" + maSo + "'";
            command = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
            sqlConnection.Close();
        }
        //
        public string UploadFile(string fullpath)
        {
            try
            {
                Connect();
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [HWEMSDB].[dbo].[EMSTRY] ([_TenTapTinDinhKem], [_TapTinDinhKem]) VALUES(@name, @data)", sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Add(new SqlParameter("@name", (object)this.GetName(fullpath)));
                sqlCommand.Parameters.Add(new SqlParameter("@data", (object)this.GetData(fullpath)));
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return "OK";
            }
            catch (SqlException ex)
            {
                return "Error: " + ex.Message;
                //MessageBox.Show(ex.Message);
                //_txtlog.AppendText(ex.Message + "\r\n");
            }
        }
        // String FileName
        public string GetFileNameDB(string sql)
        {
            Connect();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlConnection);
            DataTable dt = new DataTable();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["_TenTapTinDinhKem"].ToString();
            }

            Disconnect();
            return "Error";
        }
        // Byte FileData
        public byte[] GetFileDataDB(string sql)
        {
            Connect();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlConnection);
            DataTable dt = new DataTable();
            if (dt.Rows.Count > 0)
            {
                return (byte[])dt.Rows[0]["_TapTinDinhKem"];
            }
            Disconnect();

            return (byte[])null;
        }
        private string GetName(string fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf("\\") + 1, fullPath.Length - fullPath.LastIndexOf("\\") - 1);
        }

        private byte[] GetData(string path)
        {
            try
            {
                return new BinaryReader((Stream)new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadBytes((int)new FileInfo(path).Length);
            }
            catch (Exception)
            {
                return (byte[])null;
            }
        }

        public string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
