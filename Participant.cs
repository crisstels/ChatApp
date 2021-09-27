using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatApp
{
    public class Participant
    {
        #region Properties

        private Message _textMessage;
        private String _lastMessage;
        private IPAddress _localAdress;
        private IPAddress _remoteAdress;


        private NetworkStream _stream;

        protected TcpListener   listener;       
        protected TcpClient     sender;
        protected List<Socket>  connectionIn;
        protected Thread        dispatchThread;
        protected List<Thread>  receiverThread;
        protected bool stopListener;

        #endregion

        #region Accessors/Modifiers

        public string LastMessage
        {
            get => _lastMessage;
            set => _lastMessage = value;
        }
        
        public IPAddress LocalAdress
        {
            get => _localAdress;
            set => _localAdress = value;
        }
        
        public IPAddress RemoteAdress
        {
            get => _remoteAdress;
            set => _remoteAdress = value;
        }

        public Message TextMessage
        {
            get => _textMessage;
            set => _textMessage = value;
        }
        public NetworkStream Stream
        {
            get => _stream;
            set => _stream = value;
        }

        #endregion

        #region Contructor

        public Participant()
        {
            LastMessage = " ";
            TextMessage = new Message("Chat App", "me");
        }

        public Participant(string ipAdress, int port)
        {
            connectionIn = new List<Socket>();
            receiverThread = new List<Thread>();
            LocalAdress = IPAddress.Parse(ipAdress);
            listener = new TcpListener(LocalAdress, port);
            sender = new TcpClient(LocalAdress.ToString(), 3001);
            
            try
            {
                listener.Start();
                Console.WriteLine("Listening on {0}, Writing to 3001", port );
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread is aborted and the error message is "
                                  + e);
            }

            try
            {
                dispatchThread = new Thread(Dispatch);
                dispatchThread.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine("Thread is aborted and the error message is "
                                  + e);
            }

            try
            {
                Stream = sender.GetStream();
                Console.Write("connected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        #endregion
        
        // startet Threads für das warten und empfangen von Nachrichten
        private void Dispatch()
        {
            try
            {
                while (true)
                {
                    Socket sock = listener.AcceptSocket(); 
                    connectionIn.Add(sock);                 

                    Thread recv = new Thread(() => Recieve(sock));
                    recv.Start();                          
                    receiverThread.Add(recv);               
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread is aborted and the error message is "
                                  + e);
                throw;
            }
        }
        
        // empfängt Nachrichten
        private void Recieve(Socket sock)
        {
            Byte[] buffer = new Byte[256];
            bool x = true;
            try
            {
                while (x)
                {
                    int lenOfMsg = sock.Receive(buffer);

                    LastMessage = Encoding.UTF8.GetString(buffer, 0, lenOfMsg);
                    var json = (JObject)JsonConvert.DeserializeObject(LastMessage);

                    string app = json["Application"].Value<string>();
                    string message = json["Nachricht"].Value<string>();
                    string nick = json["Nickname"].Value<string>();
                    
                    Console.WriteLine(nick + ":" + message + "\n");

                    if (LastMessage == ":q" || app != "Chat App")
                    {
                        x = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread is aborted and the error message is "
                                  + e);
                throw;
            }
        }
        
        //sendet Nachrichten
        public void SendMessage(Message message, string text)
        {
            message.Nachricht = text;
            byte[] sendBuffer = message.ConvertMessageToByte();
            Stream.Write(sendBuffer, 0, sendBuffer.Length);
        }
        
        // die Verbindung zu den Clients wird gestoppt, der Stream wird beendet
        public void Disconnect()
        {
            foreach (var t in connectionIn)
            {
                t.Close();
            }
            sender.GetStream().Close();
            sender.Dispose();
            Stream.Dispose();

        }
        
        // Threads werden interrupted, daher wird eine Fehlermeldung gegeben
        public void StopThreads()
        {
            try
            {
                foreach (var t in receiverThread)
                {
                    t.Interrupt();
                    listener.Stop();
                    dispatchThread.Interrupt(); //only for .Net5
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine("Thread is aborted and the code is "
                                  + ex.ExceptionState);
            }
        }
        
        // baut eine Verbindung von einem TCPClient zu einem server auf
        public void ConnectTo(int portNr)
        {
            sender = new TcpClient(LocalAdress.ToString(), portNr);
            Stream = sender.GetStream();
            Console.WriteLine("Connected");
        }
    }
}