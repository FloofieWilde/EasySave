using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Projet.Client
{
    public class Server
    {
        public static Socket SeConnecter()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 55979);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen(10);
            return server;
        }

        public static Socket AccepterConnection(Socket server)
        {
            Socket client = server.Accept();
            //Console.WriteLine("Connecté avec l'adresse: " + IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString()) + " et port: " + (((IPEndPoint)client.RemoteEndPoint).Port.ToString()));
            return client;
        }

        public static void EcouterReseau(Socket client, object sender)
        {
            string data;
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRec = client.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if ((sender as BackgroundWorker).WorkerReportsProgress == true)
                {
                    (sender as BackgroundWorker).ReportProgress(0, data);
                }
            }
            //Deconnecter(server);
            //return data;
        }

        public static void SendMsg(string message, Socket server)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            server.Send(msg);
        }

        private static void Deconnecter(Socket server)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
