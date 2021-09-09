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
            Participant server = new Participant(3000);
            Participant client1 = new Participant(3001);

            client1.ConnectTo("192.168.15.160", 3000);
            Console.WriteLine("Nachricht eingeben: ");

            String msg= "";
            while (msg != "c"){
                System.Console.WriteLine("\nSie können nun ebenfalls Nachrichten schreiben. Senden durch <CR>, Beenden mit <c>.");
                System.Console.Write("Nachricht: ");
                msg = System.Console.ReadLine();
                if (msg != "c")
                    client1.SendMessage(msg); 
                    System.Threading.Thread.Sleep(200);
            }



        }
    }
}