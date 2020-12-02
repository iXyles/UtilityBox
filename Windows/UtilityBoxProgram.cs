using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ServerApp = UtilityBox.App.Server.ServerApp;

namespace UtilityBox.App.Windows
{
    static class UtilityBoxProgram
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
            _notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Shield,
                ContextMenuStrip  = new ContextMenuStrip()
                {
                    Items =
                    {
                        { "Open", null, OpenApplication },
                        { "Exit", null, ExitApplication }
                    }
                }
            };
            Application.ApplicationExit += this.OnApplicationExit;
            _notifyIcon.DoubleClick += OpenApplication;
            server.StartWebServer(address =>
            {
                _serverAddress = address;
                Process.Start(new ProcessStartInfo("cmd", $"/c start {address}") {CreateNoWindow = true});
            });
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            try
            {
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

        private void ExitApplication(object sender, EventArgs e)
        {
            try
            {
                _notifyIcon.Dispose();
            } catch { /* ignored */}
            Environment.Exit(1);
        }
    }
}