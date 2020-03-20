using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektKryptering
{
    class LoopException : Exception
    {
        public LoopException() { }
        public LoopException(string text) : base(text) { }

    }
}
