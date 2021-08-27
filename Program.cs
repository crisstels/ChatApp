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
            /*
             * Um dieses Programm auszuprobieren muss man 2 Projekte anlegen. In Projekt 1 wird der Server gestartet und
             * in Projekt 2 wird der Client gestartet. Dann wartet der Server darauf, dass sich jemand mit ihm verbinden möchte.
             * Der Client kann über die Konsole Nachrichten (strings) eingeben und an den Server senden. Dies wurde so einfach gehalten,
             * da der nächste Prototyp Multithreading beeinhalten wird und dadurch braucht man nur noch ein Projekt zu starten.
             * So wird dann ein Server und ein CLient gleichzeitig gestartet.
             */
            
            // Starte den Server
            Participant server = new Participant();
            TcpListener listener = server.InitTcpServer();
            server.TcpListener(listener);
            
            // Oder starte den Client, wobei die IpAdressse und der Port des zu verbindenden Servers übergeben werden
            Participant client = new Participant("192.168.15.160", 300);
            
            client.SendMessage("192.168.15.160", 3000);
            
            Console.ReadKey(true);
            
            
        }
    }
}