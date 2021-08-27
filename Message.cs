using System;

namespace ChatApp
{
    public class Message
    {
        private string _text;
        private string _sender;
        private string _reciever;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public string Sender
        {
            get => _sender;
            set => _sender = value;
        }

        public string Reciever
        {
            get => _reciever;
            set => _reciever = value;
        }
        
        public Message(string text, string sender, string reciever)
        {
            _text = text;
            _sender = sender;
            _reciever = reciever;
        }

        public Message()
        {
            Text = "";
            Sender = "";
            Reciever = "";
        }

        public void DisplayMessage()
        {
            Console.WriteLine(Text);
        }
    }
}