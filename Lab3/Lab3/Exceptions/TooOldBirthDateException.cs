using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Exceptions
{
    class TooOldBirthDateException : BirthDateException
    {
        public TooOldBirthDateException()
            : base("Birth date can't be too old")
        { }

        public TooOldBirthDateException(string message)
            : base(message)
        { }
    }
}
