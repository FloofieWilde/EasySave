using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Projet.Client
{
    class Server
    {
        static void Main(string[] args)
        {

            Socket server = SeConnecter();
            Socket client = AccepterConnection(server);
            EcouterReseau(client);
            Deconnecter(server);
        }

        private static Socket SeConnecter()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 55979);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen(10);
            return server;
        }

        private static Socket AccepterConnection(Socket server)
        {

            Socket client = server.Accept();
            //Console.WriteLine("Connecté avec l'adresse: " + IPAddress.Parse(((IPEndPoint)client.RemoteEndPoint).Address.ToString()) + " et port: " + (((IPEndPoint)client.RemoteEndPoint).Port.ToString()));
            return client;
        }

        private static void EcouterReseau(Socket client)
        {
            string data = null;
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRec = client.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
                Console.WriteLine("Client: " + data);
                data = "Bonjour client";
                byte[] msg = Encoding.ASCII.GetBytes(data);
                client.Send(msg);
            }
        }

        private static void Deconnecter(Socket server)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
