using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

        public void ReadData(TcpListener server)
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;
            try
            {
                // Enter the listening loop.
                while (true)
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
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
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
            catch (SocketException e)
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

        public void SendMessage( String serverAdress,int port)
        {
            TcpClient client = new TcpClient(serverAdress, port);
            Message message = new Message(24, "Chat App", "Hallo");

            Console.WriteLine("Text eingeben: ");
            message.Nachricht = Console.ReadLine();

            JsonConvert.ToString(message);
            
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message.Nachricht);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        public void RecieveMessage()
        {
            
        }
    }
}