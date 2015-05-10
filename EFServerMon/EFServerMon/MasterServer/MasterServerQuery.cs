using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EFServerMon.MasterServer
{
    public class MasterServerQuery : IMasterServerQuery
    {
        private const string msHost = "master.stef1.ravensoft.com";
        private const int msPort = 27953;
        private readonly IPAddress msAddress;

        /// <summary>
        /// EOT End of Text
        /// \0  Null Character
        /// </summary>
        private readonly string[] responseHeaders = new string[] {
            "????getserversResponse",
            "EOT\0\0"
        };

        public MasterServerQuery()
        {
            var resolvedAddresses = Dns.GetHostEntry(msHost);
            if(resolvedAddresses.AddressList.Length > 0)
            {
                this.msAddress = resolvedAddresses.AddressList[0];
            }
        }

        public void GetServerList()
        {
            if (this.msAddress == null)
            {
                return;
            }
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = 3000
            };
            client.Connect(this.msAddress, msPort);

            var bytes = Encoding.ASCII.GetBytes("xxxxgetservers 24 full");
            bytes[0] = byte.Parse("255");
            bytes[1] = byte.Parse("255");
            bytes[2] = byte.Parse("255");
            bytes[3] = byte.Parse("255");

            client.Send(bytes, SocketFlags.None);

            var array = new byte[65001];
            client.Receive(array);
                
            this.ParseServers(array);
        }

        private List<object> ParseServers(byte[] streamServers)
        {
            var address = IPAddress.Parse("74.91.116.133");

            var decoded = Encoding.ASCII.GetString(streamServers);
            var trimmed = this.TrimHeaders(decoded);            
            var split = trimmed.Split('\\');

            var servers = new List<object>();

            return servers;
        }


        private string TrimHeaders(string input)
        {
            foreach (var header in responseHeaders)
            {
                if (input.Contains(header))
                {
                    input = input.Replace(header, "").Trim()
                        .TrimStart('\\')
                        .TrimEnd('\\');
                }
            }

            return input;
        }
    }
}