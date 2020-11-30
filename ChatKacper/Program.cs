using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatKacper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server starting");
            var server = new Server("127.0.0.1", 14000);
            server.ClientConnected += Server_ClientConnected;

            Task task = Task.Run(() => server.Start());

            Console.ReadKey();            
            server.Stop(); // zadamie zatrzymania serwera - w tle serwer sie zatrzymuje
            Console.WriteLine("Stopping...");

            task.Wait(); // czekaj az to zatrzymanei rzeczewiscie sie zrealizuje
           
        }

        private static void Server_ClientConnected(AddressFamily addressFamily)
        {
           // addressFamily
            Console.WriteLine("Client connected");
        }
    }
}
