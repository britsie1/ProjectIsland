using NetFwTypeLib;
using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class Server : IDisposable
    {
        public string Name
        {
            get
            {
                return !string.IsNullOrEmpty(DomainName) ? $"http://{ MachineName }.{ DomainName }:{ Port }" : $"http://{ MachineName }:{ Port }";
            }
        }

        public int Port { get; private set; }
        public string Url { get; private set; }
        public string ExternalIP { get; private set; }
        public string MachineName { get; private set; }
        public string DomainName { get; private set; }

        private string FirewallRuleName { get; set; }
        private INetFwRule2 FirewallRule { get; set; }
        private INetFwPolicy2 FirewallPolicy { get; set; }

        public bool IsDisposed { get; private set; }

        public Server()
        {
            Port = 8080;
            Url = @"http://" + GetLocalIPAddress() + ":" + Port + "/";

            ExternalIP = new WebClient().DownloadString("http://icanhazip.com");
            MachineName = Environment.MachineName;
            DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

            InitFirewallOptions();
            AddFirewallRule();

            IsDisposed = false;
        }

        private void InitFirewallOptions()
        {
            FirewallRuleName = "GameServer - Island";

            FirewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            FirewallRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            FirewallRule.InterfaceTypes = "All";
            FirewallRule.Description = "Used to allow clients to connect to " + FirewallRuleName;
            FirewallRule.Name = FirewallRuleName;

            FirewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            FirewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            FirewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            FirewallRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            FirewallRule.LocalPorts = Port.ToString();
            FirewallRule.Enabled = true;
        }
        private void AddFirewallRule()
        {
            FirewallPolicy.Rules.Add(FirewallRule);
        }
        private void RemoveFirewallRule()
        {
            FirewallPolicy.Rules.Remove(FirewallRuleName);
        }

        private string GetLocalIPAddress()
        {
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var IP in Host.AddressList)
            {
                if (IP.AddressFamily == AddressFamily.InterNetwork)
                    return IP.ToString();
            }

            throw new Exception("Local IP Address Not Found!");
        }

        public void Dispose()
        {
            Port = 0;
            Url = null;

            RemoveFirewallRule();
            FirewallRuleName = null;
            FirewallRule = null;
            FirewallPolicy = null;

            IsDisposed = true;
        }
    }
}
