using CefSharp;
using CefSharp.WinForms;
using Microsoft.Owin.Hosting;
using System;
using System.Windows.Forms;

namespace GameServer
{
    public partial class GameServerForm : Form
    {
        private Server Server { get; set; }
        private bool Running { get; set; }

        public static ChromiumWebBrowser ChromeBrowser;

        private delegate void TextDelegate(Control control, string text, bool append);
        private delegate void EnableDelegate(Control control, bool enable);

        private TextDelegate UpdateText;
        private EnableDelegate UpdateEnabled;

        private void SetText(Control control, string text, bool append) { if (append) control.Text += text + "\r\n"; else control.Text = text; }
        private void SetEnabled(Control control, bool enabled) { control.Enabled = enabled; }

        public GameServerForm()
        {
            InitializeComponent();
        }

        private void GameServerForm_Load(object sender, EventArgs e)
        {
            Running = false;

            UpdateText = new TextDelegate(SetText);
            UpdateEnabled = new EnableDelegate(SetEnabled);

            WriteToLog("Starting browser...");
            InitializeChromium();
        }
        private void GameServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();

            if (Server != null && !Server.IsDisposed)
                Server.Dispose();
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            ChromeBrowser = new ChromiumWebBrowser("");
            ChromeBrowser.LoadHtml(Properties.Resources.index);
            ChromeBrowser.MenuHandler = new CustomMenuHandler();
            // Add it to the form and fill it to the form window.
            pnlBrowser.Controls.Add(ChromeBrowser);
            ChromeBrowser.Dock = DockStyle.Fill;
            ChromeBrowser.LoadingStateChanged += ChromeBrowser_LoadingStateChanged;

            ChromeBrowser.RegisterJsObject("cefCustomObject", new CefCustomObject(ChromeBrowser, this));

        }
        private void ChromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                WriteToLog("Browser loaded...");
                Invoke(UpdateEnabled, new object[] { btnHostServer, true });
                Invoke(UpdateEnabled, new object[] { mnuMain, true });

                ChromeBrowser.ShowDevTools();
            }
        }

        private void StartServer()
        {
            try
            {
                Server = new Server();
                WriteToLog($"Server hosted on => " + Server.Name);

                WebApp.Start<Startup>(Server.Url);
                Game.Map.Generate();

                ChromeBrowser.GetMainFrame().ExecuteJavaScriptAsync($"DrawMap({ Newtonsoft.Json.JsonConvert.SerializeObject(Game.Map) });");
            }
            catch (Exception ex)
            {
                Running = false;

                WriteToLog(ex.Message + "\r\n" + ex.InnerException);

                Invoke(UpdateText, new object[] { btnHostServer, "Host Server", false });
                Invoke(UpdateEnabled, new object[] { tbxWatchURL, true });
                Invoke(UpdateEnabled, new object[] { btnWatchServer, true });
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            GameServerSettingsForm settings = new GameServerSettingsForm();
            settings.ShowDialog();
        }
        private void FullScreen_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        private void Windowed_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void HostServer_Click(object sender, EventArgs e)
        {
            if (!Running)
            {
                StartServer();
                Running = true;

                Invoke(UpdateText, new object[] { btnHostServer, "Stop Server", false });
                Invoke(UpdateEnabled, new object[] { tbxWatchURL, false });
                Invoke(UpdateEnabled, new object[] { btnWatchServer, false });
            }
            else
            {
                if (Server != null && !Server.IsDisposed)
                    Server.Dispose();

                Running = false;

                Invoke(UpdateText, new object[] { btnHostServer, "Host Server", false });
                Invoke(UpdateEnabled, new object[] { tbxWatchURL, true });
                Invoke(UpdateEnabled, new object[] { btnWatchServer, true });
            }
        }

        private void WriteToLog(string message)
        {
            Invoke(UpdateText, new object[] { tbxLog, message, true });
        }
    }
}
