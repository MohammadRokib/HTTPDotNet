namespace src
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpNet net = new HttpNet();
            try
            {
                await net.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
