using System;
using System.Collections.Generic;
using System.Text;

namespace Console_GetMyInvoice_Script.Code
{
    class UserInfoException : Exception
    {
        public UserInfoException()
        {
        }

        public UserInfoException(string message) : base(message)
        {
        }

        public UserInfoException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
