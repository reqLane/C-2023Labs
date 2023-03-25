using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Exceptions
{
    class IncorrectInputException : Exception
    {
        public IncorrectInputException()
            : base("Incorrect input data")
        { }

        public IncorrectInputException(string message) 
            : base(message)
        { }
    }
}
