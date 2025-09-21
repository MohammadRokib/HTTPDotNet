namespace src
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpNet net = new HttpNet();
            try
            {
                net.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
