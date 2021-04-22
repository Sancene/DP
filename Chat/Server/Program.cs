using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using SharedLibrary;

namespace Server
{
    internal static class Program
    {
        private static void StartListening(int port)
        {
            var history = new List<string>();
            var ipAddress = IPAddress.Any;
            var localEndPoint = new IPEndPoint(ipAddress, port);
            var listener = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    var handler = listener.Accept();
                    var data = Interactions.ReceiveMsg(handler);
                    history.Add(data);
                    Console.WriteLine($"Message received: {data}");
                    var jsonMsg = JsonSerializer.Serialize(history);
                    Interactions.SendMsg(handler, jsonMsg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Запуск сервера...");

            Interactions.CheckArgumentsCount(args, 1);
            StartListening(int.Parse(args[0]));

            Console.WriteLine("\nНажмите ENTER чтобы выйти...");
            Console.Read();
        }
    }
}