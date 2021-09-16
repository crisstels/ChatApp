using System;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;
// TODO: überprüfe, ob application type in Ordnung ist
// Damit das Programm lauffähig ist, wird  .NET5 benötigt
namespace ChatApp
{
    // beendet zuerst die Verbindung vom Client zum Server und beendet dann die listener Threads
    delegate void DelegateCloseConnection();
    class Program
    {
        static void Main(string[] args)
        {
            // Instanziiere Objekte
            Console.WriteLine("Please enter your Ip-Adress (Ipv4): ");
            string ipAdress = Console.ReadLine();
            Participant server = new Participant(3000, ipAdress);
            Participant client1 = new Participant(3001, ipAdress);
            Message message = new Message("Chat App", "me");
            bool start = true;
            String msg= "";
            DelegateCloseConnection closeConnection = new DelegateCloseConnection(client1.Disconnect);
            closeConnection += server.StopThreads;
            
            // Baue Verbindung auf
            client1.ConnectTo(ipAdress, 3000);
            Console.WriteLine("Sie können nun Nachrichten eingeben.Beenden Sie mit <:q>.");
            
            // Nachrichten schreiben, solange bis :q eingegeben wird
            while (start)
            {
                Console.Write("Nachricht: ");
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
            
            // Schließt alle Verbindungen, wirft eine Exception, da der Thread Interrupted wird
            closeConnection();
            Console.WriteLine("Press any button to end");
            Console.ReadKey(true);
            server.StopThreads();
            
        }
    }
}