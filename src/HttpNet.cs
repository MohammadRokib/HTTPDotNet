using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace src
{
    public class HttpNet
    {
        private string directory = String.Empty;
        private string inputPath = String.Empty;
        private string outputPath = String.Empty;
        private readonly int chunkSize;

        public HttpNet()
        {
            directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.ToString();
            inputPath = Path.Combine(directory, "src", "messages.txt");
            outputPath = Path.Combine(directory, "src", "output.txt");
            chunkSize = 8;
        }
        public async Task Read()
        {
            var encoding = Encoding.UTF8;
            var channel = Channel.CreateUnbounded<string>();

            using (var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var outputWriter = new StreamWriter(outputPath, false, encoding))
            {
                _ = Task.Run(() =>
                {
                    GetLineChannel(inputStream, channel.Writer);
                    channel.Writer.Complete();
                });

                while (await channel.Reader.WaitToReadAsync())
                {
                    string line = await channel.Reader.ReadAsync();
                    outputWriter.Write(line);
                    Console.Write(line);
                }
                await outputWriter.FlushAsync();
            }
        }

        public async Task Read(Stream inputStream)
        {
            var encoding = Encoding.UTF8;
            var channel = Channel.CreateUnbounded<string>();

            using (var outputWriter = new StreamWriter(outputPath, false, encoding))
            {
                _ = Task.Run(() =>
                {
                    GetLineChannel(inputStream, channel.Writer);
                    channel.Writer.Complete();
                });

                while (await channel.Reader.WaitToReadAsync())
                {
                    string line = await channel.Reader.ReadAsync();
                    await outputWriter.WriteAsync(line);
                    Console.Write(line);
                }
                await outputWriter.FlushAsync();
            }
        }

        public void GetLineChannel(Stream inputStream, ChannelWriter<string> writer)
        {
            var encoding = Encoding.UTF8;

            StringBuilder lineBuilder = new StringBuilder();
            byte[] buffer = new byte[chunkSize];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, chunkSize)) > 0)
            {
                string chunk = encoding.GetString(buffer, 0, bytesRead);
                foreach (char ch in chunk)
                {
                    lineBuilder.Append(ch);
                    if (ch == '\n')
                    {
                        writer.TryWrite($"read: {lineBuilder.ToString()}");
                        lineBuilder.Clear();
                    }
                }
            }

            if (lineBuilder.Length > 0)
            {
                writer.TryWrite($"read: {lineBuilder.ToString()}\n");
                lineBuilder.Clear();
            }
        }
    }
}
