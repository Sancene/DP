using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chain
{
    class Program
    {
        private static Socket sender;
        private static Socket listener;
        private static int x;

        static void Main(string[] args)
        {
            int listenPort = Convert.ToInt32(args[0]);

            string writerAddress = args[1];
            var writerPort = Convert.ToInt32(args[2]);
            bool isInitiator = args.Length == 4 && args[3] == "true";

            x = Convert.ToInt32(Console.ReadLine());

            InitSockets(listenPort, writerAddress, writerPort);

            if (isInitiator)
            {
                WorkAsInitiator();
            }
            else
            {
                WorkAsNormalProcess();
            }

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            Console.ReadKey();
        }

        private static int ReadIntFromSocket(Socket handler)
        {
            byte[] buf = new byte[1024];
            string data = null;
            do
            {
                int bytes = handler.Receive(buf);
                data += Encoding.UTF8.GetString(buf, 0, bytes);
            }
            while (handler.Available > 0);
            return Convert.ToInt32(data);
        }

        private static void SendIntToSocket(Socket handler, int number)
        {
            sender.Send(Encoding.UTF8.GetBytes("" + number));
        }

        private static void WorkAsInitiator()
        {
            SendIntToSocket(sender, x);

            Socket handler = listener.Accept();
            int y = ReadIntFromSocket(handler);
            Console.WriteLine(y);

            SendIntToSocket(sender, Math.Max(x, y));

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private static void WorkAsNormalProcess()
        {
            Socket handler = listener.Accept();
            int y = ReadIntFromSocket(handler);

            SendIntToSocket(sender, Math.Max(x, y));

            y = ReadIntFromSocket(handler);
            Console.WriteLine(y);

            SendIntToSocket(sender, y);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private static void InitSockets(int listenPort, string writerAddress, int writerPort)
        {

            IPAddress listenIpAddress = IPAddress.Any;
            IPEndPoint localEP = new IPEndPoint(listenIpAddress, listenPort);
            listener = new Socket(
                 listenIpAddress.AddressFamily,
                 SocketType.Stream,
                 ProtocolType.Tcp);
            listener.Bind(localEP);
            listener.Listen(10);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(writerAddress);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, writerPort);
            sender = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            ConnectWriter(remoteEP);
        }

        private static void ConnectWriter(IPEndPoint remoteEP)
        {
            while (true)
            {
                try
                {
                    sender.Connect(remoteEP);
                    return;
                }
                catch (SocketException ex)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}