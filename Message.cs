using System;
using Newtonsoft.Json;

namespace ChatApp
{
    public class Message
    {
        private int _laenge;
        private String _application;
        private String _nickname;
        private string _nachricht;

        public int Laenge
        {
            get => _laenge;
            set => _laenge = value;
        }

        public string Application
        {
            get => _application;
            set => _application = value;
        }

        public string Nickname
        {
            get => _nickname;
            set => _nickname = value;
        }

        public string Nachricht
        {
            get => _nachricht;
            set => _nachricht = value;
        }
        
        public Message(string application, string nickname)
        {
            _application = application;
            _nickname = nickname;
        }

        public Message()
        {
            this.Laenge = Laenge;
            this.Application = Application;
            this.Nickname = Nickname;
            this.Nachricht = Nachricht;
        }

        public Byte[] ConvertMessageToByte()
        {
            string json = JsonConvert.SerializeObject(this);
            Console.WriteLine("json message {0}", json);
            Laenge = json.Length;
            json = JsonConvert.SerializeObject(this);
            Console.WriteLine("json message1 {0}", json);
            Laenge = json.Length;
            json = JsonConvert.SerializeObject(this);
            Console.WriteLine("json message2 {0}", json);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
            Console.WriteLine(data);

            return data;
        }

    }
}