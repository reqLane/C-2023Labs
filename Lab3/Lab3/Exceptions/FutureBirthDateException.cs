using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Exceptions
{
    class FutureBirthDateException : BirthDateException
    {
        public FutureBirthDateException()
            : base("Birth date can't be from future")
        { }

        public FutureBirthDateException(string message) 
            : base(message) 
        { }
    }
}
