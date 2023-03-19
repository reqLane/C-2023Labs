using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Models
{
    internal class UserInfo
    {
        #region Fields
        private DateTime _birthDate = DateTime.Today;
        private int? _age;
        private string _westernSign = "";
        private string _chineseSign = "";
        #endregion

        #region Properties
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }
        public int? Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public string WesternSign
        {
            get { return _westernSign; } 
            set { _westernSign = value; }
        }
        public string ChineseSign
        {
            get { return _chineseSign; }
            set { _chineseSign = value; }
        }
        #endregion
    }
}
