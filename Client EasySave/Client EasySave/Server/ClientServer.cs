using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Projet.Server
{
    public class ClientServer
    {
        public static Socket SeConnecter()
        {
            
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 55979);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(localEndPoint);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return server;
        }

        public static void EcouterReseau(Socket server, object sender)
        {
            string data = "";
            byte[] bytes = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRec = server.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("]") > -1)
                    {
                        if ((sender as BackgroundWorker).WorkerReportsProgress == true)
                        {
                            (sender as BackgroundWorker).ReportProgress(0, data);
                            EcouterReseau(server, sender);
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void SendMsg(string message, Socket server)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            server.Send(msg);
        }

        public static void Deconnecter(Socket server)
        {
            try
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
