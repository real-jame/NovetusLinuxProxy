using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace NovetusLinuxProxy
{
    public class WebProxyModule
    {
        // This segment is originally from the broader extension class, but we don't need those for this program, so we'll roll it into one class.
        public virtual string Name() { return "Unnamed Module"; }
        public virtual string Version() { return "1.0.0"; }
        public virtual string Author() { return "Unknown Author"; }
        public virtual string FullInfoString() { return (Name() + " v" + Version() + " by " + Author()); }
        public virtual void OnExtensionLoad() { }
        public virtual void OnExtensionUnload() { }

        public virtual void OnProxyStart() { }
        public virtual void OnProxyStopped() { }
        public virtual bool IsValidUrl(string absolutePath, string host) { return false; }
        public virtual Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e) { return Task.CompletedTask; }
        public virtual async Task OnRequest(object sender, SessionEventArgs e)
        {
            e.Ok("Test successful. \nRunning Novetus Web Proxy for Linux");
        }
    }

    public class ExtensionManager
    {
        // TODO
    }

    public class WebProxy
    {
        private ProxyServer Server = new ProxyServer();
        // private static readonly SemaphoreLocker _locker = new SemaphoreLocker();
        public bool Started { get { return Server.ProxyRunning; } }
        private int WebProxyPort = 61710;

        public void Start()
        {
            System.Console.WriteLine("Booting up web proxy on port " + WebProxyPort + "...");
            // Server.CertificateManager.RootCertificateIssuerName = "realjame";
            // Server.CertificateManager.RootCertificateName = "Novetus Web Proxy for Linux";
            Server.BeforeRequest += new AsyncEventHandler<SessionEventArgs>(OnRequest);

            ExplicitProxyEndPoint explicitEndpoint = new ExplicitProxyEndPoint(IPAddress.Any, WebProxyPort, true);
            explicitEndpoint.BeforeTunnelConnectRequest += new AsyncEventHandler<TunnelConnectSessionEventArgs>(OnBeforeTunnelConnectRequest);
            Server.AddEndPoint(explicitEndpoint);

            Server.Start();

            // foreach (ProxyEndPoint endpoint in Server.ProxyEndPoints)
            // {
            //     Server.SetAsSystemProxy(explicitEndpoint, ProxyProtocolType.AllHttp);
            // }

            System.Console.WriteLine("Web proxy started on port " + WebProxyPort);

            // TODO: notify modules of proxy starting
        }

        private bool IsValidURL(HttpWebClient client)
        {
            string uri = client.Request.RequestUri.Host;

            if ((!uri.StartsWith("www.") &&
                !uri.StartsWith("web.") &&
                !uri.StartsWith("assetgame.") &&
                !uri.StartsWith("wiki.") &&
                !uri.EndsWith("api.roblox.com") &&
                !uri.StartsWith("roblox.com") || !uri.EndsWith("roblox.com")) &&
                !uri.EndsWith("robloxlabs.com"))
            {
                return false;
            }

            // check the header
            WebHeaderCollection headers = client.Request.Headers;
            List<HttpHeader> userAgents = headers.GetHeaders("User-Agent");

            if (userAgents == null)
                return false;

            if (string.IsNullOrWhiteSpace(userAgents.FirstOrDefault().Value))
                return false;

            string userAgent = userAgents.FirstOrDefault().Value.ToLowerInvariant();

            //"for some reason, this doesn't go through for the browser unless we look for mozilla/4.0."
            //"this shouldn't break modern mozilla browsers though."
            return (userAgent.Contains("mozilla/4.0") || userAgent.Contains("Roblox"));
        }

        private async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
            // TODO
            System.Console.WriteLine("Tunnel: " + e.HttpClient.Request.RequestUri.ToString());
        }

        private async Task OnRequest(object sender, SessionEventArgs e)
        {
            System.Console.WriteLine("Request: " + e.HttpClient.Request.RequestUri.ToString());

            await _locker.LockAsync(async () =>
            {
                if (!IsValidURL(e.HttpClient))
                    return;

                Uri uri = e.HttpClient.Request.RequestUri;

                foreach ()
            })
        }

        public void Stop()
        {
            try
            {
                System.Console.WriteLine("Web proxy stopping...");
                Server.BeforeRequest -= new AsyncEventHandler<SessionEventArgs>(OnRequest);
                Server.Stop();

                // TODO: notify modules of proxy stopping
            }
            catch
            {

            }
        }
    }
}