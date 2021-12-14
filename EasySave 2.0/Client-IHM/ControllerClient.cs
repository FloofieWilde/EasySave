using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_IHM
{
    public class ControllerClient
    {
        public static Socket SeConnecter()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 55979);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            server.Connect(localEndPoint);

            return server;
        }

        public static void EcouterReseau(Socket server)
        {
            string data;
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRec = server.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            //Deconnecter(server);
            //return data;
        }

        public static void SendMsg(string message, Socket client)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            client.Send(msg);
        }

        public static void Deconnecter(Socket server)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
