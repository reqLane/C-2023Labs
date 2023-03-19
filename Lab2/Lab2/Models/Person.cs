using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Models
{
    class Person
    {
        #region Constructors
        public Person(string name, string surname, string email, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthDate;
        }
        public Person(string name, string surname, string email) :
            this(name, surname, email, new DateTime(2003, 11, 25)) 
        { }
        public Person(string name, string surname, DateTime birthDate) :
            this(name, surname, "vlad5000191@gmail.com", birthDate)
        { }
        #endregion

        #region Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        #endregion
    }
}
