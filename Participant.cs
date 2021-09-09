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

namespace ChatApp
{
    public class Participant
    {
        private string _ipAdresse;
        private int _port;
        private Message _text;
        
        private String _lastMessage;
        private IPAddress _localAdress;

        protected TcpListener   listener;       
        protected Socket        connectionOut;
        protected List<Socket>  connectionIn;
        protected Thread        dispatchThread;
        protected List<Thread>  receiverThread;

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

        public Message Text
        {
            get => _text;
            set => _text = value;
        }


        public string IpAdresse
        {
            get => _ipAdresse;
            set => _ipAdresse = value;
        }

        public int Port
        {
            get => _port;
            set => _port = value;
        }

        public Participant(string ipAdresse, int port)
        {
            _ipAdresse = ipAdresse;
            _port = port;
        }

        public Participant(string ipAdresse, int port, Message text)
        {
            _ipAdresse = ipAdresse;
            _port = port;
            _text = text;
        }

        public Participant()
        {
            IpAdresse = "";
            Port = 0;
            Text = new Message();
        }

        public Participant(int port)
        {
            connectionIn = new List<Socket>();
            receiverThread = new List<Thread>();
            LocalAdress = IPAddress.Parse("192.168.15.160");
            
            listener = new TcpListener(LocalAdress, 3000);
            listener.Start();
            dispatchThread = new Thread(Dispatch);
            dispatchThread.Start();
        }

        public void Dispatch()
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

        public void Recieve(Socket sock)
        {
            Byte[] buffer = new Byte[256];
            while (true)
            {
                int lenOfMsg = sock.Receive(buffer);

                IPEndPoint remoteIpEndPoint = sock.RemoteEndPoint as IPEndPoint;
                String     remoteAddr = remoteIpEndPoint.Address.ToString();

                LastMessage = Encoding.UTF8.GetString(buffer, 0, lenOfMsg);
                System.Console.WriteLine("FROM   : " + remoteAddr + ":" + remoteIpEndPoint.Port +
                                         "\nTO     : " + LocalAdress +
                                         "\nMessage: " + LastMessage);
            }
        }

        public TcpListener InitTcpServer()
        {
            TcpListener server=null;
            try
            {
                // Set the TcpListener
                Int32 port = Port;
                IPAddress localAddr = IPAddress.Parse(IpAdresse);
                
                server = new TcpListener(localAddr, port);
                Console.WriteLine(localAddr);

                // Start listening for client requests.
                server.Start();

            }
            catch (Exception socketError)
            {
             Console.WriteLine(socketError);   
            }

            return server;
        }

        

        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message);

            connectionOut.Send(sendBuffer);
        }

        public void Disconnect()
        {
            connectionOut.Close();
            connectionOut = null;
        }
        
        public void CloseAllConnections()
        {
            for (int i = 0; i < connectionIn.Count; i++){
                connectionIn[i].Close();
                receiverThread[i].Interrupt();
                listener.Stop();
                dispatchThread.Interrupt(); //only for .Net5
            }
     
        }
        
        public void ConnectTo(String strIP4Addr, int portNr)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(strIP4Addr), portNr);

            connectionOut = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionOut.Connect(endPoint);
        }
    }
}