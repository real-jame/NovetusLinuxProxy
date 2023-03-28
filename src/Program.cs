

// See https://aka.ms/new-console-template for more information
namespace NovetusLinuxProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            WebProxy Proxy = new WebProxy();
            Proxy.Start();

            Console.CancelKeyPress += delegate
            {
                Proxy.Stop();
            };

            while (true) { }

        }
    }
}

