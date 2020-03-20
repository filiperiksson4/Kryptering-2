using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektKryptering
{
    class Message
    {
        string author;
        static string cryptMethod = "Single Dislocation";
        string text;

        //Constructors
        public Message(string author, string text)
        {
            this.author = author;
            this.text = text;
        }
        public string Author
        {
            set { author = value; }
            get { return author; }
        }
        public string CryptMethod
        {
            get { return cryptMethod; }
        }
        public string Text
        {
            set { text = value; }
            get { return text; }
        }

        // Encryption
        public void Encrypt()
        {
            if(text.Length > 1)
            {
                //Exekvera krypteringsmetod
                string encryptedMessage = "";
                //Början av stringen är den sista bokstaven i meddelandet
                encryptedMessage += text[text.Length - 1];
                //Length -1 då sista bokstaven inte ska med
                for (int i = 0; i < text.Length-1; i++)
                {
                    encryptedMessage += text[i];
                }

                //Ersätt texten med den krypterad texten
                text = encryptedMessage;
            }
            
        }

        public static string Decrypt(string s)
        {
            string decryptedMessage = "";
            for (int i = 1; i < s.Length; i++)
            {
                decryptedMessage += s[i];
            }
            decryptedMessage += s[0];
            s = decryptedMessage;
            return s;
        }
    }
}
