﻿using System;
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

        public static Socket AccepterConnection(Socket server)
        {
            Socket client = server.Accept();
            return client;
        }

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