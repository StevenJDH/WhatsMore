using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMore
{
    class Connection
    {
        [Flags]
        enum ConnectionState
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private extern static bool InternetGetConnectedState(ref ConnectionState lpdwFlags, int dwReserved);

        public static bool IsInternetAvailable()
        {
            ConnectionState connectionState = 0;
            return InternetGetConnectedState(ref connectionState, 0);
        }
    }
}
