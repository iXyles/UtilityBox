using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ServerApp = UtilityBox.App.Server.ServerApp;

namespace UtilityBox.App.Windows
{
    static class UtilityBoxProgram
    {
        private static string UtilityBoxApplicationGuid = "c47f2a513-7b91-4f89-841d-f935a6a5286a";
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using Mutex mutex = new Mutex(false, "Global\\" + UtilityBoxApplicationGuid);
            if(!mutex.WaitOne(0, false))
            {
                MessageBox.Show("Instance already running, please close it if you are trying to restart it.");
                return;
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UtilityBoxApplication());
        }
    }

    internal class UtilityBoxApplication : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private string _serverAddress = string.Empty;
        
        public UtilityBoxApplication()
        {
            var server = new ServerApp();

            using var stream = File.OpenRead($"{AppContext.BaseDirectory}/icon.ico");
            var icon = new Icon(stream);

            _notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = icon,
                ContextMenuStrip  = new ContextMenuStrip()
                {
                    Items =
                    {
                        { "Open", null, OpenApplication },
                        { "Exit", null, ExitApplication }
                    }
                }
            };
            Application.ApplicationExit += OnApplicationExit;
            _notifyIcon.DoubleClick += OpenApplication;
            server.StartWebServer(address =>
            {
                _serverAddress = address;
                Process.Start(new ProcessStartInfo("cmd", $"/c start {address}") {CreateNoWindow = true});
            });
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                _notifyIcon.Icon.Dispose();
                _notifyIcon.Icon = null;
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            } catch { /* ignored */}
        }

        private void OpenApplication(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_serverAddress))
                MessageBox.Show("System is not ready... Please wait.");
            else
                Process.Start(new ProcessStartInfo("cmd", $"/c start http://localhost:8080") {CreateNoWindow = true});
        }

        private void ExitApplication(object sender, EventArgs e) => Environment.Exit(1);
    }
}