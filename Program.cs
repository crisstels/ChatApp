using System;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.Text.Json;
using Newtonsoft.Json;
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
            Console.WriteLine("Please enter the Ip-Adress (Ipv4) of server: ");
            string ipAdress = Console.ReadLine();
            //Participant server = new Participant(3000, ipAdress);
            Participant client1 = new Participant(ipAdress, 3001);
            Message message = new Message("Chat App", "you");
            bool start = true;
            String msg= "";
            DelegateCloseConnection closeConnection = new DelegateCloseConnection(client1.Disconnect);
            closeConnection += client1.StopThreads;
            
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
            client1.StopThreads();
            
        }
    }
}