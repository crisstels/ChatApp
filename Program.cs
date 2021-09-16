using System;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;
// TODO: überprüfe, ob application type in Ordnung ist
namespace ChatApp
{
    delegate void DelegateCloseConnection();
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            Participant server = new Participant(3000);
            Participant client1 = new Participant(3001);
            Message message = new Message("app", "me");
            
            client1.ConnectTo("192.168.15.160", 3000);
            Console.WriteLine("Nachricht eingeben: ");
            bool start = true;
            String msg= "";
            DelegateCloseConnection closeConnection = new DelegateCloseConnection(client1.Disconnect);
            closeConnection += server.CloseAllConnections;
            
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
                    client1.SendMessage(message, msg);
                    System.Threading.Thread.Sleep(200);
                }
            }
            
            //client1.Disconnect();
            Console.Write("vor delegate");
            closeConnection();
            Console.WriteLine("Press any button to end");
            Console.ReadKey(true);
            server.CloseAllConnections();
            
        }
    }
}