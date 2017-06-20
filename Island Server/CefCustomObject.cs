using CefSharp.WinForms;

namespace GameServer
{
    class CefCustomObject
    {
        // Declare a local instance of chromium and the main form in order to execute things from here in the main thread
        private static ChromiumWebBrowser _instanceBrowser = null;
        // The form class needs to be changed according to yours
        private static GameServerForm _instanceMainForm = null;

        public CefCustomObject(ChromiumWebBrowser originalBrowser, GameServerForm mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }
    }
}
