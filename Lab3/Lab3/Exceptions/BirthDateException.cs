using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Exceptions
{
    class BirthDateException : IncorrectInputException
    {
        public BirthDateException()
            : base("Incorrect birth date")
        { }

        public BirthDateException(string message)
            : base(message)
        { }
    }
}
