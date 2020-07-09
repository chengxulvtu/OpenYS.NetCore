using System;
using System.Collections.Generic;
using System.Text;

namespace OpenYS.NetCore
{
    public class OpenYSException : Exception
    {
        public OpenYSException(string message) : base(message)
        { }

        public int Code { get; private set; }

        public OpenYSException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
