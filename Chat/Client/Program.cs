using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using SharedLibrary;

namespace Client
{
    internal static class Program
    {
        private static void StartClient(string ip, int port, string message)
        {
            try
            {
                var ipAddress = ip == "localhost" ? IPAddress.Loopback : IPAddress.Parse(ip);
                var remoteEp = new IPEndPoint(ipAddress, port);
                var sender = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEp);
                    Interactions.SendMsg(sender, message);
                    var data = Interactions.ReceiveMsg(sender);
                    var history = JsonSerializer.Deserialize<List<string>>(data);
                    if (history != null)
                        foreach (var msg in history)
                            Console.WriteLine(msg);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Main(string[] args)
        {
            Interactions.CheckArgumentsCount(args, 3);
            StartClient(args[0], int.Parse(args[1]), args[2]);
        }
    }
}