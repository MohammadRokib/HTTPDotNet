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
            HttpNet net = new HttpNet();
            var listener = new TcpListener(IPAddress.Loopback, port);
            
            listener.Start();
            Console.WriteLine($"Listening on port: {port}");

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _cts.Cancel();
            };

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync(_cts.Token);
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await using var stream = client.GetStream();
                            await net.Read(stream);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            client.Dispose();
                            Console.WriteLine("Client disconnected");
                        }
                    });
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                listener.Stop();
                Console.WriteLine("Server shut down");
            }
        }
    }
}
