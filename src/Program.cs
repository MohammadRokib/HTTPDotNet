using StreamParser;
using System.Net;
using System.Net.Sockets;

namespace src
{
    internal class Program
    {
        private static readonly CancellationTokenSource _cts = new();
        private static readonly int port = 9000;
        static async Task Main(string[] args)
        {
            NetSocket net = new NetSocket();
            await net.TcpRunner();
        }
    }
}
