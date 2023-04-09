using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Models
{
    class Person
    {
        #region Fields
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate;
        private bool? _isAdult;
        private string? _sunSign;
        private string? _chineseSign;
        private bool? _isBirthday;
        #endregion

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
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public bool? IsAdult
        {
            get { return _isAdult; }
            set { _isAdult = value; }
        }
        public string? SunSign
        {
            get { return _sunSign; }
            set { _sunSign = value; }
        }
        public string? ChineseSign
        {
            get { return _chineseSign; }
            set { _chineseSign = value; }
        }
        public bool? IsBirthday
        {
            get { return _isBirthday; }
            set { _isBirthday = value; }
        }
        #endregion
    }
}
