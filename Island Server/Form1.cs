using CefSharp;
using CefSharp.WinForms;
using Microsoft.Owin.Hosting;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace IslandServer
{
    public partial class Form1 : Form
    {
        public static ChromiumWebBrowser chromeBrowser;
        public string HostPort = "8080";
        public string HostURL = @"http://localhost/";
        public static Game game;

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public void CreateFirewallRule()
        {
            INetFwRule2 firewallRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Used to allow clients to connect to Island Server";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallRule.Enabled = true;
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRule.LocalPorts = HostPort;
            firewallRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = "Island Server";

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRule);
        }

        public Form1()
        {
            InitializeComponent();
            HostURL = HostURL.Replace("localhost", GetLocalIPAddress() + ":" + HostPort);
            WriteLog("Starting browser...");
            InitializeChromium();
            chromeBrowser.RegisterJsObject("cefCustomObject", new CefCustomObject(chromeBrowser, this));
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("");
            chromeBrowser.LoadHtml(Properties.Resources.index);
            chromeBrowser.MenuHandler = new CustomMenuHandler();
            // Add it to the form and fill it to the form window.
            pnlBrowser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;            
            chromeBrowser.LoadingStateChanged += ChromeBrowser_LoadingStateChanged;
        }

        private void ChromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Invoke(new ControlStringConsumer(SetText), new object[] { "Browser loaded." });
                Invoke(new ControlBoolConsumer(EnableControl), new object[] { btnHostServer, true });
            }
        }

        private delegate void ControlStringConsumer(string text);
        private delegate void ControlBoolConsumer(Control control, bool enable);

        private void SetText(string text)
        {
            WriteLog(text);
        }

        private void EnableControl(Control control, bool enable)
        {
            control.Enabled = enable;
        }

        public string GetExternalIPAddress()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            return externalip;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void StartServer()
        {
            try
            {
                CreateFirewallRule(); //opens port so that clients can connect to this server
                string machineName = Environment.MachineName;
                string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                WebApp.Start<Startup>(HostURL);
                if (domainName != "")
                {
                    WriteLog("Server hosted on: " + "http://" + machineName + "." + domainName + ":" + HostPort);
                }
                else
                {
                    WriteLog("Server hosted on: " + "http://" + machineName + ":" + HostPort);
                }

                btnHostServer.Text = "Stop Server";
                tbxWatchURL.Enabled = false;
                btnWatchServer.Enabled = false;

                game = new Game(300, 300, 0); //seed 0 for random
                chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync("DrawMap(" + Newtonsoft.Json.JsonConvert.SerializeObject(game.MapTiles) + ");");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + "\r\n" + ex.InnerException);
                btnHostServer.Text = "Host Server";
                tbxWatchURL.Enabled = true;
                btnWatchServer.Enabled = true;
            }
        }

        private void WriteLog(string message)
        {
            tbxLog.AppendText(message + "\r\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Remove("Island Server");
        }

        private void btnHostServer_Click(object sender, EventArgs e)
        {
            StartServer();
        }
    }
}
