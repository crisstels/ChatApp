using System;
using System.Net;
using System.Web;
using System.Net.Sockets;

namespace ChatApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*//Participant client = new Participant();
            Console.WriteLine(Dns.GetHostName());
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            Console.WriteLine(ipAddr);*/
            Participant server = new Participant("192.168.15.160", 3000);
            TcpListener listener = server.InitTcpServer();
            server.TcpListener(listener);
            
            Console.ReadKey(true);
            
            
        }
    }
}