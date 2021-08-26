using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChatApp
{
    public class Participant
    {
        private string _ipAdresse;
        private int _port;
        private Message _text;

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

        public void InitTcpServer()
        {
            TcpListener server=null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

            }
            catch (Exception socketError)
            {
             Console.WriteLine(socketError);   
            }
        }

        public void TcpListener(TcpListener server)
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;
            try{
                // Enter the listening loop.
                while(true)
                {
                    Console.Write("Waiting for a connection... ");
            
                    // Perform a blocking call to accept requests.
                    // // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
            
                    data = null;
            
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
            
                    int i;
            
                    // Loop to receive all the data sent by the client.
                    while((i = stream.Read(bytes, 0, bytes.Length))!=0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
            
                        // Process the data sent by the client.
                        data = data.ToUpper();
            
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }
            
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
            
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        // Connect Socket to the remote endpoint using method Connect()
        public void Connect(Socket sender, IPEndPoint localEndPoint)
        {

        }

        public void SendMessage()
        {
            
        }

        public void AcceptClient(string IpAdresse)
        {
            
        }

        public void RecieveMessage()
        {
            
        }
    }
}