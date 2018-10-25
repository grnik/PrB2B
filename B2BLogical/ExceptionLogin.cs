using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BLogical
{
    public enum ExceptionLoginType { TokenError, LogOnError }
    public class ExceptionLogin : Exception
    {
        public ExceptionLoginType Type { get; private set; }

        internal ExceptionLogin(ExceptionLoginType type)
            : base()
        {
            Type = type;
        }
    }
}
