using System;
using Newtonsoft.Json;

namespace ChatApp
{
    public class Message
    {
        #region Properties

        private int _laenge;
        private String _application;
        private String _nickname;
        private string _nachricht;


        #endregion

        #region Accessors/Modifiers

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

        #endregion

        #region Constructors

        public Message(string application, string nickname)
        {
            Application = application;
            Nickname = nickname;
        }

        public Message()
        {
            this.Laenge = Laenge;
            this.Application = Application;
            this.Nickname = Nickname;
            this.Nachricht = Nachricht;
        }

        #endregion
        
        // konvertiert einen Nachrichtenstring in ein byte object
        public Byte[] ConvertMessageToByte()
        {
            string json = JsonConvert.SerializeObject(this);
            Laenge = json.Length;
            json = JsonConvert.SerializeObject(this);
            Laenge = json.Length;
            json = JsonConvert.SerializeObject(this);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
            return data;
        }

    }
}