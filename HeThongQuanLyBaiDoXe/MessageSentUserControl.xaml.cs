using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HeThongQuanLyBaiDoXe
{
    /// <summary>
    /// Interaction logic for MessageSentUserControl.xaml
    /// </summary>
    public partial class MessageSentUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MessageSentUserControl(string userName,string time,string content)
        {
            InitializeComponent();
            this.DataContext = this;
            this.User = userName;
            this.Time = time;
            this.ContentUsage = content;
        }

        private string user;
        public string User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private string time;
        public string Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged("Time"); }
        }

        private string contentUsage;
        public string ContentUsage
        {
            get { return contentUsage; }
            set { contentUsage = value; OnPropertyChanged("ContentUsage"); }
        }
    }
}
