using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src
{
    public class HttpNet
    {
        private string FilePath = String.Empty;

        public HttpNet()
        {
            FilePath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.ToString();
            FilePath = Path.Combine(FilePath, "src", "messages.txt");
        }
        public void Read()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }

            using (StreamReader sr = new StreamReader(FilePath))
            {
                int charCode;
                while ((charCode = sr.Read()) != -1)
                {
                    Console.WriteLine((char)charCode);
                }

            }
        }
    }
}
