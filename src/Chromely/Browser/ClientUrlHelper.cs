using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace Chromely.Browser {

    /// <summary> Helper class for finding an available TCP Port. </summary>
    public class ClientUrlHelper {

        /// <summary> The start range. </summary>
        private static int PortStartRange { get; set; } = 5001;

        /// <summary> The end range. </summary>
        private static int PortEndRange { get; set; } = 6000;

        /// <summary> Gets an available tcp port that is not in use. </summary>
        /// <returns> The available port. </returns>
        public static int GetAvailablePort() {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var endpoints = ipGlobalProperties.GetActiveTcpListeners();
            for (int i = PortStartRange; i < PortEndRange; i++) {
                if (IsPortAvailable(i, endpoints)) {
                    return i;
                }
            }
            // Free port not found
            return -1;
        }

        /// <summary> Queries if a port is available. </summary>
        /// <param name="port"> The port number. </param>
        /// <returns> True if the port is available, false if not. </returns>
        public static bool IsPortAvailable(int port) {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var endpoints = ipGlobalProperties.GetActiveTcpListeners();
            return IsPortAvailable(port, endpoints);
        }

        /// <summary> Queries if a port is available. </summary>
        /// <param name="port">      The port number. </param>
        /// <param name="endpoints"> The list of tcp endpoints. </param>
        /// <returns> True if the port is available, false if not. </returns>
        public static bool IsPortAvailable(int port, IPEndPoint[] endpoints) {
            foreach (IPEndPoint endpoint in endpoints) {
                if (endpoint.Port == port) {
                    return false;
                }
            }
            return true;
        }

        /// <summary> Get a local https / http url based on a free port. </summary>
        /// <returns> The local HTTPS / HTTP url. </returns>
        public static List<string> GetLocalHttpUrls(int port, bool https = true) {
            if (port == -1) {
                throw new IOException("No port found available within the range");
            }
            var appurls = new List<string>();
            if (https) {
                appurls.Add($"https://127.0.0.1:{port}");
            }
            else {
                appurls.Add($"http://127.0.0.1:{port}");
            }
            return appurls;
        }
    }
}
