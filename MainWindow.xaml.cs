using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Windows.Media;
using Newtonsoft.Json;

namespace LitePeom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ConfigArray config;
        private int refersh;

        public MainWindow()
        {
            ShowInTaskbar = false;
            ShowActivated = false;
            InitializeComponent();
            initConfig();
            this.ShowInTaskbar = false;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 20;
            this.Top = desktopWorkingArea.Top + 20;
            var helper = new WindowInteropHelper(this).Handle;
            SetWindowLong(helper, GWL_EX_STYLE,
                (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            showWindow();
        }

        private static bool peomShowed;

        private void initConfig()
        {
            this.config = new ConfigUtil().loadConfig();
            this.Width = config.WindowWidth;
            this.Height = config.WindowHeight;
            this.refersh = config.Time;
            peomText.FontSize = config.FontSize;
            if (config.Font != null && config.Font.Length > 0)
            {
                peomText.FontFamily = new FontFamily(config.Font);
            }
            
            // new ConfigUtil().saveConfig(config);
        }

        public void showWindow()
        {
            updatePeom(null, null);
            this.Show();
            peomShowed = true;
            time();
        }

        private void time()
        {
            Timer timer = new Timer();
            timer.Interval = refersh;
            timer.Tick += updatePeom;
            timer.Start();
        }

        private void updatePeom(object sender, EventArgs args)
        {
            peomText.Text = PeomGet();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

        public string PeomGet()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(config.PeomUrl);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                dynamic magic = JsonConvert.DeserializeObject(retString);
                return magic[config.PeomKey];
            }

            return "";
        }
    }
}