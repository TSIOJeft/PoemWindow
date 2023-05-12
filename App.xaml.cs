using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace LitePeom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [DllImport("user32")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsRunning())
            {
                Current.Shutdown();
            }
            //
            // base.OnStartup(e);
        }

        /// <summary>判断单实例程序是否正在运行</summary>
        /// <remarks>若正在运行激活窗口</remarks>
        /// copy from https://github.com/BluePointLilac/ContextMenuManager 
        public static bool IsRunning()
        {
            using (Process current = Process.GetCurrentProcess())
            {
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    using (process)
                    {
                        if (process.Id == current.Id) continue;
                        if (process.MainModule.FileName == current.MainModule.FileName)
                        {
                            const int SW_RESTORE = 9;
                            ShowWindowAsync(process.MainWindowHandle, SW_RESTORE);
                            SetForegroundWindow(process.MainWindowHandle);
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}