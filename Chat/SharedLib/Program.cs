using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SharedLibrary
{
    public static class Interactions
    {
        public static string ReceiveMsg(Socket socket)
        {
            var lenBuf = new byte[sizeof(int)];
            socket.Receive(lenBuf);
            var buf = new byte[BitConverter.ToInt32(lenBuf)];
            var data = Encoding.UTF8.GetString(buf, 0, socket.Receive(buf));

            return data;
        }

        public static void SendMsg(Socket socket, string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            socket.Send(BitConverter.GetBytes(data.Length).Concat(data).ToArray());
        }

        public static void CheckArgumentsCount(string[] args, short expectedArgumentsCount)
        {
            if (args.Length != expectedArgumentsCount) throw new Exception("Invalid argument count");
        }
    }
}