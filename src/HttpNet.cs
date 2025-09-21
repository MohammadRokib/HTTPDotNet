using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            directory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.ToString();
            inputPath = Path.Combine(directory, "src", "message.txt");
            outputPath = Path.Combine(directory, "src", "output.txt");
            chunkSize = 8;
        }
        public void Read()
        {
            var encoding = Encoding.UTF8;

            using (var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var outputWriter = new StreamWriter(outputPath, false, encoding))
            {
                StringBuilder lineBuilder = new StringBuilder();
                byte[] buffer = new byte[chunkSize];
                int bytesRead;

                while ((bytesRead = inputStream.Read(buffer, 0, chunkSize)) > 0)
                {
                    string chunk = encoding.GetString(buffer, 0, bytesRead);
                    foreach (char c in chunk)
                    {
                        lineBuilder.Append(c);
                        if (c == '\n' || c == '\r')
                        {
                            outputWriter.Write($"read: {lineBuilder.ToString()}");
                            Console.Write($"read: {lineBuilder.ToString()}");

                            outputWriter.Flush();
                            lineBuilder.Clear();
                        }
                    }
                }
                
                if (lineBuilder.Length > 0)
                {
                    outputWriter.WriteLine($"read: {lineBuilder.ToString()}");
                    Console.WriteLine($"read: {lineBuilder.ToString()}");

                    outputWriter.Flush();
                    lineBuilder.Clear();
                }
            }
        }
    }
}
