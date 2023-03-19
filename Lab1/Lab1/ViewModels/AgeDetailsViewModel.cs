using Lab1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps.Serialization;

namespace Lab1.ViewModels
{
    internal class AgeDetailsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly UserInfo _userInfo = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Properties
        public DateTime BirthDate
        {
            get { return _userInfo.BirthDate; }
            set {
                if (IsCorrect(value))
                {
                    _userInfo.BirthDate = value;
                    OnPropertyChanged();
                    CalculateInfo();
                }
                else {
                    RefreshValues();
                }
            }
        }
        public int? Age
        {
            get { return _userInfo.Age; }
            set { _userInfo.Age = value; OnPropertyChanged(); }
        }
        public string WesternSign
        {
            get { return _userInfo.WesternSign; }
            set { _userInfo.WesternSign = value; OnPropertyChanged(); }
        }
        public string ChineseSign
        {
            get { return _userInfo.ChineseSign; }
            set { _userInfo.ChineseSign = value; OnPropertyChanged(); }
        }
        #endregion

        private static bool IsCorrect(DateTime date)
        {
            DateTime now = DateTime.Now;
            DateTime tooOld = new(now.Year - 135, now.Month, now.Day);

            if(DateTime.Compare(date, now) > 0
                || DateTime.Compare(tooOld, date) > 0)
            {
                MessageBox.Show("You couldn't have born in the future or 135 years ago");
                return false;
            }

            return true;
        }

        private void CalculateInfo()
        {
            ChangeAge();
            ChangeWesternSign();
            ChangeChineseSign();

            if (DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day == BirthDate.Day)
            {
                MessageBox.Show("Happy Birthday!");
            }
        }

        private void ChangeAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Compare(new DateTime(DateTime.Now.Year, BirthDate.Month, BirthDate.Day), DateTime.Now) > 0)
            {
                age -= 1;
            }
            Age = age;
        }

        private void ChangeWesternSign()
        {
            int day = BirthDate.Day;
            int month = BirthDate.Month;
            if ((day >= 21 && month == 1) || (day <= 19 && month == 2))
                WesternSign = "Aquarius";
            else if ((day >= 20 && month == 2) || (day <= 20 && month == 3))
                WesternSign = "Pisces";
            else if ((day >= 21 && month == 3) || (day <= 20 && month == 4))
                WesternSign = "Aries";
            else if ((day >= 21 && month == 4) || (day <= 21 && month == 5))
                WesternSign = "Taurus";
            else if ((day >= 22 && month == 5) || (day <= 21 && month == 6))
                WesternSign = "Gemini";
            else if ((day >= 21 && month == 6) || (day <= 23 && month == 7))
                WesternSign = "Cancer";
            else if ((day >= 24 && month == 7) || (day <= 23 && month == 8))
                WesternSign = "Leo";
            else if ((day >= 24 && month == 8) || (day <= 23 && month == 9))
                WesternSign = "Virgo";
            else if ((day >= 24 && month == 9) || (day <= 23 && month == 10))
                WesternSign = "Libra";
            else if ((day >= 24 && month == 10) || (day <= 22 && month == 11))
                WesternSign = "Scorpio";
            else if ((day >= 23 && month == 11) || (day <= 21 && month == 12))
                WesternSign = "Sagittarius";
            else if ((day >= 22 && month == 12) || (day <= 20 && month == 1))
                WesternSign = "Capricorn";
        }

        private void ChangeChineseSign()
        {
            switch (BirthDate.Year % 12)
            {
                case 0:
                    ChineseSign = "Monkey";
                    break;
                case 1:
                    ChineseSign = "Rooster";
                    break;
                case 2:
                    ChineseSign = "Dog";
                    break;
                case 3:
                    ChineseSign = "Pig";
                    break;
                case 4:
                    ChineseSign = "Rat";
                    break;
                case 5:
                    ChineseSign = "Ox";
                    break;
                case 6:
                    ChineseSign = "Tiger";
                    break;
                case 7:
                    ChineseSign = "Rabbit";
                    break;
                case 8:
                    ChineseSign = "Dragon";
                    break;
                case 9:
                    ChineseSign = "Snake";
                    break;
                case 10:
                    ChineseSign = "Horse";
                    break;
                case 11:
                    ChineseSign = "Goat";
                    break;
            }
        }

        private void RefreshValues()
        {
            _userInfo.BirthDate = DateTime.Now;
            Age = null;
            WesternSign = "";
            ChineseSign = "";
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
