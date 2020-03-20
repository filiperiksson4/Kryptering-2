using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ProjektKryptering
{
    class Program
    {
        static void Main(string[] args)
        {
            string adress = "127.0.0.1";
            int port = 8001;


            //Anslut till servern
            Console.WriteLine("Ansluter..");
            TcpClient client = new TcpClient();
            client.Connect(adress, port);
            NetworkStream tcpStream = client.GetStream();

            //loop start
            try
            {
                while (true)
                {
                    Menu(client, tcpStream);
                }
            }
            catch 
            {
                //Stäng Avslutningen
                client.Close();
                Console.WriteLine("Anslutning stängd");
                Console.ReadKey();
            }


        }
        public static void SendMessage(Message msg, NetworkStream stream, UnicodeEncoding encoder)
        {
                string sendText = msg.Author + "|" + msg.CryptMethod + "|" + msg.Text;
                Byte[] byteMsg = encoder.GetBytes(sendText);
                stream.Write(byteMsg, 0, byteMsg.Length);
        }
        public static void SendCommando(int c, NetworkStream stream, UnicodeEncoding encoder)
        {
            string command = c.ToString();
            Byte[] byteMsg = encoder.GetBytes(command);
            stream.Write(byteMsg, 0, byteMsg.Length);
        }
        public static Message CreateMessage()
        {
            Console.Write("Skriv meddelande: ");
            string msg = Console.ReadLine();
            Console.Write("Skriv ditt namn: ");
            string author = Console.ReadLine();
            return new Message(author, msg);
        }
        public static string RecieveMessage(NetworkStream stream)
        {
            //Ta emot meddelande
            Byte[] msgRecieved = new Byte[256];
            int msgRecievedSize = stream.Read(msgRecieved, 0, msgRecieved.Length);

            //Konvertera meddelande
            string read = "";
            for (int i = 0; i < msgRecievedSize; i++)
            {
                read += Convert.ToChar(msgRecieved[i]);
            }
            return read;
        }
        public static void Menu(TcpClient client, NetworkStream stream)
        {
            int key = 0;
            UnicodeEncoding encoder = new UnicodeEncoding();

            Console.WriteLine("Välj ett alternativ");
            Console.WriteLine("1. Skicka meddelande");
            Console.WriteLine("2. Läs upp alla meddelanden");
            Console.WriteLine("3. Avsluta");
            //Då vi endast har 3 val säkerställer vi att inget annat kan skrivas in
            while (key != 1 && key != 2 && key != 3)
            {
                try
                {
                    //Registrerar knapptryck och konverterar det till int
                    key = (int.Parse(Console.ReadKey(true).KeyChar.ToString()));
                    Console.WriteLine(key);

                }
                catch
                {
                    Console.WriteLine("Skriv om!");
                }
            }
            Console.Clear();
            SendCommando(key, stream, encoder);
            switch (key)
            {
                case 1:
                    //Skriv in meddelande
                    Message message = CreateMessage();
                    Console.Clear();
                    //Skicka iväg meddelande
                    Console.WriteLine("Skickar...");
                    message.Encrypt();
                    SendMessage(message, stream, encoder);
                    break;
                case 2:
                    RecieveAllMessages(stream, encoder);
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 3:
                    throw new LoopException();
            }
        }
        public static void RecieveAllMessages(NetworkStream stream, UnicodeEncoding encoder)
        {
            string msg;
            while (true)
            {
                msg = RecieveMessage(stream);
                
                //Svara servern att man tagit emot meddelandet
                stream.Write(encoder.GetBytes("1"), 0, 1);
                if (msg != "Klar")
                {
                    Console.WriteLine(Message.Decrypt(msg));
                    
                }
                else
                {
                    Console.WriteLine("Inga meddelanden kvar");
                    break;
                }

            }
        }
    }
}
