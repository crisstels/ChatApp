using System;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;

namespace ChatApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Participant server = new Participant(3000, true);
            Participant client1 = new Participant(3001, false);

            client1.ConnectTo("192.168.15.160", 3000);
            Console.WriteLine("Nachricht eingeben: ");
            bool start = true;
            String msg= "";
            
            while (start)
            {
                System.Console.WriteLine("\nSie können nun ebenfalls Nachrichten schreiben. Beenden Sie mit <:q>.");
                System.Console.Write("Nachricht: ");
                msg = System.Console.ReadLine();
                if (msg == ":q")
                {
                    start = false;
                }
                else
                {
                    client1.SendMessage(msg);
                    System.Threading.Thread.Sleep(200);
                }
            }
            
            //client1.Disconnect();
            Console.WriteLine("Press any button to end");
            Console.ReadKey(true);
            server.CloseAllConnections();
            
        }
    }
}