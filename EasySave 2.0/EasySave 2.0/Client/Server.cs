using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Projet.Client
{
    public class Server
    {
        /// <summary>
        /// Create a stocket to make a server.
        /// </summary>
        /// <returns></returns>
        public static Socket SeConnecter()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 55979);
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Bind(localEndPoint);
                server.Listen(10);
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

        /// <summary>
        /// Accept the connection from the client.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static Socket AccepterConnection(Socket server)
        {
            Socket client = server.Accept();
            return client;
        }

        /// <summary>
        /// Listen the client and check if there is a new message to receive. If there is one, we return the progress.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sender"></param>
        public static void EcouterReseau(Socket client, object sender)
        {
            string data;
            byte[] bytes = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRec = client.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if ((sender as BackgroundWorker).WorkerReportsProgress == true)
                    {
                        (sender as BackgroundWorker).ReportProgress(0, data);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }
        }

        /// <summary>
        /// Send a message to the client. Only once every 0,5second.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="server"></param>
        public static void SendMsg(string message, Socket server)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            Thread.Sleep(500);
            try
            {
                server.Send(msg);
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

        /// <summary>
        /// Stop the connection between the server and the client.
        /// </summary>
        /// <param name="server"></param>
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
