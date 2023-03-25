using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Exceptions
{
    class IncorrectEmailException : IncorrectInputException
    {
        public IncorrectEmailException()
            :base("Email is incorrect")
        {
            EmailValue = "";
        }

        public IncorrectEmailException(string email)
            :base($"{email} is incorrect email")
        {
            EmailValue = email;
        }

        public string EmailValue { get; private set; }
    }
}
