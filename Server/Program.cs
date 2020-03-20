using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8001;
            //Skapa TCPListener och börja lyssna och vänta på anslutning
            TcpListener tcpListener = new TcpListener(ip, port);
            tcpListener.Start();
            Console.WriteLine("Väntar på anslutning...");
            //Någon försöker ansluta
            Socket socket = tcpListener.AcceptSocket();
            Console.WriteLine("Anslutning accepterad från " + socket.RemoteEndPoint);
            while (true)
            {
                try
                {
                    //Ta emot meddelande
                    RecieveMessage(socket);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                  //  Console.Clear();
                    Console.WriteLine("Anslutning från klient avbruten");
                    break;
                }
            }

            Console.ReadKey();
            //Avsluta anslutningen
            //socket.Close();
        }

        public static void RecieveMessage(Socket socket)
        {
            //Ta emot meddelande
            Byte[] bits = new Byte[256];
            int msgSize = socket.Receive(bits);
            Console.WriteLine("Meddelande mottogs...");

            //Konvertera till string och skriv ut
            string recievedText = "";
            for (int j = 0; j < msgSize; j++)
            {
                recievedText += Convert.ToChar(bits[j]);
            }
            Console.WriteLine("Meddelande: " + recievedText);

            switch (recievedText)
            {
                case "1":
                    RecieveMessage(socket);
                    break;
                case "2":
                    PresentMessages(socket);
                    break;
                case "3":
                    break;
                default:
                    //Konverterar den skickade stringen till ett message-objekt och sedan sparar i xml-filen
                    SaveInXml(TextToMessage(recievedText));
                    break;
            }
        }
        public static void SaveInXml(SMessage msg)
        {
            XmlDocument doc = new XmlDocument();
            while (true)
            {
                try
                {
                    doc.Load(@"C:\Users\filip.eriksson4\source\repos\Server\messages.xml");

                    XmlNode message = doc.CreateElement("Message");

                    //Skapa de element som meddelandet ska innehålla
                    XmlElement author = doc.CreateElement("Author");
                    author.InnerText = msg.Author;
                    XmlElement cryptMethod = doc.CreateElement("CryptationMethod");
                    cryptMethod.InnerText = msg.CryptMethod;
                    XmlElement text = doc.CreateElement("Text");
                    text.InnerText = msg.Text;

                    //Lägg in alla egenskaper på meddelandet
                    message.AppendChild(author);
                    message.AppendChild(cryptMethod);
                    message.AppendChild(text);

                    //Söker dokumentet efter huvudet messages sedan lägger in meddelande
                    doc.SelectSingleNode("Messages").AppendChild(message);
                    doc.Save(@"C:\Users\filip.eriksson4\source\repos\Server\messages.xml");
                    break;

                }
               
                catch
                {
                    CreateXmlfile(doc);
                }
            }
        }
        public static void CreateXmlfile(XmlDocument doc)
        {
            //Skapa deklarationen till dokumente
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-16", null);
            doc.AppendChild(declaration);
            //Skapa överhuvudet Meddelanden
            XmlNode messages = doc.CreateElement("Messages");
            doc.AppendChild(messages);
            doc.Save(@"C:\Users\filip.eriksson4\source\repos\Server\messages.xml");

        }
        public static void PresentMessages(Socket socket)
        {
            XmlDocument doc = new XmlDocument();
            UnicodeEncoding uni = new UnicodeEncoding();
            try
            {
                doc.Load(@"C:\Users\filip.eriksson4\source\repos\Server\messages.xml");
                XmlNodeList messagesList = doc.SelectNodes("Messages/Message");
                foreach (XmlNode node in messagesList)
                {
                    string sendText = /*node.SelectSingleNode("Author").InnerText + ": " + */node.SelectSingleNode("Text").InnerText;
                    Byte[] send = uni.GetBytes(sendText);
                    socket.Send(send);
                    socket.Receive(new Byte[256]);
                }
                socket.Send(uni.GetBytes("Klar"));
            }
            
            catch
            {
                Console.WriteLine("Messages is empty!");
                socket.Send(uni.GetBytes("Klar"));
            }



        }
        public static SMessage TextToMessage(string text)
        {
            SMessage msg = new SMessage();
            string[] msgString = text.Split('|');
            Console.WriteLine(msgString[0]);
            msg.Author = msgString[0];
            Console.WriteLine(msgString[1]);
            msg.CryptMethod = msgString[1];
            msg.Text = msgString[2];

            return msg;
        }

    }
}
