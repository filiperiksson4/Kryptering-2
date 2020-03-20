using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class SMessage
    {
        string author;
        string cryptMethod;
        string text;

        //Constructors
        public SMessage(string author, string text)
        {
            this.author = author;
            this.text = text;
        }
        public SMessage() { 
        }
        public string Author
        {
            set { author = value; }
            get { return author; }
        }
        public string CryptMethod
        {
            set { cryptMethod = value; }
            get { return cryptMethod; }
        }
        public string Text
        {
            set { text = value; }
            get { return text; }
        }
    }
}
