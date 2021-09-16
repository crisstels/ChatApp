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
        private Message _text;
        
        private String _lastMessage;
        private IPAddress _localAdress;
        private NetworkStream _stream;
        private bool _isServer;

        protected TcpListener   listener;       
        protected TcpClient     client;
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
        public NetworkStream Stream
        {
            get => _stream;
            set => _stream = value;
        }
        
        public bool IsServer
        {
            get => _isServer;
            set => _isServer = value;
        }
        
        public Participant()
        {
            LastMessage = " ";
            Text = new Message();
        }

        public Participant(int port)
        {
            try
            {
                connectionIn = new List<Socket>();
                receiverThread = new List<Thread>();
                LocalAdress = IPAddress.Parse("192.168.15.160");
                listener = new TcpListener(LocalAdress, port);
                listener.Start();
                dispatchThread = new Thread(Dispatch);
                dispatchThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("constructor catch" +e);
            }
            
        }

        public void Dispatch()
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
                Console.WriteLine(e);
                throw;
            }
        }

        public void Recieve(Socket sock)
        {
            Byte[] buffer = new Byte[256];
            bool x = true;
            try
            {
                while (x)
                {
                    int lenOfMsg = sock.Receive(buffer);

                    IPEndPoint remoteIpEndPoint = sock.RemoteEndPoint as IPEndPoint;
                    String     remoteAddr = remoteIpEndPoint.Address.ToString();

                    LastMessage = Encoding.UTF8.GetString(buffer, 0, lenOfMsg);
                    System.Console.WriteLine("FROM   : " + remoteAddr + ":" + remoteIpEndPoint.Port +
                                             "\nTO     : " + LocalAdress +
                                             "\nMessage: " + LastMessage);

                    if (LastMessage == ":q")
                    {
                        x = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message);

            Stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public void Disconnect()
        {
            foreach (var t in connectionIn)
            {
                t.Close();
            }
            client.GetStream().Close();
            client.Dispose();
            Stream.Dispose();

        }        
        public void CloseAllConnections()
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
        
        public void ConnectTo(String strIP4Addr, int portNr)
        {
            client = new TcpClient(strIP4Addr.ToString(), portNr);
            //client.Connect(endPoint);
            Stream = client.GetStream();
        }
    }
}