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
            inputPath = Path.Combine(directory, "src", "messages.txt");
            outputPath = Path.Combine(directory, "src", "output.txt");
            chunkSize = 8;
        }
        public void Read()
        {
            byte[] buffer = new byte[chunkSize];
            var encoding = Encoding.UTF8;
            var decoder = encoding.GetDecoder();

            using (var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var outputWriter = new StreamWriter(outputPath, false, encoding))
            {
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, chunkSize)) > 0)
                {
                    var maxCharCount = encoding.GetMaxCharCount(bytesRead);
                    var chars = new char[maxCharCount];
                    var charCount = decoder.GetChars(buffer, 0, bytesRead, chars, 0, false);

                    if (charCount > 0)
                    {
                        string text = new string(chars, 0, charCount);
                        outputWriter.WriteLine($"read: {text}");
                        Console.WriteLine($"read: {text}");
                    }
                    outputWriter.Flush();
                }

                var finalChars = new char[encoding.GetMaxCharCount(0)];
                var finalCharCount = decoder.GetChars(buffer, 0, 0, finalChars, 0, true);
                if (finalCharCount > 0)
                {
                    string finalText = new String(finalChars, 0, finalCharCount);
                    outputWriter.WriteLine($"read: {finalText}");
                    Console.WriteLine($"read: {finalText}");
                }
            }
        }
    }
}
